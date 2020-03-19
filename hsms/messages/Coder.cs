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
	public class Coder
	{
		#region Class constants
		/// <summary>
		/// 
		/// </summary>
		public const int MESSAGE_PREFIX_LEN = 4;
		#endregion

		#region Class 'Encode' methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sr"></param>
		public static byte [] Encode( Message m )
		{
			byte [] msgBytes = null;

			using( var ms = new MemoryStream() )
			{
				using( var w = new BinaryWriter( ms ) )
				{
					// SessionId
					w.WriteDevice( m );

					// Byte 2
					w.Write( byte.MinValue );

					// Byte 3
					w.WriteByte3( m );

					// PType
					w.Write( byte.MinValue );

					// SType
					w.WriteType( m );

					// System bytes
					w.WriteContext( m );

					w.WriteBody( m );
				}

				msgBytes = ms.PrependLengthBytes();
			}

			return msgBytes;
		}
		#endregion

		#region Class 'Decode' methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static Message Decode( byte [] buffer )
		{
			using( var ms = new MemoryStream( buffer ) )
			{
				using( var r = new BinaryReader( ms ) )
				{
					var device = BitConverter.ToUInt16(	r
						.ReadBytes(2)
						.Reverse()
						.ToArray(), 0);

					var hb2 = r.ReadByte();

					var hb3 = r.ReadByte();

					var ptype = r.ReadByte();

					var stype = r.ReadByte();

					var context = BitConverter.ToUInt32( r
						.ReadBytes( 4 )
						.Reverse()
						.ToArray(), 0 );

					switch ( ( MessageType )stype )
					{
						case MessageType.SelectReq:
							return new SelectReq( device, context );

						case MessageType.SelectRsp:
							return new SelectRsp( device, context, hb3 );

						//case MessageType.DeselectReq:
						//	{
						//		return new DeselectReq(device, context);
						//	}
						//case MessageType.DeselectRsp:
						//	{
						//		var status = reader.ReadByte();
						//		return new DeselectRsp(device, context, status);
						//	}
						//case MessageType.LinktestReq:
						//	{
						//		return new LinkTestReq(context);
						//	}
						//case MessageType.LinktestRsp:
						//	{
						//		return new LinkTestRsp(context);
						//	}
						//case MessageType.RejectReq:
						//	{
						//		var reason = reader.ReadByte();
						//		return new RejectReq(device, context, reason);
						//	}
						//case MessageType.SeparateReq:
						//	{
						//		return new SeparateReq(device, context);
						//	}
						//default:
						//	return null;


					}
				}
			}

			return null;
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	internal static class CoderExt
	{
		#region Class public methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public static void WriteDevice( this BinaryWriter writer, Message m )
		{
			writer.Write( BitConverter
					.GetBytes( m.Device )
					.Reverse()
					.ToArray() );
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public static void WriteContext( this BinaryWriter writer, Message m )
		{
			writer.Write( BitConverter
					.GetBytes( m.Context )
					.Reverse()
					.ToArray() );
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public static void WriteByte3( this BinaryWriter writer, Message m )
		{
			switch( m )
			{
				case SelectRsp selectRsp:
					writer.Write( selectRsp.Status );
					break;

				case DeselectRsp deselectRsp:
					writer.Write( deselectRsp.Status );
					break;

				case RejectReq rejectReq:
					writer.Write( rejectReq.Reason );
					break;

				default:
					writer.Write( byte.MinValue );
					break;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public static void WriteType( this BinaryWriter writer, Message m )
		{
			writer.Write( ( byte )m.Type );
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public static void WriteBody( this BinaryWriter writer, Message m )
		{
			var dm = m as DataMessage;

			if( null == dm )
				return;

			// todo
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public static byte [] PrependLengthBytes( this MemoryStream ms )
		{
			var arr = ms.ToArray();

			var arrFinal = new byte [ arr.Length + 4 ];

			var bytesForLength = BitConverter
					.GetBytes( arr.Length )
					.Reverse()
					.ToArray();

			bytesForLength.CopyTo( arrFinal, 0 );
			arr.CopyTo( arrFinal, 4 );

			return arrFinal;
		}
		#endregion
	}
}