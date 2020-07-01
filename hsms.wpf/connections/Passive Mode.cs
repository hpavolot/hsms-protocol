using Semi.Hsms.connections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Semi.Hsms.connections
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Connection
    {
        #region Class 'Passive Mode' methods
        /// <summary>
        /// 
        /// </summary>
        private void TryListen()
        {

            lock (_syncObject)
            {
                Events.Add(EventType.IsListening);

                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var ea = new SocketAsyncEventArgs()
                {
                    RemoteEndPoint = new IPEndPoint(_config.IP, _config.Port)
                };

                ea.Completed += OnConnectionAccepted;

                _socket.Bind(ea.RemoteEndPoint);

                _socket.Listen(10);

                _socket.AcceptAsync(ea);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectionAccepted(object sender, SocketAsyncEventArgs e)
        {
            lock (_syncObject)
            {
                if (!_bRun)
                    return;

                _socket.Close();

                _socket = e.AcceptSocket;

                _state = State.ConnectedNotSelected;

                _timerT7ConnectionIdleTimeout.Change(_config.T7 * 1000, Timeout.Infinite);

                _timerT5ConnectSeparationTimeout.Change(Timeout.Infinite, Timeout.Infinite);

                new Thread(MessageProcessor).Start();

                BeginRecv();
            }
        }
        #endregion
    }
}
