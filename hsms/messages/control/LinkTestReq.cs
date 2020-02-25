#region Usings
#endregion

namespace Semi.Hsms.Messages
{
	public class LinkTestReq : ControlMessage
	{
		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public override MessageType Type => MessageType.LinktestReq;
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device"></param>
		/// <param name="context"></param>
		public LinkTestReq( uint context ) 
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
			return "link test req";
		}
		#endregion
	}
}
