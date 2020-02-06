#region Usings
#endregion

namespace Semi.Hsms.Messages
{
    public class RejectReq : ControlMessage
    {
        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public override MessageType Type => MessageType.RejectReq;
        /// <summary>
        /// 
        /// </summary>
        public override bool IsReplyRequired => false;
        /// <summary>
        /// 
        /// </summary>
        public byte Reason { get; protected set; }
        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="context"></param>
        /// <param name="reason"></param>
        public RejectReq(ushort device, uint context, byte reason):base(device,context)
        {
            Reason = reason;
        }
        #endregion

        #region Class public methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "reject req";
        }
        #endregion
    }
}
