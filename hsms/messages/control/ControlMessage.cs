#region Usings
#endregion

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ControlMessage : Message
	{
		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public override bool IsPrimary
		{
			get
			{
				return (0 != (((uint)Type) & 1));
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public override bool IsReplyRequired
		{
			get
			{
				return IsPrimary;
			}
		}
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device"></param>
		/// <param name="context"></param>
		public ControlMessage(ushort device, uint context)
		  : base(device, context)
		{

		}
		#endregion
	}
}
