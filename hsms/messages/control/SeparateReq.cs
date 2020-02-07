#region Usings
#endregion

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public class SeparateReq : ControlMessage
	{
		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public override MessageType Type => MessageType.SeparateReq;
		/// <summary>
		/// 
		/// </summary>
		public override bool IsReplyRequired => false;
		#endregion

		#region Class Initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device"></param>
		/// <param name="context"></param>
		public SeparateReq( ushort device, uint context ) 
			: base( device, context )
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
			return "separate req";
		}
		#endregion
	}
}
