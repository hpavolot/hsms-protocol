#region Usings
using hsms.wpf.ViewModels.Base;
using hsms.wpf.Views;
using Semi.Hsms.connections;
using Semi.Hsms.Messages;
using System;
using System.Windows.Input;
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
        private Connection _connection;
        /// <summary>
        /// 
        /// </summary>
        private string _logLine;
        /// <summary>
        /// 
        /// </summary>
        private bool _canTryToConnect = true;
        /// <summary>
        /// 
        /// </summary>
        private readonly ConfigurationViewModel _configurationViewModel;
        
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
                if (string.IsNullOrEmpty(value))
                    _logLine = default;
                else
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
        /// <summary>
        /// 
        /// </summary>
        public ICommand SendCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommand ClearLogCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommand ConfigureCommand { get; private set; }


        #endregion

        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        public ConnectionViewModel()
        {
            _configurationViewModel = new ConfigurationViewModel();

            InitializeCommands();
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeCommands()
        {
            ConfigureCommand = new RelayCommand(Configure);
            ConnectCommand = new RelayCommand(Start);
            DisconnectCommand = new RelayCommand(Stop);
            SendCommand = new RelayCommand(Send);
            ClearLogCommand = new RelayCommand(ClearLog);
        }
        /// <summary>
        /// 
        /// </summary>
        private void SubscribeToEvents()
        {
            _connection.Events.Connected += (s, ea) => LogLine = "CONNECTION ESTABLISHED";
            _connection.Events.Disconnected += (s, ea) => LogLine = "HSMS DISCONNECTED";
            _connection.Events.IsConnecting += (s, ea) => LogLine = $"HSMS IS CONNECTING ACTIVELY: {ea}";
            _connection.Events.IsListening += (s, ea) => LogLine = "HSMS IS LISTENING PASSIVELY";
            _connection.Events.Sent += (s, ea) => LogLine = $"SEND: {ea} {ea.Context}";
            _connection.Events.Received += (s, ea) => LogLine = $"RCV: {ea} {ea.Context}";
            _connection.Events.T3Timeout += (s, ea) => LogLine = $"Message has not been delivered: {ea} {ea.Context}";
        }

        #endregion

        #region Class methods
        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            _connection = new Connection(_configurationViewModel.Configurator);

            _connection.Start();
            CanTryToConnect = false;

            SubscribeToEvents();
        }
        /// <summary>
        /// 
        /// </summary>
        private void Stop()
        {
            _connection.Stop();
            CanTryToConnect = true;
        }
        /// <summary>
        /// 
        /// </summary>
        private void Send()
        {
            var m = DataMessage
                .Builder
                .NewContext()
                .Device(1)
                .Stream(1)
                .Function(101)
                .Build();

            _connection.Send(m);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearLog()
        {
            LogLine = default;
        }
        /// <summary>
        /// 
        /// </summary>
        private void Configure()
        {
            ConfigurationView view = new ConfigurationView();
            view.DataContext = _configurationViewModel;
            view.ShowDialog();
        }
        #endregion
    }
}
