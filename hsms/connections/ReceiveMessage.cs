using Semi.Hsms.Messages;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Semi.Hsms.connections
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Connection
    {
        #region Class 'Receive' methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        protected void BeginRecv()
        {
            lock (_syncObject)
            {
                if (!_bRun)
                    return;

                var buffer = new byte[Coder.MESSAGE_PREFIX_LEN];

                _socket.BeginReceive(buffer, 0, buffer.Length,SocketFlags.None, OnRecv, buffer);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        protected void OnRecv(IAsyncResult ar)
        {
            lock (_syncObject)
            {
                if (_socket == null)
                    return;

                var bClose = false;

                try
                {
                    var buffer = CompleteRecv(ar);

                    bClose = (null == buffer);

                    if (bClose)
                        return;

                    var m = Coder.Decode(buffer);

                    Events.Add(EventType.Received, new Tuple<byte[], Message>(buffer, m));

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        protected virtual byte[] CompleteRecv(IAsyncResult ar)
        {
            lock (_syncObject)
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
                case MessageType.DataMessage:
                    HandleDataMessage(m as DataMessage);
                    break;

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

                case MessageType.SeparateReq:
                    HandleSeparateReq();
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected void HandleDataMessage(DataMessage m)
        {
            if (_state == State.ConnectedSelected)
            {
                if (_transactions.ContainsKey(m.Context))
                {
                    _transactions.Remove(m.Context);
                }
            }
            else
            {
                Send(new RejectReq(m.Device, m.Context, 4));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected void HandleSelectReq(SelectReq m)
        {
            if (_state == State.ConnectedNotSelected)
            {
                _timerT7ConnectionIdleTimeout.Change(Timeout.Infinite, Timeout.Infinite);

                Send(new SelectRsp(m.Device, m.Context, 0));

                _state = State.ConnectedSelected;

                Events.Add(EventType.Connected, _config.Port);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected void HandleSelectRsp(SelectRsp m)
        {
            if (_state == State.ConnectedNotSelected)
            {
                if (_transactions.ContainsKey(m.Context))
                {
                    _transactions.Remove(m.Context);

                    if (m.Status == 0)
                    {
                        _timerT6ControlTimeout.Change(Timeout.Infinite, Timeout.Infinite);

                        _state = State.ConnectedSelected;

                        Events.Add(EventType.Connected,_config.Port);
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
        protected void HandleDeselectRsp(DeselectRsp m)
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
        protected void HandleSeparateReq()
        {
            CloseConnection();
        }
        #endregion
    }
}
