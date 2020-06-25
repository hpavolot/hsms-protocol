#region Usings
using Semi.Hsms.Messages;
using System;
using System.Collections.Generic;
using System.Threading;
#endregion

namespace Semi.Hsms
{
	/// <summary>
	/// 
	/// </summary>
	public class EventDispatcher
	{
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		private readonly Queue<EventEntry> _eventQueue;
		/// <summary>
		/// 
		/// </summary>
		private readonly object _syncObject = new object();
		#endregion

		#region Class events
		public event EventHandler IsConnecting;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler IsListening;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler Connected;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler Disconnected;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<Message> Sent;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<Message> Received;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<Message> T3Timeout;
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		public EventDispatcher()
		{
			_eventQueue = new Queue<EventEntry>();
			
			var ts = new ThreadStart( EventProcessor );

			var t = new Thread( ts )
			{
				IsBackground = true// !!!
			};

			t.Start();
		}
		#endregion

		#region Class methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="m"></param>
		public void Add( EventType type, object arg = null )
		{
			lock( _syncObject )
			{
				_eventQueue.Enqueue( new EventEntry( type, arg ) );

				Monitor.Pulse( _syncObject );
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void EventProcessor()
		{
			while( true )
			{
				EventEntry entry = null;

				lock( _syncObject )
				{
					if( _eventQueue.Count != 0 )
					{
						entry = _eventQueue.Dequeue();
					}
					else
					{
						Monitor.Wait( _syncObject );
					}
				}

				if( null != entry ) 
				{
					CompleteProcessing( entry );
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="et"></param>
		/// <param name="arg"></param>
		private void CompleteProcessing( EventEntry entry ) 
		{
			switch( entry.Type )
			{
				case EventType.IsConnecting:
					IsConnecting?.Invoke(this, EventArgs.Empty);
					break;

				case EventType.IsListening:
					IsListening?.Invoke(this, EventArgs.Empty);
					break;

				case EventType.Connected:
					Connected?.Invoke( this, EventArgs.Empty );
					break;

				case EventType.Disconnected:
					Disconnected?.Invoke( this, EventArgs.Empty );
					break;

				case EventType.Sent:
					Sent?.Invoke( this, ( Message )entry.Argument );
					break;

				case EventType.Received:
					Received?.Invoke( this, ( Message )entry.Argument );
					break;

				case EventType.T3Timeout:
					T3Timeout?.Invoke( this, ( Message )entry.Argument );
					break;

				default: break;
			}
		}
		#endregion

		#region Class internal structs
		/// <summary>
		/// 
		/// </summary>
		private class EventEntry 
		{
			#region Class properties
			/// <summary>
			/// 
			/// </summary>
			public EventType Type { get; set; }
			/// <summary>
			/// 
			/// </summary>
			public object Argument{ get; set; }
			#endregion

			#region Class initialization
			/// <summary>
			/// 
			/// </summary>
			/// <param name="t"></param>
			/// <param name="arg"></param>
			public EventEntry( EventType t, object arg = null ) 
			{
				Type = t;
				Argument = arg;
			}
			#endregion
		}
		#endregion
	}
}
