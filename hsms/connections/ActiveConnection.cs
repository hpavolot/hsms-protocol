#region Usings
using Semi.Hsms.Messages;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
#endregion

namespace Semi.Hsms.Connections
{
	/// <summary>
	/// 
	/// </summary>
	public class ActiveConnection
	{
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		private State _state;
		/// <summary>
		/// 
		/// </summary>
		private Configurator _config;
		/// <summary>
		/// 
		/// </summary>
		private Timer _timerT5ConnectSeparationTimeout;
		/// <summary>
		/// 
		/// </summary>
		private Timer _timerT7ConnectionIdleTimeout;
		/// <summary>
		/// 
		/// </summary>
		private bool _bRun;
		/// <summary>
		/// 
		/// </summary>
		private Socket _socket;
		/// <summary>
		/// 
		/// </summary>
		private Object _mLock = new Object();
		#endregion

		#region Class events
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler Connected;

		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurator"></param>
		public ActiveConnection(Configurator configurator)
		{
			_config = configurator;

			_timerT5ConnectSeparationTimeout = new Timer(s => TryConnect(),
				null, Timeout.Infinite, Timeout.Infinite);

			_timerT7ConnectionIdleTimeout = new Timer(s => CloseConnection(),
				null, Timeout.Infinite, Timeout.Infinite);
		}
		#endregion

		#region Class methods
		/// <summary>
		/// 
		/// </summary>
		public void Start()
		{
			lock (_mLock)
			{
				if (_bRun)
					return;

				_bRun = true;

				TryConnect();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void Stop()
		{
			Console.WriteLine("Stopped");

			lock (_mLock)
			{
				if (!_bRun)
					return;

				_bRun = false;

				CloseConnection();
				//_timerT5ConnectSeparationTimeout.Change(Timeout.Infinite, Timeout.Infinite);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void CloseConnection()
		{
			Console.WriteLine( "Reconnecting" );

			lock( _mLock )
			{
				_socket.Close();
				_socket = null;

				_state = State.NotConnected;

				var t5FireTime = ( _bRun ) ? _config.T5 * 1000 : Timeout.Infinite;
				_timerT5ConnectSeparationTimeout.Change( t5FireTime, Timeout.Infinite );

				_timerT7ConnectionIdleTimeout.Change( Timeout.Infinite, Timeout.Infinite );
			}
		}
		#endregion

		#region Class 'Connection' methods
		/// <summary>
		/// 
		/// </summary>
		private void TryConnect()
		{
			//TODO: add lock !!!

			Console.WriteLine("trying to connect...");

			var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			var ea = new SocketAsyncEventArgs()
			{
				RemoteEndPoint = new IPEndPoint(_config.IP, _config.Port)
			};

			ea.Completed += OnConnectionCompleted;

			s.NoDelay = true;

			s.ConnectAsync(ea);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnConnectionCompleted(object sender, SocketAsyncEventArgs e)
		{
			lock( _mLock ) 
			{
				if( !_bRun )
					return;

				if( null != e.ConnectSocket )
				{
					_socket = e.ConnectSocket;

					_state = State.ConnectedNotSelected;

					_timerT7ConnectionIdleTimeout.Change( _config.T7 * 1000, Timeout.Infinite );

					_timerT5ConnectSeparationTimeout.Change( Timeout.Infinite, Timeout.Infinite );

					Console.WriteLine( "connected !!!" );

					BeginRecv();

					//Send( new SelectReq( 1, 9 ) );
				}
				else
				{
					_timerT5ConnectSeparationTimeout.Change( _config.T5 * 1000, Timeout.Infinite );
				}
			}
		}
		#endregion

		#region Class 'Send' methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="m"></param>
		public void Send(Message m)
		{
			lock (_mLock)
			{
				if( !_bRun )
					return;

				Debug.WriteLine($"sending: {m.ToString()}");

				var arr = Coder.Encode(m);

				_socket.Send(arr);
			}
		}
		#endregion

		#region Class 'Receive' methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="socket"></param>
		private void BeginRecv()
		{
			lock (_mLock)
			{
				if (!_bRun)
					return;

				var buffer = new byte[Coder.MESSAGE_PREFIX_LEN];

				_socket.BeginReceive(buffer, 0, buffer.Length,
					 SocketFlags.None, OnRecv, buffer);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ar"></param>
		private void OnRecv(IAsyncResult ar)
		{
			// think about lock !!!
			var bClose = false;

			try
			{
				var buffer = CompleteRecv( ar );

				bClose = ( null == buffer );

				if( bClose )
					return;

				var m = Coder.Decode( buffer );

				AnalyzeRecv( m );

				BeginRecv();
			}
			catch //(Exception e)
			{
				bClose = true;
			}
			finally
			{
				if( bClose ) 
				{
					CloseConnection();
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ar"></param>
		protected virtual byte[] CompleteRecv(IAsyncResult ar)
		{
			int count = _socket.EndReceive(ar);

			if (count != Coder.MESSAGE_PREFIX_LEN)
				return null;

			var prefix = ar.AsyncState as byte[];
			Array.Reverse(prefix);
			var len = BitConverter.ToInt32(prefix, 0);

			var buffer = new byte[len];

			int iBytesToReadLeft = len;
			int iOffset = 0;

			while (iBytesToReadLeft > 0)
			{
				int iRecvCount = _socket.Receive(buffer, iOffset, iBytesToReadLeft, SocketFlags.None);

				if (0 == iRecvCount)
					break;

				iBytesToReadLeft -= iRecvCount;
				iOffset += iRecvCount;
			}

			if (iBytesToReadLeft > 0)
				throw new Exception("invalid message length");

			return buffer;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		protected virtual void AnalyzeRecv(Message m)
		{
			if (m is null)
				return;

			switch (m.Type)
			{
				case MessageType.SelectRsp:
					HandleSelectRsp(m as SelectRsp);
					break;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="m"></param>
		private void HandleSelectRsp(SelectRsp m)
		{
			Console.WriteLine( "Connected Selected" );

			_state = State.ConnectedSelected;
			
			_timerT7ConnectionIdleTimeout.Change(Timeout.Infinite, Timeout.Infinite);

			Connected?.Invoke(this, EventArgs.Empty);
		}
		
		#endregion

		#region Class internal structs
		/// <summary>
		/// 
		/// </summary>
		internal enum State
		{
			#region Class properties
			/// <summary>
			/// 
			/// </summary>
			NotConnected,
			/// <summary>
			/// 
			/// </summary>
			ConnectedNotSelected,
			/// <summary>
			/// 
			/// </summary>
			ConnectedSelected,
			#endregion
		}
		#endregion
	}
}
