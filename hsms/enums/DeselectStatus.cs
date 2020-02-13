using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semi.Hsms.messages.control
{
	/// <summary>
	/// 
	/// </summary>
	public enum DeselectStatus : byte
	{
		#region Class properties
		Communication_Ended = 0,
		Communication_NotEstablished = 1,
		Communication_Busy = 2
		#endregion
	}
}
