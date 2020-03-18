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
	public class Connection
	{
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		private Configurator _config;
		/// <summary>
		/// 
		/// </summary>
		private Timer _connectionTimer;
		/// <summary>
		/// 
		/// </summary>
		private bool _bRun;
		/// <summary>
		/// 
		/// </summary>
		private Socket _socket;
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurator"></param>
		public Connection(Configurator configurator)
		{
			_config = configurator;

			_connectionTimer = new Timer(s => TryConnect(),
				null, Timeout.Infinite, Timeout.Infinite);
		}
		#endregion

		#region Class methods
		/// <summary>
		/// 
		/// </summary>
		public void Start()
		{
			_bRun = true;

			TryConnect();
		}
		/// <summary>
		/// 
		/// </summary>
		public void Stop()
		{
			_bRun = false;

			_connectionTimer.Change(Timeout.Infinite, Timeout.Infinite);
		}
		#endregion

		#region Class 'Connection' methods
		/// <summary>
		/// 
		/// </summary>
		private void TryConnect()
		{
			Debug.WriteLine("trying to connect...");

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
			if (!_bRun)
				return;

			if (null != e.ConnectSocket)
			{
				_socket = e.ConnectSocket;

				Debug.WriteLine("connected !!!");

				Send(new SelectReq(4, 9));

				Receive(_socket);
				
			}
			else
			{
				_connectionTimer.Change(1000, Timeout.Infinite);
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
			Debug.WriteLine($"sending: {m.ToString()}");

			var arr = Coder.Encode(m);

			_socket.Send(arr);
		}
		#endregion

		#region Class 'Receive' methods
		public class StateObject
		{
			public Socket workSocket = null;
			public const int BufferSize = 256;
			public byte[] buffer = new byte[BufferSize];
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="socket"></param>
		private void Receive(Socket socket)
		{
			StateObject state = new StateObject();
			state.workSocket = socket;

			socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
			new AsyncCallback(ReceiveCallback), state);
		}

		private static void ReceiveCallback(IAsyncResult ar)
		{
			StateObject state = (StateObject)ar.AsyncState;
			Socket socket = state.workSocket;

			int bytesRead = socket.EndReceive(ar);

			var msg = Coder.Decode(state.buffer);
			Console.WriteLine(msg.ToString());

			for (int i = 0; i < bytesRead; i++)
			{
				Console.Write(" " +state.buffer[i]);
			}

			if (bytesRead > 0)
			{
				
				socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
					new AsyncCallback(ReceiveCallback), state);
			}
		}
				#endregion
			}
}
