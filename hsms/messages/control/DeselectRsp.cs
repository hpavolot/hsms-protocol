#region Usings
#endregion

namespace Semi.Hsms.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public class DeselectRsp : ControlMessage
    {
        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public override MessageType Type => MessageType.DeselectRsp;
        /// <summary>
        /// 
        /// </summary>
        public byte Status { get; protected set; }
        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="context"></param>
        /// <param name="status"></param>
        public DeselectRsp(ushort device, uint context, byte status) : base(device, context)
        {
            Status = status;
        }
        #endregion

        #region Class public methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "deselect rsp";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var m = obj as DeselectRsp;

            if (null == m)
                return false;

            if (Status != m.Status)
                return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();

            hash = hash * 23 + Status.GetHashCode();

            return hash;
        }
        #endregion
    }
}