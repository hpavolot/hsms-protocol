
using System;

namespace hsms.wpf
{
    public class LoggerViewModel: BaseViewModel
    {
        #region Class members
        /// <summary>
        /// 
        /// </summary>
        private string _logLine;
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
                _logLine = value;
                OnPropertyChanged("LogLine");
            }
        }
        #endregion
    }
}
