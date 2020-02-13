#region Usings
#endregion

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public enum Reason : byte
	{
		#region Class properties
		STypeNotSupported = 1,
		PTypeNotSupported = 2,
		TransactionNotOpen = 3,
		EntityNotSelected = 4
		#endregion
	}
}
