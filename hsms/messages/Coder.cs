#region Usings
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
				foreach (var item in dataItems)
				{
					var type = item.Type;

					if (type.IsNumeric())
					{
						writer.WriteNumericItem(item);
					}

					if (type.IsString())
					{
						writer.WriteStringItem(item);
					}

					if (type.IsList())
					{
						writer.WriteByte((byte)type | 1);
						writer.WriteByte(dataItems.Count());

						var sublist = item as ListItem;
						ms.WriteDataItems(sublist.Items);
					}
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="item"></param>
		private static void WriteStringItem(this BinaryWriter writer, DataItem item)
		{

			var si = item as StringItem;
			var len = si.Length;

			var numOfBytes = (byte)(len < byte.MaxValue ? 1 : (len > byte.MaxValue && len < ushort.MaxValue ? 2 : 3));

			var itemHeader = (byte)item.Type | numOfBytes;

			writer.WriteByte(itemHeader);
			writer.WriteByte(len);

			writer.Write(Encoding.ASCII
				.GetBytes(si.Value)
				.Reverse()
				.ToArray());

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="item"></param>
		private static void WriteNumericItem(this BinaryWriter writer, DataItem item)
		{
			writer.WriteByte((byte)item.Type | 1);

			switch (item.Type)
			{
				case Format.I1:
					var i1 = item as NumericItem<sbyte>;
					writer.WriteByte(1);
					writer.Write(BitConverter
						.GetBytes(i1.Value)
						.Reverse()
						.ToArray());
					break;

				case Format.I2:
					var i2 = item as NumericItem<short>;
					writer.WriteByte(2);
					writer.Write(BitConverter
						.GetBytes(i2.Value)
						.Reverse()
						.ToArray());
					break;

				case Format.I4:
					var i4 = item as NumericItem<int>;
					writer.WriteByte(4);
					writer.Write(BitConverter
						.GetBytes(i4.Value)
						.Reverse()
						.ToArray());
					break;

				case Format.I8:
					var i8 = item as NumericItem<long>;
					writer.WriteByte(8);

					writer.Write(BitConverter
						.GetBytes(i8.Value)
						.Reverse()
						.ToArray());
					break;

				case Format.F4:
					var f4 = item as NumericItem<float>;
					writer.WriteByte(4);
					writer.Write(BitConverter
						.GetBytes(f4.Value)
						.Reverse()
						.ToArray());
					break;

				case Format.F8:
					var f8 = item as NumericItem<double>;
					writer.WriteByte(8);
					writer.Write(BitConverter
						.GetBytes(f8.Value)
						.Reverse()
						.ToArray());
					break;

				case Format.U1:
					var u1 = item as NumericItem<byte>;
					writer.WriteByte(1);
					writer.Write(BitConverter
						.GetBytes(u1.Value)
						.Reverse()
						.ToArray());
					break;

				case Format.U2:
					var u2 = item as NumericItem<ushort>;
					writer.WriteByte(2);
					writer.Write(BitConverter
						.GetBytes(u2.Value)
						.Reverse()
						.ToArray());
					break;

				case Format.U4:
					var u4 = item as NumericItem<uint>;
					writer.WriteByte(4);
					writer.Write(BitConverter
						.GetBytes(u4.Value)
						.Reverse()
						.ToArray());
					break;

				case Format.U8:
					var u8 = item as NumericItem<ulong>;
					writer.WriteByte(8);
					writer.Write(BitConverter
						.GetBytes(u8.Value)
						.Reverse()
						.ToArray());
					break;
			}
		}
		private static void WriteByte(this BinaryWriter writer, int v)
		{
			writer.Write((byte)v);
			;
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

	internal static class FormaExt
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public static bool IsString(this Format format)
		{
			return format == Format.A;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public static bool IsList(this Format format)
		{
			return format == Format.List;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public static bool IsNumeric(this Format format)
		{
			switch (format)
			{
				case Format.I1:
					return true;

				case Format.I2:
					return true;

				case Format.I4:
					return true;

				case Format.I8:
					return true;

				case Format.F4:
					return true;

				case Format.F8:
					return true;

				case Format.U1:
					return true;

				case Format.U2:
					return true;

				case Format.U4:
					return true;

				case Format.U8:
					return true;

				default:
					return false;
			}
		}
	}
}