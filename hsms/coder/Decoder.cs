#region Usings
using Semi.Hsms.Messages;
using System;
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
                        case MessageType.DataMessage:
                            {
                                var dataItem = r.ReadDataItem();

                                return DataMessage.Builder
                                    .Device(device)
                                    .Context(context)
                                    .Stream(hb2)
                                    .Function(hb3)
                                    .Items(dataItem)
                                    .Build();
                            }

                        case MessageType.SelectReq:
                            return new SelectReq(device, context);

                        case MessageType.SelectRsp:
                            return new SelectRsp(device, context, hb3);

                        case MessageType.SeparateReq:
                            return new SeparateReq(device, context);

                        case MessageType.DeselectReq:
                            {
                                return new DeselectReq(device, context);
                            }
                        case MessageType.DeselectRsp:
                            {
                                var status = r.ReadByte();
                                return new DeselectRsp(device, context, status);
                            }
                        case MessageType.LinktestReq:
                            {
                                return new LinkTestReq(context);
                            }
                        case MessageType.LinktestRsp:
                            {
                                return new LinkTestRsp(context);
                            }
                        case MessageType.RejectReq:
                            {
                                var reason = r.ReadByte();
                                return new RejectReq(device, context, reason);
                            }
                        default:
                            return null;
                    }
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    internal static partial class CoderExt
    {
        #region Class Read methods
        /// <summary>
        /// 
        /// </summary>
        public static DataItem ReadDataItem(this BinaryReader r)
        {
            int format = (r.ReadByte());
            Format type = (Format)(format & (format - 1));

            if (type.IsNumeric())
            {
                return r.ReadNumericItem(type);
            }
            else if (type.IsString())
            {
                return r.ReadStringItem();
            }
            else if (type.IsList())
            {
                return r.ReadListItem();
            }
            else if (type.IsBoolean())
            {
                return r.ReadBooleanItem();
            }
            else return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DataItem ReadNumericItem(this BinaryReader r, Format format)
        {
            var len = r.ReadByte();

            switch (format)
            {
                case Format.I1:
                    return new I1((sbyte)r.ReadByte());

                case Format.U1:
                    return new U1(r.ReadByte());

                case Format.I2:
                    return new I2(BitConverter
                                .ToInt16(r
                                .ReadBytes(len)
                                .Reverse()
                                .ToArray(), 0));

                case Format.U2:
                    return new U2(BitConverter
                                .ToUInt16(r
                                .ReadBytes(len)
                                .Reverse()
                                .ToArray(), 0));

                case Format.I4:
                    return new I4(BitConverter
                                .ToInt32(r
                                .ReadBytes(len)
                                .Reverse()
                                .ToArray(), 0));

                case Format.U4:
                    return new U4(BitConverter
                                .ToUInt32(r
                                .ReadBytes(len)
                                .Reverse()
                                .ToArray(), 0));

                case Format.I8:
                    return new I8(BitConverter
                                .ToInt64(r
                                .ReadBytes(len)
                                .Reverse()
                                .ToArray(), 0));

                case Format.U8:
                    return new U8(BitConverter
                                .ToUInt64(r
                                .ReadBytes(len)
                                .Reverse()
                                .ToArray(), 0));

                case Format.F4:
                    return new F4(BitConverter
                                 .ToSingle(r
                                 .ReadBytes(len)
                                 .Reverse()
                                 .ToArray(), 0));

                case Format.F8:
                    return new F8(BitConverter
                                .ToDouble(r
                                .ReadBytes(len)
                                .Reverse()
                                .ToArray(), 0));

                default:
                    return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static DataItem ReadStringItem(this BinaryReader r)
        {
            var len = r.ReadByte();

            var bytes = r.ReadBytes(len);
            var v = Encoding.ASCII.GetString(bytes);

            return new A(v, len);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static DataItem ReadListItem(this BinaryReader r)
        {
            var count = r.ReadByte();

            var items = new DataItem[count];

            for (int i = 0; i < count; i++)
            {
                items[i] = r.ReadDataItem();
            }

            return new ListItem(items);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static DataItem ReadBooleanItem(this BinaryReader r)
        {
            var len = r.ReadByte();

            var v = BitConverter.ToBoolean(r
                    .ReadBytes(len)
                    .ToArray(), 0);

            return new BoolItem(v);
        }
        #endregion
    }
}