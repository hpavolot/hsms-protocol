#region Usings
using Semi.Hsms.Messages;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using static Semi.Hsms.Messages.Configurator;
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
			var config = new ConfigurationBuilder()
							.IP( "127.0.0.1" )
							.Port( 11000 )
							.Mode( ConnectionMode.Active )
							.T5( 2 )
							.Build();

			var connection = new Connection( config );

			//connection.T3Timeout += ( s, ea ) => Console.WriteLine( "Message was not delivered" );

			connection._eventDispatcher.Connected += ( s, ea ) => Console.WriteLine( "Connection established" );
			connection._eventDispatcher.Disconnected += (s, ea) => Console.WriteLine("Connection closed");
			connection._eventDispatcher.Sent += (s, ea) => Console.WriteLine($"Message sent: {ea.ToString()} {ea.Context.ToString()}");
			connection._eventDispatcher.Received += (s, ea) => Console.WriteLine($"Message received: {ea.ToString()} {ea.Context.ToString()}");
			connection._eventDispatcher.T3Timeout += (s, ea) => Console.WriteLine($"Message has not been delivered: {ea.ToString()} {ea.Context.ToString()}");


			byte[] uintBuffer = new byte [ sizeof( uint ) ];

			var m = DataMessage
					.Builder
					.NewContext()
					.Device( 1 )
					.Stream( 1 )
					.Function( 101 )
					.Build();


			while( true )
			{
				var cmd = Console.ReadLine();

				switch( cmd )
				{
					case "start":
						connection.Start();
						break;

					case "send":
						connection.Send(m);
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