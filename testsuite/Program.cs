#region Usings
using Semi.Hsms.Coding;
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
			var sr = new SelectReq( 1, 2 );
			var bytes = Encoder.Encode( sr );

			var sr2 = Decoder.Decode( bytes );

			Console.WriteLine( sr.Equals( sr2 ) );
		
		}
		#endregion
	}
}
