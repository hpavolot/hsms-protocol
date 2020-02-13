#region Usings
#endregion

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public enum SelectStatus : byte
	{
		#region Class properties
		Communication_Established = 0,
		CommunicationA_lreadyActive = 1,
		Connection_NotReady = 2,
		Connect_Exhaust = 3
		#endregion
	}
}
