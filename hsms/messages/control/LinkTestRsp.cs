#region Usings 
#endregion

namespace Semi.Hsms.Messages
{
    public class LinkTestRsp : ControlMessage
    {
        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public override MessageType Type => MessageType.LinktestRsp;
        /// <summary>
        /// 
        /// </summary>
        public override bool IsReplyRequired => false;
        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="context"></param>
        public LinkTestRsp(ushort device, uint context) : base(device, context)
        { 
        }
        #endregion

        #region Class public methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "link test rsp";
        }
        #endregion

    }
}
