#region Usings
using Semi.Hsms.config;
using Semi.Hsms.Messages;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
#endregion

namespace Semi.Hsms.connections
{
	/// <summary>
	/// 
	/// </summary>
	public class ActiveConnection : Connection
	{
		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nic"></param>
		public ActiveConnection( Configurator configurator  )
			: base( configurator )
		{

		}
		#endregion

		#region Class 'Active Mode' methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="state"></param>
		protected override void OnT5Expired( object state ) => TryConnect();
		/// <summary>
		/// 
		/// </summary>
		public void TryConnect()
		{
			lock( _syncObject )
			{
				Events.Add( EventType.Connecting, _config.Port );

				var s = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

				var ea = new SocketAsyncEventArgs()
				{
					RemoteEndPoint = new IPEndPoint( _config.IP, _config.Port )
				};

				ea.Completed += OnConnectionCompleted;

				s.NoDelay = true;

				s.ConnectAsync( ea );
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnConnectionCompleted( object sender, SocketAsyncEventArgs e )
		{
			lock( _syncObject )
			{
				if( !_bRun )
					return;

				if( null != e.ConnectSocket )
				{
					_socket = e.ConnectSocket;

					_state = State.ConnectedNotSelected;

					var t = new Thread( MessageProcessor );
					t.IsBackground = true;
					t.Start();

					_timerT5ConnectSeparationTimeout.Change( Timeout.Infinite, Timeout.Infinite );

					BeginRecv();

					Send( new SelectReq( 1, Message.NextContext ) );
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
