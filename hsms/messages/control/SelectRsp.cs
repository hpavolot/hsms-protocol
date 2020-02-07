#region Usings
#endregion

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
  public class SelectRsp : ControlMessage 
  {
		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public byte Status { get; protected set; }
		/// <summary>
		/// 
		/// </summary>
		public override MessageType Type => MessageType.SelectRsp;
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device"></param>
		/// <param name="context"></param>
		public SelectRsp( ushort device, uint context, byte status )
			: base( device, context )
		{
			Status = status;
		}
		#endregion

		#region Class public methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "select rsp";
		}
		#endregion
	}
}
