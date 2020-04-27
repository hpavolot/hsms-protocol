using Semi.Hsms.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using static Semi.Hsms.Messages.Configurator;

namespace Semi.Hsms.connections
{
    public class Connection
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
        protected Timer _timerT3ReplyTimeout;
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
        protected Object _mLock = new Object();

        protected Dictionary<uint, Timer> _transactions = new Dictionary<uint, Timer>();
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
        public Connection(Configurator configurator)
        {
            _config = configurator;

            _timerT6ControlTimeout = new Timer(s => CloseConnection(),
                null, Timeout.Infinite, Timeout.Infinite);

            _timerT7ConnectionIdleTimeout = new Timer(s => CloseConnection(),
                null, Timeout.Infinite, Timeout.Infinite);

            if (_config.Mode == ConnectionMode.Active)
            {
                _timerT5ConnectSeparationTimeout = new Timer(s => TryConnect(),
                    null, Timeout.Infinite, Timeout.Infinite);
            }
            else
            {
                _timerT5ConnectSeparationTimeout = new Timer(s => TryListen(),
                    null, Timeout.Infinite, Timeout.Infinite);
            }
        }

        private void RemoveTransaction(uint context)
        {
            lock (_mLock)
            {
                _transactions[context].Change(Timeout.Infinite, Timeout.Infinite);
                _transactions.Remove(context);
                Console.WriteLine($"Timeout: T3 Timeout for context {context}");
            }
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

                _timerT5ConnectSeparationTimeout.Change(0, Timeout.Infinite);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            lock (_mLock)
            {
                if (!_bRun)
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
            lock (_mLock)
            {
                _socket.Close();
                _socket = null;

                _state = State.NotConnected;

                var t5FireTime = (_bRun) ? _config.T5 * 1000 : Timeout.Infinite;
                _timerT5ConnectSeparationTimeout.Change(t5FireTime, Timeout.Infinite);

                _timerT6ControlTimeout.Change(Timeout.Infinite, Timeout.Infinite);
                _timerT7ConnectionIdleTimeout.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }
        #endregion

        #region Class 'Active Mode' methods
        /// <summary>
        /// 
        /// </summary>
        private void TryConnect()
        {
            lock (_mLock)
            {
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectionCompleted(object sender, SocketAsyncEventArgs e)
        {
            lock (_mLock)
            {
                if (!_bRun)
                    return;

                if (null != e.ConnectSocket)
                {
                    _socket = e.ConnectSocket;

                    _state = State.ConnectedNotSelected;

                    Console.WriteLine("connected !!!");

                    BeginRecv();

                    Send(new SelectReq(1, 9));

                    _timerT6ControlTimeout.Change(_config.T6 * 1000, Timeout.Infinite);

                    _timerT5ConnectSeparationTimeout.Change(Timeout.Infinite, Timeout.Infinite);
                }
                else
                {
                    _timerT5ConnectSeparationTimeout.Change(_config.T5 * 1000, Timeout.Infinite);
                }
            }
        }
        #endregion

        #region Class 'Passive Mode' methods
        /// <summary>
        /// 
        /// </summary>
        private void TryListen()
        {
            Console.WriteLine("Waiting for a connection...");

            lock (_mLock)
            {
                var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var ea = new SocketAsyncEventArgs()
                {
                    RemoteEndPoint = new IPEndPoint(_config.IP, _config.Port)
                };

                ea.Completed += OnConnectionAccepted;

                s.Bind(ea.RemoteEndPoint);

                s.Listen(10);
                s.AcceptAsync(ea);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectionAccepted(object sender, SocketAsyncEventArgs e)
        {
            lock (_mLock)
            {
                if (!_bRun)
                    return;

                if (null != e.AcceptSocket)
                {
                    _socket = e.AcceptSocket;

                    _state = State.ConnectedNotSelected;

                    _timerT7ConnectionIdleTimeout.Change(_config.T7 * 1000, Timeout.Infinite);

                    _timerT5ConnectSeparationTimeout.Change(Timeout.Infinite, Timeout.Infinite);

                    Console.WriteLine("connected !!!");

                    BeginRecv();
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
                if (!_bRun)
                    return;

                Console.WriteLine($"sending: {m.ToString()}");

                var arr = Coder.Encode(m);

                _socket.Send(arr);

                if (m.IsReplyRequired)
                { 
                    var timer = new Timer(s => RemoveTransaction(m.Context),
                    null, _config.T3 *100, Timeout.Infinite);

                    _transactions.Add(m.Context, timer);
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
        protected void OnRecv(IAsyncResult ar)
        {
            lock (_mLock)
            {
                var bClose = false;

                try
                {
                    var buffer = CompleteRecv(ar);

                    bClose = (null == buffer);

                    if (bClose)
                        return;

                    var m = Coder.Decode(buffer);

                    AnalyzeRecv(m);

                    BeginRecv();
                }
                catch
                {
                    bClose = true;
                }
                finally
                {
                    if (bClose)
                    {
                        CloseConnection();
                    }

                }
            }
        }
        // <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        protected virtual byte[] CompleteRecv(IAsyncResult ar)
        {
            lock (_mLock)
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
                case MessageType.SelectReq:
                    HandleSelectReq(m as SelectReq);
                    break;
                case MessageType.SelectRsp:
                    HandleSelectRsp(m as SelectRsp);
                    break;
                case MessageType.DeselectReq:
                    HandleDeselectReq(m as DeselectReq);
                    break;
                case MessageType.DeselectRsp:
                    HandleDeselectRsp(m as DeselectRsp);
                    break;
                case MessageType.DataMessage:
                    HandleDataMessage(m as DataMessage);
                    break;

                case MessageType.SeparateReq:
                    HandleSeparateReq(m as SeparateReq);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        private void HandleDataMessage(DataMessage m)
        {
            if (_state == State.ConnectedSelected)
            {
                //todo
            }
            else
            {
                Send(new RejectReq(m.Device, m.Context, 4));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectReq"></param>
        protected void HandleSelectReq(SelectReq m)
        {
            if (_state == State.ConnectedNotSelected)
            {
                _timerT7ConnectionIdleTimeout.Change(Timeout.Infinite, Timeout.Infinite);

                Send(new SelectRsp(m.Device, m.Context, 0));

                _state = State.ConnectedSelected;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected void HandleSelectRsp(SelectRsp m)
        {
            Console.WriteLine("Received selet rsp");
            if (_state == State.ConnectedNotSelected)
            {
                if (m.Status == 0)
                {
                    _transactions[m.Context].Change(Timeout.Infinite, Timeout.Infinite);
                    _transactions.Remove(m.Context);

                    _timerT6ControlTimeout.Change(Timeout.Infinite, Timeout.Infinite);

                    _state = State.ConnectedSelected;

                    Connected?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    CloseConnection();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected void HandleDeselectReq(DeselectReq m)
        {
            if (_state == State.ConnectedSelected)
            {
                Send(new DeselectRsp(m.Device, m.Context, 0));

                _state = State.ConnectedNotSelected;
            }
            else
            {
                Send(new DeselectRsp(m.Device, m.Context, 1));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deselectRsp"></param>
        private void HandleDeselectRsp(DeselectRsp m)
        {
            if (_state == State.ConnectedSelected)
            {
                if (m.Status == 0)
                    _state = State.ConnectedNotSelected;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="separateReq"></param>
        protected void HandleSeparateReq(SeparateReq separateReq)
        {
            if (_state == State.ConnectedSelected)
                CloseConnection();
        }
        #endregion

        #region Class internal structs
        /// <summary>
        /// 
        /// </summary>
        protected enum State
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
