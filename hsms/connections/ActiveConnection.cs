#region Usings
using Semi.Hsms.connections;
using Semi.Hsms.Messages;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
#endregion

namespace Semi.Hsms.Connections
{
    /// <summary>
    /// 
    /// </summary>
    public class ActiveConnection : Connection
    {
        #region Class members
        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurator"></param>
        public ActiveConnection(Configurator configurator) : base(configurator)
        {
            _timerT5ConnectSeparationTimeout = new Timer(s => TryConnect(),
                null, Timeout.Infinite, Timeout.Infinite);
        }
        #endregion

        #region Class methods
        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            lock (_mLock)
            {
                if (_bRun)
                    return;

                _bRun = true;

                TryConnect();
            }
        }
        #endregion

        #region Class 'Connection' methods
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

    }
}
