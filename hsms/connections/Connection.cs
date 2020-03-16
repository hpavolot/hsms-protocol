#region Usings
using Semi.Hsms.Messages;
using System;
using System.Net;
using System.Net.Sockets;
#endregion

namespace Semi.Hsms.connections
{
	public class Connection
	{


		#region Class fields
		/// <summary>
		/// 
		/// </summary>
		private Socket socket;
		/// <summary>
		/// 
		/// </summary>
		private IPEndPoint endPoint;

		#endregion

		#region Class Initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurator"></param>
		public Connection(Configurator configurator)
		{
			endPoint = new IPEndPoint(configurator.IpAddress, configurator.Port);

			socket = new Socket(configurator.IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		}
		#endregion

		#region Class methods

		public  void Start()
		{
			try
			{
				socket.Connect(endPoint);

				Console.WriteLine("Socket connected to {0}",
				   socket.RemoteEndPoint.ToString());
			}
			catch (Exception e)
			{

				Console.WriteLine(e);
			}
		}
		#endregion
	}
}
