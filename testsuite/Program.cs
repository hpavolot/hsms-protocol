#region Usings
using System.Net;
using static Semi.Hsms.Messages.Configurator;
using System;
using Semi.Hsms.Connections;
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
			var config = new ConfigurationBuilder()
				.IP( "127.0.0.1" )
				.Port(11000)
				.Build();

			var connection = new Connection(config);

			while( true ) 
			{
				var cmd = Console.ReadLine();

				switch( cmd ) 
				{
					case "start":
						connection.Start();
						break;

					case "stop":
						connection.Stop();
						break;

					case "exit":
						return;
				}
			}
			
		}
		#endregion
	}
}