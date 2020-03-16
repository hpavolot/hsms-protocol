#region Usings
using System.Net;
using static Semi.Hsms.Messages.Configurator;
using Semi.Hsms.connections;
using Semi.Hsms.messages.control;
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
		static void Main(string[] args)
		{
			var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

			var config = new ConfigurationBuilder()
				.IP(ip)
				.Port(11000)
				.Build();

			var connection = new Connection(config);

			connection.Start();

			var selectReq = new SelectReq(1, 2);
			connection.Send(selectReq);
			
			connection.Stop();

		}
		#endregion
	}
}