#region Usings
using System;
using System.Collections.Generic;
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
		public static byte[] Encode(Message m)
		{
			byte[] msgBytes = null;

			using (var ms = new MemoryStream())
			{
				using (var w = new BinaryWriter(ms))
				{
					// SessionId
					w.WriteDevice(m);

					// Byte 2
					w.WriteByte2(m);

					// Byte 3
					w.WriteByte3(m);

					// PType
					w.Write(byte.MinValue);

					// SType
					w.WriteType(m);

					// System bytes
					w.WriteContext(m);

					//Message body
					ms.WriteBody(m);

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
		public static Message Decode(byte[] buffer)
		{
			using (var ms = new MemoryStream(buffer))
			{
				using (var r = new BinaryReader(ms))
				{
					var device = BitConverter.ToUInt16(r
						.ReadBytes(2)
						.Reverse()
						.ToArray(), 0);

					var hb2 = r.ReadByte();

					var hb3 = r.ReadByte();

					var ptype = r.ReadByte();

					var stype = r.ReadByte();

					var context = BitConverter.ToUInt32(r
						.ReadBytes(4)
						.Reverse()
						.ToArray(), 0);

					switch ((MessageType)stype)
					{
						case MessageType.SelectReq:
							return new SelectReq(device, context);

						case MessageType.SelectRsp:
							return new SelectRsp(device, context, hb3);

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
		public static void WriteDevice(this BinaryWriter writer, Message m)
		{
			writer.Write(BitConverter
					.GetBytes(m.Device)
					.Reverse()
					.ToArray());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public static void WriteContext(this BinaryWriter writer, Message m)
		{
			writer.Write(BitConverter
					.GetBytes(m.Context)
					.Reverse()
					.ToArray());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="w"></param>
		/// <param name="m"></param>
		public static void WriteByte2(this BinaryWriter w, Message m)
		{
			if (m is DataMessage dm)
			{
				w.Write(dm.Stream);
			}
			else
			{
				w.Write(byte.MinValue);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public static void WriteByte3(this BinaryWriter writer, Message m)
		{
			switch (m)
			{
				case DataMessage dataMsg:
					writer.Write(dataMsg.Function);
					break;

				case SelectRsp selectRsp:
					writer.Write(selectRsp.Status);
					break;

				case DeselectRsp deselectRsp:
					writer.Write(deselectRsp.Status);
					break;

				case RejectReq rejectReq:
					writer.Write(rejectReq.Reason);
					break;

				default:
					writer.Write(byte.MinValue);
					break;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public static void WriteType(this BinaryWriter writer, Message m)
		{
			writer.Write((byte)m.Type);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public static void WriteBody(this MemoryStream ms, Message m)
		{
			var dm = m as DataMessage;

			if (null == dm)
				return;
			else 
			{ 
				ms.WriteDataItems(dm.Items);
			}
		}			
		public static void WriteDataItems(this MemoryStream ms, IEnumerable<DataItem> dataItems)
		{
			using (var writer = new BinaryWriter(ms))
			{
				//List 
				writer.Write((sbyte)Format.List);

				//Number of Elements in the list
				writer.Write((byte)dataItems.Count());

				foreach (var item in dataItems)
				{
					switch (item.Type)
					{
						case Format.List:
							var sublist = item as ListItem;
							ms.WriteDataItems(sublist.Items);
							break;

						case Format.A:
							writer.Write((byte)Format.A);
							var si = item as StringItem;
							//writer.Write((byte)si.Length);
							writer.Write(si.Value);
							break;

						case Format.I1:
							writer.Write((byte)Format.I1);
							var i1 = item as NumericItem<sbyte>;
							writer.Write((byte) 1);
							writer.Write(i1.Value);
							break;

						case Format.I2:
							writer.Write((byte)Format.I2);
							var i2 = item as NumericItem<short>;
							writer.Write((byte) 2);
							writer.Write((byte)i2.Value);
							break;

						case Format.I4:
							writer.Write((byte)Format.I4);
							var i4 = item as NumericItem<int>;
							writer.Write((byte) 4);
							writer.Write(i4.Value);
							break;

						case Format.I8:
							writer.Write((byte)Format.I8);
							var i8 = item as NumericItem<long>;
							writer.Write((byte) 8);
							writer.Write(i8.Value);
							break;

						case Format.F4:
							writer.Write((byte)Format.F4);
							var f4 = item as NumericItem<float>;
							writer.Write((byte) 4);
							writer.Write(f4.Value);
							break;

						case Format.F8:
							writer.Write((byte)Format.F8);
							var f8 = item as NumericItem<double>;
							writer.Write((byte) 8);
							writer.Write((byte)f8.Value);
							break;

						case Format.U1:
							writer.Write((byte)Format.U1);
							var u1 = item as NumericItem<byte>;
							writer.Write((byte) 1);
							writer.Write(u1.Value);
							break;

						case Format.U2:
							writer.Write((byte)Format.U2);
							var u2 = item as NumericItem<ushort>;
							writer.Write((byte)2);
							writer.Write((byte)u2.Value);
							break;

						case Format.U4:
							writer.Write((byte)Format.U4);
							var u4 = item as NumericItem<uint>;
							writer.Write((byte) 4);
							writer.Write(u4.Value);
							break;

						case Format.U8:
							writer.Write((byte)Format.U8);
							var u8 = item as NumericItem<ulong>;
							writer.Write((byte)8);
							writer.Write(u8.Value);
							break;
					}
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public static byte[] PrependLengthBytes(this MemoryStream ms)
		{
			var arr = ms.ToArray();

			var arrFinal = new byte[arr.Length + 4];

			var bytesForLength = BitConverter
					.GetBytes(arr.Length)
					.Reverse()
					.ToArray();

			bytesForLength.CopyTo(arrFinal, 0);
			arr.CopyTo(arrFinal, 4);

			return arrFinal;
		}
		#endregion
	}
}