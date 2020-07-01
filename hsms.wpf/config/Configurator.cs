#region Usings

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Xml;

#endregion

namespace Semi.Hsms
{
	/// <summary>
	/// 
	/// </summary>
	public class Configurator
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

		#region Class Configuration Builder

		/// <summary>
		/// 
		/// </summary>
		public class ConfigurationBuilder
		{
			#region Class constants

			/// <summary>
			/// 
			/// </summary>
			public const ushort DEFAULT_T3 = 45;

			/// <summary>
			/// 
			/// </summary>
			public const ushort DEFAULT_T5 = 10;

			/// <summary>
			/// 
			/// </summary>
			public const ushort DEFAULT_T6 = 5;

			/// <summary>
			/// 
			/// </summary>
			public const ushort DEFAULT_T7 = 10;

			/// <summary>
			/// 
			/// </summary>
			public const ushort DEFAULT_T8 = 5;

			#endregion

			#region Class members
			/// <summary>
			/// 
			/// </summary>
			private IPAddress _ipAddress;
			/// <summary>
			/// 
			/// </summary>
			private int _port;
			/// <summary>
			/// 
			/// </summary>
			private ConnectionMode _mode;
			/// <summary>
			/// 
			/// </summary>
			private ushort _t3 = DEFAULT_T3;
			/// <summary>
			/// 
			/// </summary>
			private ushort _t5 = DEFAULT_T5;
			/// <summary>
			/// 
			/// </summary>
			private ushort _t6 = DEFAULT_T6;
			/// <summary>
			/// 
			/// </summary>
			private ushort _t7 = DEFAULT_T7;
			/// <summary>
			/// 
			/// </summary>
			private ushort _t8 = DEFAULT_T8;

			#endregion

			#region Class public methods

			/// <summary>
			/// 
			/// </summary>
			/// <param name="t3"></param>
			/// <returns></returns>
			public ConfigurationBuilder T3( ushort t3 )
			{
				_t3 = ( t3 < 0 || t3 >= 120 ) ? DEFAULT_T3 : t3;

				return this;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="ipAddress"></param>
			/// <returns></returns>
			public ConfigurationBuilder IP( IPAddress ipAddress )
			{
				_ipAddress = ipAddress;

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="ipAddress"></param>
			/// <returns></returns>
			public ConfigurationBuilder IP( string ip )
			{
				if( IPAddress.TryParse( ip, out IPAddress res ))
				{
					_ipAddress = res;
				}

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="port"></param>
			/// <returns></returns>
			public ConfigurationBuilder Port( int port )
			{
				_port = port;
				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="port"></param>
			/// <returns></returns>
			public ConfigurationBuilder Mode(ConnectionMode mode)
			{
				_mode = mode;
				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="t5"></param>
			/// <returns></returns>
			public ConfigurationBuilder T5( ushort t5 )
			{
				_t5 = ( t5 < 0 || t5 >= 240 ) ? DEFAULT_T5 : t5;

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="t6"></param>
			/// <returns></returns>
			public ConfigurationBuilder T6( ushort t6 )
			{
				_t6 = ( t6 < 0 || t6 >= 240 ) ? DEFAULT_T6 : t6;

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="t7"></param>
			/// <returns></returns>
			public ConfigurationBuilder T7( ushort t7 )
			{
				_t7 = ( t7 < 0 || t7 >= 240 ) ? DEFAULT_T7 : t7;

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="t8"></param>
			/// <returns></returns>
			public ConfigurationBuilder T8( ushort t8 )
			{
				_t8 = ( t8 < 0 || t8 >= 120 ) ? DEFAULT_T8 : t8;

				return this;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="c"></param>
			/// <returns></returns>
			public ConfigurationBuilder Copy( Configurator c )
			{
				_ipAddress = c.IP;
				_port = c.Port;
				_mode = c.Mode;
				_t3 = c.T3;
				_t5 = c.T5;
				_t6 = c.T6;
				_t7 = c.T7;
				_t8 = c.T8;

				return this;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public Configurator Build()
			{
				var c = new Configurator
				{
					IP = _ipAddress,
					Port = _port,
					Mode = _mode,
					T3 = _t3,
					T5 = _t5,
					T6 = _t6,
					T7 = _t7,
					T8 = _t8
				};

				return c;
			}

			#endregion
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public enum ConnectionMode
	{
		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		Active,
		/// <summary>
		/// 
		/// </summary>
		Passive,
		#endregion
	}
}