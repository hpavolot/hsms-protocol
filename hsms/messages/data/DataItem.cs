#region Usings
#endregion

namespace Semi.Hsms.Messages
{
  /// <summary>
  /// 
  /// </summary>
  public class DataItem
  {
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		protected string _name;
		/// <summary>
		/// 
		/// </summary>
		protected Format _format;
		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}
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
