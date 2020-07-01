using Semi.Hsms.Messages;
using System;

namespace Semi.Hsms.connections
{
	/// <summary>
	/// 
	/// </summary>
	public class Transaction
    {
		#region Class Properties
		/// <summary>
		/// 
		/// </summary>
		public Message Message { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime Timestamp { get; private set; }

		#endregion

		#region Class Initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="m"></param>
		public Transaction(Message m)
		{
			Message = m;
			Timestamp = DateTime.Now;
		}

		#endregion
	}
}
