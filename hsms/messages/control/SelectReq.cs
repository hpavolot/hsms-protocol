#region Usings
#endregion

namespace Semi.Hsms.Messages
{
    public class SelectReq : ControlMessage
    {
        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public override MessageType Type
        {
            get { return MessageType.SelectReq; }
        }
        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="context"></param>
        public SelectReq(ushort device, uint context )
            : base(device, context)
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
            return $"select req";
        }
        #endregion
    }
}