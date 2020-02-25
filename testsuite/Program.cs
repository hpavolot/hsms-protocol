#region Usings
using Semi.Hsms.Messages;
using System;
using System.Collections.Generic;
using System.IO;
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
			//var dict = new Dictionary<Message, string>();

			var a = new SelectRsp( 1,2,3 );

			var arr = Coder.Encode( a );
			//BinaryWriter bw;

			

			//var res = a.Equals( b );



			//var sreq = Coder.Encode(new SelectReq(1, 2));
			//var sreq_decoded = Coder.Decode(sreq);


			//var srsp = Coder.Encode(new SelectRsp(1, 2, 1));
			//var srsp_decoded = Coder.Decode(srsp);



			//var dreq = Coder.Encode(new DeselectReq(3, 4));
			//var dreq_decoded = Coder.Decode(dreq);

			//var drsp = Coder.Encode(new DeselectRsp(3, 4, 2));
			//var drsp_decoded = Coder.Decode(drsp);


			//var ltreq = Coder.Encode(new LinkTestReq(1, 2));
			//var ltreq_decoded = Coder.Decode(ltreq);

			//var ltrsp = Coder.Encode(new LinkTestRsp(1, 2));
			//var ltrsp_decoded = Coder.Decode(ltrsp);

			//var rejectReq = Coder.Encode(new RejectReq(5, 6,1));
			//var rejectReq_decoded = Coder.Decode(rejectReq);

			//var separateReq = Coder.Encode(new SeparateReq(1, 2));
			//var separateReq_decoded = Coder.Decode(separateReq);












		}
		#endregion
	}
}
