#region Usings
using Semi.Hsms.config;
using System;
using System.Windows.Input;
#endregion

namespace hsms.wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class VmConfiguration : BaseViewModel
    {
        #region Class members
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Closing;
        /// <summary>
        /// 
        /// </summary>
        private Configuration _configuration;
        /// <summary>
        /// 
        /// </summary>
        private Configuration _originalConfiguration;


        #endregion

        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public string IP
        {
            get
            {
                return _configuration.IP;
            }
            set
            {
                if (value == _configuration.IP)
                    return;

                _configuration.IP = value;
                FirePropertyChanged(() => IP);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get
            {
                return _configuration.Port;
            }
            set
            {
                if (value == _configuration.Port)
                    return;

                _configuration.Port = value;
                FirePropertyChanged(() => Port);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ConnectionMode Mode
        {
            get
            {
                return _configuration.Mode;
            }
            set
            {
                if (value == _configuration.Mode)
                    return;

                _configuration.Mode = value;
                FirePropertyChanged(() => Mode);
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
        public VmConfiguration(Configuration configuration)
        {
            _configuration = configuration;

            _originalConfiguration = new Configuration
            {
                IP = configuration.IP,
                Port = configuration.Port,
                Mode = configuration.Mode
            };

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
            _originalConfiguration.IP = _configuration.IP;
            _originalConfiguration.Port = _configuration.Port;
            _originalConfiguration.Mode = _configuration.Mode;

            this.Closing?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        private void Cancel()
        {
            IP = _originalConfiguration.IP;
            Port = _originalConfiguration.Port;
            Mode = _originalConfiguration.Mode;

            this.Closing?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
