#region Usings
using System.Net;
using static Semi.Hsms.Messages.Configurator;
using System;
using Semi.Hsms.Connections;
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
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.IP( "127.0.0.1" )
				.Port(11000)
				.Build();

			var connection = new Connection(config);

			connection.Connected += ( s, ea ) => 
			{
				var m = DataMessage
					.Builder
					.Device( 1 )
					.Context( 12 )
					.Stream( 5 )
					.Function( 3 )
					.Items(
					 new A ("test",4),
					 new I1(8),
					 new ListItem(
						 new I2(7)))
					.Build();

				connection.Send( m );
			};

			connection.Start();

			//connection.Send( null );

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