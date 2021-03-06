﻿#region Usings
using Semi.Hsms.config;
using Semi.Hsms.Messages;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
#endregion

namespace Semi.Hsms.connections
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class Connection
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
		protected Connection( Configurator configurator )
		{
			_config = configurator;

			_timerT36 = new Timer( s => CheckTransactions(), null, 1000, Timeout.Infinite );

			_timerT6ControlTimeout = new Timer( s => CloseConnection(),
        null, Timeout.Infinite, Timeout.Infinite );

			_timerT7ConnectionIdleTimeout = new Timer( s => CloseConnection(),
        null, Timeout.Infinite, Timeout.Infinite );

      _timerT5ConnectSeparationTimeout = new Timer( OnT5Expired, null, Timeout.Infinite, Timeout.Infinite );
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Connection Create( Configurator c ) 
    {
      return ( c.Mode == ConnectionMode.Active ) ?
        new ActiveConnection( c ) : ( Connection )new PassiveConnection( c );
    }
		#endregion

		#region Class public methods
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
        if( null == _socket )
          return;

        _socket.Close();
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

    #region Class 'Timeout' methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    protected abstract void OnT5Expired( object state );
    /// <summary>
    /// 
    /// </summary>
    private void CheckTransactions()
    {
      lock( _syncObject )
      {
        if( !_bRun )
          return;

        var now = DateTime.Now;
        var toRemove = new List<Transaction>();

        try
        {
          foreach( var p in _transactions )
          {
            var t = p.Value;
            var m = t.Message;

            if( m.Type == MessageType.DataMessage )
            {
              if( TimeSpan.FromSeconds( _config.T3 ) < now - t.Timestamp )
              {
                toRemove.Add( t );
              }
            }
          }

          foreach( var key in toRemove )
          {
            _transactions.Remove( key.Message.Context );

            Events.Add( EventType.T3Timeout, key.Message );
          }
        }
        finally
        {
          _timerT36.Change( 1000, Timeout.Infinite );
        }
      }
    }
    #endregion

    #region Class 'Receive' methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="socket"></param>
    protected void BeginRecv()
    {
      //lock( _syncObject )
      //{
      //  if( !_bRun )
      //    return;

      //  var buffer = new byte [ Coder.MESSAGE_PREFIX_LEN ];

      //  _socket.BeginReceive( buffer, 0, buffer.Length, SocketFlags.None, OnRecv, buffer );
      //}
      var buffer = new byte [ Coder.MESSAGE_PREFIX_LEN ];

      _socket.BeginReceive( buffer, 0, buffer.Length, SocketFlags.None, OnRecv, buffer );
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ar"></param>
    protected void OnRecv( IAsyncResult ar )
    {
      lock( _syncObject )
      {
        if( _socket == null )
          return;

        var bClose = false;

        try
        {
          var buffer = CompleteRecv( ar );

          bClose = ( null == buffer );

          if( bClose )
            return;

          var m = Coder.Decode( buffer );

          Events.Add( EventType.Received, new Tuple<byte [], Message>( buffer, m ) );

          AnalyzeRecv( m );

          BeginRecv();
        }
        catch
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
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ar"></param>
    /// <returns></returns>
    protected virtual byte [] CompleteRecv( IAsyncResult ar )
    {
      lock( _syncObject )
      {
        int count = _socket.EndReceive( ar );

        if( count != Coder.MESSAGE_PREFIX_LEN )
          return null;

        var prefix = ar.AsyncState as byte [];
        Array.Reverse( prefix );
        var len = BitConverter.ToInt32( prefix, 0 );

        var buffer = new byte [ len ];

        int iBytesToReadLeft = len;
        int iOffset = 0;

        while( iBytesToReadLeft > 0 )
        {
          int iRecvCount = _socket.Receive( buffer, iOffset, iBytesToReadLeft, SocketFlags.None );

          if( 0 == iRecvCount )
            break;

          iBytesToReadLeft -= iRecvCount;
          iOffset += iRecvCount;
        }

        if( iBytesToReadLeft > 0 )
          throw new Exception( "invalid message length" );

        return buffer;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    protected virtual void AnalyzeRecv( Message m )
    {
      if( m is null )
        return;

      switch( m.Type )
      {
        case MessageType.DataMessage:
          HandleDataMessage( m as DataMessage );
          break;

        case MessageType.SelectReq:
          HandleSelectReq( m as SelectReq );
          break;

        case MessageType.SelectRsp:
          HandleSelectRsp( m as SelectRsp );
          break;

        case MessageType.DeselectReq:
          HandleDeselectReq( m as DeselectReq );
          break;

        case MessageType.DeselectRsp:
          HandleDeselectRsp( m as DeselectRsp );
          break;

        case MessageType.SeparateReq:
          //HandleSeparateReq();
          throw new Exception( "we must write nice exception here" );
          //	ConnException.Throw( ConnException.Code.SeparateReq );
          //break;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="m"></param>
    protected void HandleDataMessage( DataMessage m )
    {
      if( _state == State.ConnectedSelected )
      {
        if( _transactions.ContainsKey( m.Context ) )
        {
          _transactions.Remove( m.Context );
        }
      }
      else
      {
        Send( new RejectReq( m.Device, m.Context, 4 ) );
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="m"></param>
    protected void HandleSelectReq( SelectReq m )
    {
      if( _state == State.ConnectedNotSelected )
      {
        _timerT7ConnectionIdleTimeout.Change( Timeout.Infinite, Timeout.Infinite );

        Send( new SelectRsp( m.Device, m.Context, 0 ) );

        _state = State.ConnectedSelected;

        Events.Add( EventType.Connected, _config.Port );

      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="m"></param>
    protected void HandleSelectRsp( SelectRsp m )
    {
      if( _state == State.ConnectedNotSelected )
      {
        if( _transactions.ContainsKey( m.Context ) )
        {
          _transactions.Remove( m.Context );

          if( m.Status == 0 )
          {
            _timerT6ControlTimeout.Change( Timeout.Infinite, Timeout.Infinite );

            _state = State.ConnectedSelected;

            Events.Add( EventType.Connected, _config.Port );
          }
          else
          {
            CloseConnection();
          }
        }
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="m"></param>m.
    protected void HandleDeselectReq( DeselectReq m )
    {
      if( _state == State.ConnectedSelected )
      {
        Send( new DeselectRsp( m.Device, m.Context, 0 ) );

        _state = State.ConnectedNotSelected;
      }
      else
      {
        Send( new DeselectRsp( m.Device, m.Context, 1 ) );
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="deselectRsp"></param>
    protected void HandleDeselectRsp( DeselectRsp m )
    {
      if( _state == State.ConnectedSelected )
      {
        if( m.Status == 0 )
          _state = State.ConnectedNotSelected;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    protected void HandleSeparateReq()
    {
      CloseConnection();
    }

    #endregion

    #region Class 'Send' methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="m"></param>
    public void Send( Message m )
    {
      lock( _syncObject )
      {
        if( !_bRun )
          return;

        _queueToSend.Enqueue( m );
        Monitor.Pulse( _syncObject );
      }
    }
    /// <summary>
    /// 
    /// </summary>
    protected void MessageProcessor()
    {
      while( true )
      {
        lock( _syncObject )
        {
          if( _queueToSend.Count != 0 )
          {
            var m = _queueToSend.Dequeue();

            CompleteSend( m );
          }
          else
          {
            Monitor.Wait( _syncObject );
          }
        }
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="m"></param>
    protected void CompleteSend( Message m )
    {
      try
      {
        var arr = Coder.Encode( m );

        if( m.IsReplyRequired )
        {
          _transactions.Add( m.Context, new Transaction( m ) );
        }

        _socket.Send( arr );

        Events.Add( EventType.Sent, new Tuple<byte [], Message>( arr, m ) );
      }
      catch
      {
        CloseConnection();
      }
    }
    #endregion

    #region Class internal structs
    /// <summary>
    /// 
    /// </summary>
    public class Transaction
		{
			#region Class Properties
			/// <summary>
			/// 
			/// </summary>
			public Message Message { get; private set; }
			/// <summary>
			/// 
			/// </summary>
			public DateTime Timestamp { get; private set; }

			#endregion

			#region Class Initialization
			/// <summary>
			/// 
			/// </summary>
			/// <param name="m"></param>
			public Transaction( Message m )
			{
				Message = m;
				Timestamp = DateTime.Now;
			}
			#endregion
		}
		/// <summary>
		/// 
		/// </summary>
		public enum State
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
			ConnectedSelected
			#endregion
		}
		#endregion
	}
}
