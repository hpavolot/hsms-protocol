#region Usings
using hsms.wpf.ViewModels.Base;
using Semi.Hsms;
using Semi.Hsms.connections;
using System;
using System.Windows.Input;
using static Semi.Hsms.Configurator;
#endregion

namespace hsms.wpf.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    internal class ConnectionViewModel : BaseViewModel
    {
        #region Class members
        /// <summary>
        /// 
        /// </summary>
        private readonly Connection _connection;
        /// <summary>
        /// 
        /// </summary>
        private string _logLine;
        /// <summary>
        /// 
        /// </summary>
        private bool _canTryToConnect = true;

        #endregion

        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public string LogLine
        {
            get
            {
                return _logLine;
            }
            set
            {
                _logLine += "[" + DateTime.Now + "] " + value + "\r\n";
                OnPropertyChanged("LogLine");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CanTryToConnect
        {
            get
            {
                return _canTryToConnect;
            }
            set
            {
                _canTryToConnect = value;
                OnPropertyChanged("CanTryToConnect");
                OnPropertyChanged("CannotConnect");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CannotConnect => !CanTryToConnect;

        #region Commands
        /// <summary>
        /// 
        /// </summary>
        public ICommand ConnectCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommand DisconnectCommand { get; private set; }

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ConnectionViewModel()
        {
            var config = new ConfigurationBuilder()
                 .IP("127.0.0.1")
                 .Port(11000)
                 .Mode(ConnectionMode.Active)
                 .T5(2)
                 .Build();

            _connection = new Connection(config);

            ConnectCommand = new RelayCommand(Start);
            DisconnectCommand = new RelayCommand(Stop);

            _connection.Events.Connected += (s, ea) => LogLine = "CONNECTION ESTABLISHED";
            _connection.Events.Disconnected += (s, ea) => LogLine = "HSMS DISCONNECTED";
            _connection.Events.IsConnecting += (s, ea) => LogLine = "HSMS IS CONNECTING ACTIVELY";
            _connection.Events.IsListening += (s, ea) => LogLine = "HSMS IS LISTENING PASSIVELY";
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            _connection.Start();
            CanTryToConnect = false;

        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            _connection.Stop();
            CanTryToConnect = true;
        }
    }
}
