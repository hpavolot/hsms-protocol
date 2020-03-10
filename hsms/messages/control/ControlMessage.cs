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

		#region Class public methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
				return false;

			var cm = obj as ControlMessage;

			if (null == cm)
				return false;

			if (IsPrimary != cm.IsPrimary)
				return false;

			if (IsReplyRequired != cm.IsReplyRequired)
				return false;

			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			int hash = base.GetHashCode();

			hash = hash * 23 + IsPrimary.GetHashCode();
			hash = hash * 23 + IsReplyRequired.GetHashCode();

			return hash;
		}
		#endregion
	}
}
