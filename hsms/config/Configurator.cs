#region Usings

using Semi.Hsms.config;
using System.Net;

#endregion

namespace Semi.Hsms.config
{
	/// <summary>
	/// 
	/// </summary>
	public partial class Configurator
	{
		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public IPAddress IP { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Port { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public ConnectionMode Mode { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public ushort T3 { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public ushort T5 { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public ushort T6 { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public ushort T7 { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public ushort T8 { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public static ConfigurationBuilder Builder => new ConfigurationBuilder();

		#endregion

		#region Class initialization

		/// <summary>
		/// 
		/// </summary>
		private Configurator()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{IP}:{Port}";
		}
		#endregion
	}
}