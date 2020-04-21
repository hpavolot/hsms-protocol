using Semi.Hsms.Messages;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace Semi.Hsms.connections
{
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
        }
        #endregion

        #region Class methods
        /// <summary>
        /// 
        /// </summary>
        public abstract void Start();
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
            lock (_mLock)
            {
                if (_state == State.ConnectedNotSelected)
                {
                    _timerT7ConnectionIdleTimeout.Change(Timeout.Infinite, Timeout.Infinite);

                    Send(new SelectRsp(m.Device, m.Context, 0));

                    _state = State.ConnectedSelected;

                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected void HandleSelectRsp(SelectRsp m)
        {
            lock (_mLock)
            {
                if (_state == State.ConnectedNotSelected)
                {
                    if (m.Status == 0)
                    {
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
            lock (_mLock)
            {
                if (_state == State.ConnectedSelected)
                    CloseConnection();
            }
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
