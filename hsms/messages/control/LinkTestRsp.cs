#region Usings 
#endregion

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public class LinkTestRsp : ControlMessage
	{
		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public override MessageType Type => MessageType.LinktestRsp;
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device"></param>
		/// <param name="context"></param>
		public LinkTestRsp( uint context ) 
			: base( ushort.MaxValue, context )
		{
		}
		#endregion

		#region Class public methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "link test rsp";
		}
		#endregion
	}
}
