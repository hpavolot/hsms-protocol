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
		private Logger _logger;

		#endregion

		public VmLogger(Logger logger)
		{
			_logger = logger;
			_logger.LogLineChanged +=(s,ea) => FirePropertyChanged(() => LogLine);
		}

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

	}
}