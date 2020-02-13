#region Usings
using System;
using System.IO;
using System.Linq;
#endregion

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public class Coder
	{
		#region Class 'Encode' methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sr"></param>
		public static byte[] Encode(Message message)
		{
			byte[] arr = null;

			using (var ms = new MemoryStream())
			{
				using (var writer = new BinaryWriter(ms))
				{
					// SessionId
					writer.Write(BitConverter
					  .GetBytes(message.Device)
					  .Reverse()
					  .ToArray());

					// Byte 2
					writer.Write(byte.MinValue); //what about Reject.req? and linktest == 0xxFFFF

					// Byte 3
					if (message.IsReplyRequired || message.Type == MessageType.LinktestRsp)
						writer.Write(byte.MinValue);
					else
						switch (message)
						{
							case SelectRsp selectRsp:
								writer.Write(selectRsp.Status);
								break;
							case DeselectRsp deselectRsp:
								writer.Write(deselectRsp.Status);
								break;
							case RejectReq rejectReq:
								writer.Write(rejectReq.Reason);
								break;
						}
					// PType
					writer.Write(byte.MinValue);

					// SType
					writer.Write((byte)message.Type);

					// System bytes
					writer.Write(BitConverter
					  .GetBytes(message.Context)
					  .Reverse()
					  .ToArray());
				}
				arr = ms.ToArray();
			}
			return arr;
		}
	
		#region Explicit Encode methods
		/// /// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="selectReq"></param>
		private static void Encode(BinaryWriter writer, SelectReq selectReq)
		{
			// SessionId
			writer.Write(BitConverter
			  .GetBytes(selectReq.Device)
			  .Reverse()
			  .ToArray());

			// Byte 2
			writer.Write(byte.MinValue);

			// Byte 3
			writer.Write(byte.MinValue);

			// PType
			writer.Write(byte.MinValue);

			// SType
			writer.Write((byte)selectReq.Type);

			// System bytes
			writer.Write(BitConverter
			  .GetBytes(selectReq.Context)
			  .Reverse()
			  .ToArray());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="selectRsp"></param>
		private static void Encode(BinaryWriter writer, SelectRsp selectRsp)
		{
			// SessionId
			writer.Write(BitConverter
			  .GetBytes(selectRsp.Device)
			  .Reverse()
			  .ToArray());

			// Byte 2
			writer.Write(byte.MinValue);

			// Byte 3
			writer.Write(selectRsp.Status);

			// PType
			writer.Write(byte.MinValue);

			// SType
			writer.Write((byte)selectRsp.Type);

			// System bytes
			writer.Write(BitConverter
			  .GetBytes(selectRsp.Context)
			  .Reverse()
			  .ToArray());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="deselectReq"></param>
		private static void Encode(BinaryWriter writer, DeselectReq deselectReq)
		{
			// SessionId
			writer.Write(BitConverter
			  .GetBytes(deselectReq.Device)
			  .Reverse()
			  .ToArray());

			// Byte 2
			writer.Write(byte.MinValue);

			// Byte 3
			writer.Write(byte.MinValue);

			// PType
			writer.Write(byte.MinValue);

			// SType
			writer.Write((byte)deselectReq.Type);

			// System bytes
			writer.Write(BitConverter
			  .GetBytes(deselectReq.Context)
			  .Reverse()
			  .ToArray());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="deselectRsp"></param>
		private static void Encode(BinaryWriter writer, DeselectRsp deselectRsp)
		{
			// SessionId
			writer.Write(BitConverter
			  .GetBytes(deselectRsp.Device)
			  .Reverse()
			  .ToArray());

			// Byte 2
			writer.Write(byte.MinValue);

			// Byte 3
			writer.Write(deselectRsp.Status);

			// PType
			writer.Write(byte.MinValue);

			// SType
			writer.Write((byte)deselectRsp.Type);

			// System bytes
			writer.Write(BitConverter
			  .GetBytes(deselectRsp.Context)
			  .Reverse()
			  .ToArray());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="linkTestReq"></param>
		private static void Encode(BinaryWriter writer, LinkTestReq linkTestReq)
		{
			// SessionId
			writer.Write(BitConverter
			  .GetBytes(linkTestReq.Device)
			  .Reverse()
			  .ToArray()); //0xFFFF ?

			// Byte 2
			writer.Write(byte.MinValue);

			// Byte 3
			writer.Write(byte.MinValue);

			// PType
			writer.Write(byte.MinValue);

			// SType
			writer.Write((byte)linkTestReq.Type);

			// System bytes
			writer.Write(BitConverter
			  .GetBytes(linkTestReq.Context)
			  .Reverse()
			  .ToArray());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="linkTestRsp"></param>
		private static void Encode(BinaryWriter writer, LinkTestRsp linkTestRsp)
		{
			// SessionId
			writer.Write(BitConverter
			  .GetBytes(linkTestRsp.Device)
			  .Reverse()
			  .ToArray());

			// Byte 2
			writer.Write(byte.MinValue);

			// Byte 3
			writer.Write(byte.MinValue);

			// PType
			writer.Write(byte.MinValue);

			// SType
			writer.Write((byte)linkTestRsp.Type);

			// System bytes
			writer.Write(BitConverter
			  .GetBytes(linkTestRsp.Context)
			  .Reverse()
			  .ToArray());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="rejectReq"></param>
		private static void Encode(BinaryWriter writer, RejectReq rejectReq)
		{
			// SessionId
			writer.Write(BitConverter
			  .GetBytes(rejectReq.Device)
			  .Reverse()
			  .ToArray());

			// Byte 2
			writer.Write(byte.MinValue); //PType ot Stype of message being rejected ?

			// Byte 3
			writer.Write(rejectReq.Reason);

			// PType
			writer.Write(byte.MinValue);

			// SType
			writer.Write((byte)rejectReq.Type);

			// System bytes
			writer.Write(BitConverter
			  .GetBytes(rejectReq.Context)
			  .Reverse()
			  .ToArray());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="separateReq"></param>
		private static void Encode(BinaryWriter writer, SeparateReq separateReq)
		{
			// SessionId
			writer.Write(BitConverter
			  .GetBytes(separateReq.Device)
			  .Reverse()
			  .ToArray());

			// Byte 2
			writer.Write(byte.MinValue);

			// Byte 3
			writer.Write(byte.MinValue);

			// PType
			writer.Write(byte.MinValue);

			// SType
			writer.Write((byte)separateReq.Type);

			// System bytes
			writer.Write(BitConverter
			  .GetBytes(separateReq.Context)
			  .Reverse()
			  .ToArray());
		}

		#endregion

		#endregion

		#region Class 'Decode' methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static SelectReq Decode(byte[] buffer)
		{
			SelectReq sr = null;

			using (var ms = new MemoryStream(buffer))
			{
				using (var reader = new BinaryReader(ms))
				{
					var device = reader.ReadUInt16();

					var context = reader.ReadUInt32();

					sr = new SelectReq(device, context);
				}
			}

			return sr;
		}
		#endregion
	}
}
