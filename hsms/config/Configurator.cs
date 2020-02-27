#region Usings
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
#endregion

namespace Semi.Hsms.Messages
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
		public ushort T3 { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public ushort T5 { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public static ConfBuilder Builder => new ConfBuilder();
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		private Configurator( ) 
		{
		}
		#endregion

		#region Class internal structs
		/// <summary>
		/// 
		/// </summary>
		public class ConfBuilder 
		{
			#region Class constants
			/// <summary>
			/// 
			/// </summary>
			public const ushort DEFAULT_T3 = 20;
			#endregion

			#region Class members
			/// <summary>
			/// 
			/// </summary>
			private ushort _t3 = DEFAULT_T3;
			/// <summary>
			/// 
			/// </summary>
			private ushort _t5;
			#endregion

			#region Class public methods
			/// <summary>
			/// 
			/// </summary>
			/// <param name="t5"></param>
			/// <returns></returns>
			public ConfBuilder T5( ushort t5 ) 
			{
				_t5 = t5;

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="t5"></param>
			/// <returns></returns>
			public ConfBuilder T3( ushort t3 )
			{
				_t3 = ( t3 < 0 || t3 > 200 ) ? DEFAULT_T3 : t3;

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="c"></param>
			/// <returns></returns>
			public ConfBuilder Copy( Configurator c ) 
			{
				_t3 = c.T3;
				_t5 = c.T5;

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public Configurator Build() 
			{
				var c = new Configurator();

				c.T3 = _t3;
				c.T5 = _t5;

				return c;
			}
			#endregion
		}
		#endregion
	}
}