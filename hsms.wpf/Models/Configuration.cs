# region Usings
using hsms.wpf.ViewModels.Base;
using Semi.Hsms.config;
#endregion

namespace hsms.wpf.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Configuration : BaseViewModel
    {
        #region Class members
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

        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        public Configuration()
        {
            Mode = ConnectionMode.Passive;
            IP = "127.0.0.1";
            Port = 11005;
        }

        #endregion
    }
}
