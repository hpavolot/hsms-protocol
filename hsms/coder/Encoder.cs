#region Usings
using Semi.Hsms.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#endregion

namespace Semi.Hsms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Coder
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
                    w.WriteBody(m);

                }

                msgBytes = ms.PrependLengthBytes();
            }

            return msgBytes;
        }
        #endregion

    }
    /// <summary>
    /// 
    /// </summary>
    internal static partial class CoderExt
    {
        #region Class Write methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public static void WriteDevice(this BinaryWriter w, Message m)
        {
            w.Write(BitConverter
                    .GetBytes(m.Device)
                    .Reverse()
                    .ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public static void WriteContext(this BinaryWriter w, Message m)
        {
            w.Write(BitConverter
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
        /// <param name="w"></param>
        public static void WriteByte3(this BinaryWriter w, Message m)
        {
            switch (m)
            {
                case DataMessage dataMsg:
                    w.Write(dataMsg.Function);
                    break;

                case SelectRsp selectRsp:
                    w.Write(selectRsp.Status);
                    break;

                case DeselectRsp deselectRsp:
                    w.Write(deselectRsp.Status);
                    break;

                case RejectReq rejectReq:
                    w.Write(rejectReq.Reason);
                    break;

                default:
                    w.Write(byte.MinValue);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public static void WriteType(this BinaryWriter w, Message m)
        {
            w.Write((byte)m.Type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public static void WriteBody(this BinaryWriter w, Message m)
        {
            var dm = m as DataMessage;

            if (null == dm)
                return;
            else
            {
                w.WriteDataItems(dm.Items);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="dataItems"></param>
        public static void WriteDataItems(this BinaryWriter w, IEnumerable<DataItem> dataItems)
        {
            foreach (var item in dataItems)
            {
                var type = item.Type;

                if (type.IsNumeric())
                {
                    w.WriteNumericItem(item);
                }
                else if (type.IsString())
                {
                    w.WriteStringItem(item);
                }
                else if (type.IsList())
                {
                    w.WriteListItem(item);
                }
                else if (type.IsBoolean())
                {
                    w.WriteBooleanItem(item);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="item"></param>
        private static void WriteListItem(this BinaryWriter w, DataItem item)
        {
            var sublist = item as ListItem;
            w.WriteItemHeader(Format.List, sublist.Items.Count());
            w.WriteDataItems(sublist.Items);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="item"></param>
        private static void WriteStringItem(this BinaryWriter w, DataItem item)
        {
            var si = item as StringItem;
            w.WriteItemHeader(Format.A, si.Length);
            w.Write(Encoding.ASCII.GetBytes(si.Value));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="item"></param>
        private static void WriteBooleanItem(this BinaryWriter w, DataItem item)
        {
            var bi = item as BoolItem;
            w.WriteByte((byte)Format.Bool | 1);
            w.WriteByte(1);
            w.Write(bi.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="btNoLenBytes"></param>
        /// <param name="len"></param>
        private static void WriteItemHeader(this BinaryWriter w, Format type, int len)
        {
            byte btNoLenBytes = (byte)((len <= byte.MaxValue) ? 1 : (len <= ushort.MaxValue) ? 2 : 3);

            var btFormatByte = (byte)type | btNoLenBytes;
            w.WriteByte(btFormatByte);

            switch (btNoLenBytes)
            {
                case 1:
                    w.WriteByte(len);
                    break;

                case 2:
                    w.Write(BitConverter
                        .GetBytes(Convert.ToInt16(len))
                        .Reverse()
                        .ToArray());
                    break;

                case 3:
                    w.Write((byte[])BitConverter
                        .GetBytes(len)
                        .Reverse()
                        .Skip(1)
                        .ToArray());
                    break;
            }
        }
        /// <summary>()
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="item"></param>
        private static void WriteNumericItem(this BinaryWriter w, DataItem item)
        {
            w.WriteByte((byte)item.Type | 1);

            var bytes = GetBytes(item);

            w.WriteByte(bytes.Length);

            w.Write(bytes
                .Reverse()
                .ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <returns></returns>
        private static byte[] GetBytes(DataItem item)
        {
            switch (item)
            {
                case NumericItem<sbyte> i1:
                    return new byte[] { (byte)i1.Value };

                case NumericItem<short> i2:
                    return BitConverter.GetBytes(i2.Value);

                case NumericItem<int> i4:
                    return BitConverter.GetBytes(i4.Value);

                case NumericItem<long> i8:
                    return BitConverter.GetBytes(i8.Value);

                case NumericItem<float> f4:
                    return BitConverter.GetBytes(f4.Value);

                case NumericItem<double> f8:
                    return BitConverter.GetBytes(f8.Value);

                case NumericItem<byte> u1:
                    return new byte[] { u1.Value };

                case NumericItem<ushort> u2:
                    return BitConverter.GetBytes(u2.Value);

                case NumericItem<uint> u4:
                    return BitConverter.GetBytes(u4.Value);

                case NumericItem<ulong> u8:
                    return BitConverter.GetBytes(u8.Value);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="v"></param>
        private static void WriteByte(this BinaryWriter w, int v)
        {
            w.Write((byte)v);
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