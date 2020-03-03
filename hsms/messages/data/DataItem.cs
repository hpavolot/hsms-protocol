#region Usings
#endregion

namespace Semi.Hsms.Messages
{
  /// <summary>
  /// 
  /// </summary>
  public abstract class DataItem
  {
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		protected Format _format;
		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public Format Type
		{
			get
			{
				return _format;
			}
		}
		#endregion
	}
}
