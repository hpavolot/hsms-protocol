#region Usings
#endregion

namespace Semi.Hsms.Messages
{
    public class DeselctReq : ControlMessage
    {
        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public override MessageType Type => MessageType.DeselectReq;
        /// <summary>
        /// 
        /// </summary>
        public override bool IsReplyRequired => true;
        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="context"></param>
        public DeselctReq(ushort device, uint context) : base(device, context)
        {

        }
        #endregion

        #region Class public methods
        public override string ToString()
        {
            return "deselect req";
        }
        #endregion

    }
}
