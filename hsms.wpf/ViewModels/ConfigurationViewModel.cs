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
    internal class ConfigurationViewModel
    {
        #region Class members
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Closing;
        /// <summary>
        /// 
        /// </summary>
        private readonly Configuration _config;
        /// <summary>
        /// 
        /// </summary>
        private readonly Configuration _originalConfig;

        #endregion

        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public Configuration Configuration { get => _config; private set { } }
        /// <summary>
        /// 
        /// </summary>
        public Configurator Configurator
        {
            get
            {
                return Configurator
                 .Builder
                 .IP(Configuration.IP)
                 .Port(Configuration.Port)
                 .Mode(Configuration.Mode)
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
            _config = new Configuration();
            _originalConfig = new Configuration();

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
            _originalConfig.IP = _config.IP;
            _originalConfig.Port = _config.Port;
            _originalConfig.Mode = _config.Mode;

            this.Closing?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        private void Cancel()
        {
            _config.IP = _originalConfig.IP;
            _config.Port = _originalConfig.Port;
            _config.Mode = _originalConfig.Mode;

            this.Closing?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
