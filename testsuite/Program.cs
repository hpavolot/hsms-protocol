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
			var sreq = new SelectReq( 1, 2 );
			var srsp = new SelectRsp( 1, 2, 3 );

			var bytes = Coder.Encode( sreq );
			var bytes2 = Coder.Encode( srsp );

			//var sr2 = Coder.Decode( bytes );

			//Console.WriteLine( sr.Equals( sr2 ) );

		}
		#endregion
	}
}
