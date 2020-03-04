#region Usings
using Semi.Hsms.Messages;
using System.Collections.Generic;
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
			var arr = new List<DataItem>();
			arr.Add( new I2( 32 ) );
			arr.Add( new U2( 2 ) );
			arr.Add(new A("lena",10));
			arr.Add(new A("denis",3));

			var message = DataMessage
					.Builder
					.Stream( 1 )
					.Function( 5 )
					.Context( 522555 )
					.Items( arr )
					.Build();

			var copy = message.Items as List<DataItem>;
		}

		#endregion
	}
}
