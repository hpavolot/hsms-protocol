#region Usings
using Semi.Hsms.connections;
using Semi.Hsms.Messages;
using System;
using System.Linq;
using System.Text;
using System.Windows.Input;
#endregion

namespace hsms.wpf
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
            _connection.Events.IsListening += (s, ea) => LogLine = $"HSMS IS LISTENING PASSIVELY: {ea}";
            _connection.Events.Sent += (s, ea) =>
            {
                StringBuilder sb = new StringBuilder("SEND : ");

                var hex = string.Join(" ", ea.Item1.Skip(4).Select(b => b.ToString("X2")));
                sb.Append(hex);

                var prefix = ea.Item1.Take(4).ToArray();
                Array.Reverse(prefix);
                var len = BitConverter.ToInt32(prefix, 0);
                sb.Append($"  len={len} ");

                var m = ea.Item2;
                if (m is DataMessage)
                {
                    var dm = m as DataMessage; 
                    sb.Append($" S{dm.Stream}F{dm.Function} ");
                }
                sb.Append($"  SB=[{m.Context}]");
                sb.Append($"  (({m}))");

                LogLine = sb.ToString();
            };

            _connection.Events.Received += (s, ea) =>
            {
                StringBuilder sb = new StringBuilder("RECV : ");
                var hex = string.Join(" ", ea.Item1.Select(b => b.ToString("X2")));
                sb.Append(hex);

                var len = ea.Item1.Length;
                sb.Append($"  len={len} ");

                var m = ea.Item2;
                if (m is DataMessage)
                {
                    var dm = m as DataMessage;
                    sb.Append($" S{dm.Stream}F{dm.Function} ");
                }
                sb.Append($"  SB=[{m.Context}]");
                sb.Append($"  (({m}))");

                LogLine = sb.ToString();
            };
            _connection.Events.T3Timeout += (s, ea) => LogLine = $"Message has not been delivered: {ea} {ea.Context}";
        }

        #endregion

        #region Class methods
        /// <summary>
        /// 54
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
                .Device(6)
                .Stream(8)
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
