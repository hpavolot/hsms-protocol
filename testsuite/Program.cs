#region Usings
using Semi.Hsms.Messages;
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
			var dm = new DataMessage( 1, 2 )
			{
				Function = 8
			};
			//var driver = new HsmsDriver();

			//driver.Send();
		}
		#endregion
	}
}
