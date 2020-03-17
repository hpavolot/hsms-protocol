#region Usings
using Semi.Hsms.Messages;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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
		public Connection( Configurator configurator)
		{
			_config = configurator;

			_connectionTimer = new Timer( s => TryConnect(),
				null, Timeout.Infinite, Timeout.Infinite );
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

			_connectionTimer.Change( Timeout.Infinite, Timeout.Infinite );
		}
		#endregion

		#region Class 'Connection' methods
		/// <summary>
		/// 
		/// </summary>
		private void TryConnect()
		{
			Debug.WriteLine( "trying to connect..." );

			var s = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

			var ea = new SocketAsyncEventArgs() 
			{
				RemoteEndPoint = new IPEndPoint( _config.IP, _config.Port )
			};

			ea.Completed += OnConnectionCompleted;

			s.NoDelay = true;

			s.ConnectAsync( ea );
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnConnectionCompleted( object sender, SocketAsyncEventArgs e )
		{
			if( !_bRun )
				return;

			if( null != e.ConnectSocket )
			{
				_socket = e.ConnectSocket;

				Debug.WriteLine( "connected !!!" );

				Send( new SelectReq( 1, 7 ) );

				//_socket.BeginReceive
			}
			else 
			{
				_connectionTimer.Change( 1000, Timeout.Infinite );
			}
		}
		#endregion

		#region Class 'Send' methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="m"></param>
		public void Send( Message m ) 
		{
			Debug.WriteLine( $"sending: {m.ToString()}" );

			var arr = Coder.Encode( m );

			_socket.Send( arr );
		}
		#endregion
	}
}
