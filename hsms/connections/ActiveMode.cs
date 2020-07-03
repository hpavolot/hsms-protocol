using Semi.Hsms.Messages;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Semi.Hsms.connections
{
    /// <summary>
    /// 
    /// </summary>
    public partial class  Connection 
    {
        #region Class 'Active Mode' methods
        /// <summary>
        /// 
        /// </summary>
        public void TryConnect()
        {
            lock (_syncObject)
            {
                Events.Add(EventType.Connecting, _config.Port);

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
            lock (_syncObject)
            {
                if (!_bRun)
                    return;

                if (null != e.ConnectSocket)
                {
                    _socket = e.ConnectSocket;

                    _state = State.ConnectedNotSelected;

                    BeginRecv();

                    SendSelectReq();

                    new Thread( MessageProcessor ) 
                    {
                      IsBackground = true
                    }.Start();

                    _timerT5ConnectSeparationTimeout.Change(Timeout.Infinite, Timeout.Infinite);
                }
                else
                {
                    _timerT5ConnectSeparationTimeout.Change(_config.T5 * 1000, Timeout.Infinite);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void SendSelectReq()
        {
            byte[] uintBuffer = new byte[sizeof(uint)];


            Send(new SelectReq(1, Message.NextContext));

            //_timerT6ControlTimeout.Change(_config.T6 * 1000, Timeout.Infinite);
        }
        #endregion
    }
}
