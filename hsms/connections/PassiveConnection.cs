#region Usings
using Semi.Hsms.connections;
using Semi.Hsms.Messages;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
#endregion

namespace Semi.Hsms.Connections
{
	/// <summary>
	/// 
	/// </summary>
	public class PassiveConnection:Connection
	{
		#region Class members
		Socket _serverSocket;
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurator"></param>
		public PassiveConnection( Configurator configurator):base(configurator)
		{
			_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			_timerT5ConnectSeparationTimeout = new Timer(s => TryListen(),
				null, Timeout.Infinite, Timeout.Infinite);
		}
		#endregion

		#region Class methods
		/// <summary>
		/// 
		/// </summary>
		public override void Start()
		{
			lock (_mLock)
			{
				if (_bRun)
					return;

				_bRun = true;

				TryListen();
			}
		}
		/// <summary>
		/// 
		/// </summary>

		#endregion

		#region Class 'Connection' methods
		/// <summary>
		/// 
		/// </summary>
		private void TryListen()
		{
			Console.WriteLine("Waiting for a connection...");

			lock (_mLock)
			{
				var ea = new SocketAsyncEventArgs()
				{
					RemoteEndPoint = new IPEndPoint(_config.IP, _config.Port)
				};

				ea.Completed += OnConnectionCompleted;
				 
				if (!_serverSocket.IsBound)
					_serverSocket.Bind(ea.RemoteEndPoint);

				_serverSocket.Listen(10);
				_serverSocket.AcceptAsync(ea);
			}
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

				if( null != e.AcceptSocket )
				{
					_socket = e.AcceptSocket;

					_state = State.ConnectedNotSelected;

					_timerT7ConnectionIdleTimeout.Change( _config.T7 * 1000, Timeout.Infinite );

					_timerT5ConnectSeparationTimeout.Change( Timeout.Infinite, Timeout.Infinite );

					Console.WriteLine( "connected !!!" );

					BeginRecv();
				}
				else
				{
					_timerT5ConnectSeparationTimeout.Change( _config.T5 * 1000, Timeout.Infinite );
				}
			}
		}
		#endregion
	}
}
