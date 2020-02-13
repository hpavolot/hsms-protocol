#region Usings
using Semi.Hsms.Messages;
using System;
#endregion

namespace Semi.Hsms.TestSuite
{
	class Program
	{
		#region Class public methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		static void Main( string [] args )
		{
			var sreq = Coder.Encode(new SelectReq( 1, 2 ));
			var srsp = Coder.Encode(new SelectRsp(1, 2,1));

			var dreq = Coder.Encode(new DeselectReq(3, 4));
			var drsp = Coder.Encode(new DeselectRsp(3, 4,1));


			var ltreq = Coder.Encode(new LinkTestReq(1, 2));
			var ltrsp = Coder.Encode(new LinkTestRsp(1, 2));

			var rejectReq = Coder.Encode(new RejectReq(5, 6,1));

			var separateReq = Coder.Encode(new SeparateReq(1, 2));


			//var sr2 = Coder.Decode( bytes );

			//Console.WriteLine( sr.Equals( sr2 ) );

		}
		#endregion
	}
}
