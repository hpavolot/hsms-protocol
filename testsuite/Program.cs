﻿#region Usings
using Semi.Hsms.connections;
using Semi.Hsms.messages.data;
using Semi.Hsms.Messages;
using System;
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
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.IP("127.0.0.1")
				.Port(11000)
				.Mode(ConnectionMode.Active)
				.T5(2)
				.Build();

			var connection = new Connection( config);

			connection.Connected += ( s, ea ) =>
			{
				var m = DataMessage
					.Builder
					.Device(1)
					.Context(12)
					.Stream(5)
					.Function(3)
					.Items(new ListItem(
						new Bool(true),
						new A("lena", 5),
						new I2(5)),
						new ListItem(
							new F4(12.5f)))
					.Build();

				connection.Send( m );
			};

			connection.Start();


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