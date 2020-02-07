#region Usings
#endregion

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public class DeselctRsp : ControlMessage
	{
		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public override MessageType Type => MessageType.DeselectRsp;
		/// <summary>
		/// 
		/// </summary>
		public byte Status { get; protected set; }
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device"></param>
		/// <param name="context"></param>
		/// <param name="status"></param>
		public DeselctRsp( ushort device, uint context, byte status ) : base( device, context )
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
			return "deselt rsp";
		}
		#endregion
	}

}