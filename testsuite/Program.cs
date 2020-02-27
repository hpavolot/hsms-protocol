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
			var c = Configurator
				.Builder
				.T3( 100 )
				.T5( 200 )
				.Build();

			var c1 = Configurator
				.Builder
				.Copy( c )
				.T5( 300 )
				.Build();
		}
		#endregion
	}
}
