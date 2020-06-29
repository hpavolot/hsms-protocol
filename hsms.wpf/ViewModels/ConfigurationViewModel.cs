using hsms.wpf.ViewModels.Base;
using Semi.Hsms;
using System;
using System.Windows.Input;
using static Semi.Hsms.Configurator;

namespace hsms.wpf.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    internal class ConfigurationViewModel: BaseViewModel
    {
        #region Class members
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Closing;
        /// <summary>
        /// 
        /// </summary>
        private string _ipAddress;
        /// <summary>
        /// 
        /// </summary>
        private int _port;
        /// <summary>
        /// 
        /// </summary>
        private ConnectionMode _mode;

        #endregion

        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public string IP
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                if (value == _ipAddress)
                    return;

                _ipAddress = value;
                OnPropertyChanged("IP");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                if (value == _port)
                    return;

                _port = value;
                OnPropertyChanged("Port");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ConnectionMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (value == _mode)
                    return;

                _mode = value;
                OnPropertyChanged("Mode");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Configurator Config
        {
            get
            {
                return new ConfigurationBuilder()
                 .IP(IP)
                 .Port(Port)
                 .Mode(Mode)
                 .T5(2)
                 .Build(); 
            }
        }

        #region Commands
        /// <summary>
        /// 
        /// </summary>
        public ICommand SaveCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommand CancelCommand { get; private set; }
        #endregion

        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        public ConfigurationViewModel()
        {
            Mode = ConnectionMode.Active;
            IP = "127.0.0.1";
            Port = 11000;

            SaveCommand = new RelayCommand(SaveConfigurationSettings);
            CancelCommand = new RelayCommand(Cancel);
        }
        #endregion

        #region Class methods
        /// <summary>
        /// 
        /// </summary>
        private void SaveConfigurationSettings()
        {

            this.Closing?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        private void Cancel()
        {
            this.Closing?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
