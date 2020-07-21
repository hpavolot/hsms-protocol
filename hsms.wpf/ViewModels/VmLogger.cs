#region Usings
#endregion

namespace hsms.wpf.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VmLogger : BaseViewModel
    {
        #region Class members
        /// <summary>
        /// 
        /// </summary>
        private readonly Logger _logger;

        #endregion

        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public string LogLine
        {
            get
            {
                return _logger.LogLine;
            }
        }

        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public VmLogger(Logger logger)
        {
            _logger = logger;
            _logger.LogLineChanged += (s, ea) => FirePropertyChanged(() => LogLine);
        }
        #endregion
    }
}