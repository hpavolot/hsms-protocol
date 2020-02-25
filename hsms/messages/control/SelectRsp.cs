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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals( object obj )
		{
			if( !base.Equals( obj ) )
				return false;

			var m = obj as SelectRsp;

			if( null == m )
				return false;

			if( Status != m.Status )
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

			hash = hash * 23 + Status.GetHashCode();

			return hash;
		}
		#endregion
	}
}
