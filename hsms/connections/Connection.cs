#region Usings
using Semi.Hsms.config;
using Semi.Hsms.Messages;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

#endregion

namespace Semi.Hsms.connections
{
	/// <summary>
	/// 
	/// </summary>
	public partial class Connection
	{
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		protected State _state;
		/// <summary>
		/// 
		/// </summary>
		protected Configurator _config;
		/// <summary>
		/// 
		/// </summary>
		protected Timer _timerT36;
		/// <summary>
		/// 
		/// </summary>
		protected Timer _timerT5ConnectSeparationTimeout;
		/// <summary>
		/// 
		/// </summary>
		protected Timer _timerT6ControlTimeout;
		/// <summary>
		/// 
		/// </summary>
		protected Timer _timerT7ConnectionIdleTimeout;
		/// <summary>
		/// 
		/// </summary>
		protected bool _bRun;
		/// <summary>
		/// 
		/// </summary>
		protected Socket _socket;
		/// <summary>
		/// 
		/// </summary>
		protected object _syncObject = new object();
		/// <summary>
		/// 
		/// </summary>
		protected Dictionary<uint, Transaction> _transactions = new Dictionary<uint, Transaction>();
		/// <summary>
		/// 
		/// </summary>
		protected Queue<Message> _queueToSend = new Queue<Message>();

		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public EventDispatcher Events { get; } = new EventDispatcher();

		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurator"></param>
		public Connection( Configurator configurator )
		{
			_config = configurator;

			_timerT36 = new Timer( s => CheckTransactions(),
							null, 1000, Timeout.Infinite );

			_timerT6ControlTimeout = new Timer( s => CloseConnection(),
							null, Timeout.Infinite, Timeout.Infinite );

			_timerT7ConnectionIdleTimeout = new Timer( s => CloseConnection(),
							null, Timeout.Infinite, Timeout.Infinite );

			if( _config.Mode == ConnectionMode.Active )
			{
				_timerT5ConnectSeparationTimeout = new Timer( s => TryConnect(),
												null, Timeout.Infinite, Timeout.Infinite );
			}
			else
			{
				_timerT5ConnectSeparationTimeout = new Timer( s => TryListen(),
																				null, Timeout.Infinite, Timeout.Infinite );
			}
		}

		#endregion

		#region Class methods
		/// <summary>
		/// 
		/// </summary>
		public void Start()
		{
			lock( _syncObject )
			{
				if( _bRun )
					return;

				_bRun = true;

				_timerT5ConnectSeparationTimeout.Change( 0, Timeout.Infinite );
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void Stop()
		{
			lock( _syncObject )
			{
				if( !_bRun )
					return;

				_bRun = false;

				CloseConnection();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected void CloseConnection()
		{
			lock( _syncObject )
			{
				_socket?.Close();
				_socket = null;

				_state = State.NotConnected;

				_queueToSend.Clear();

				Events.Add( EventType.Disconnected, _config.Port );

				var t5FireTime = ( _bRun ) ? _config.T5 * 1000 : Timeout.Infinite;
				_timerT5ConnectSeparationTimeout.Change( t5FireTime, Timeout.Infinite );

				_timerT6ControlTimeout.Change( Timeout.Infinite, Timeout.Infinite );
				_timerT7ConnectionIdleTimeout.Change( Timeout.Infinite, Timeout.Infinite );
			}
		}

		#endregion

	}
}
