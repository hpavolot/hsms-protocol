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
			byte res = 0;
			byte day = 9;
			res = ( byte )( res | day );

			res = ( byte )( res << 1 );
			res |= 1;

			var female = res & 1;
			var dayRes = res >> 1;









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
						new A( "lena", 10 ),
						new U2( 43 ),
						new U4( 200 ))
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