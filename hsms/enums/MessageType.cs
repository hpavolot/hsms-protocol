#region Usings
#endregion

namespace Semi.Hsms.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public enum MessageType : byte
    {
        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        DataMessage = 0,
        /// <summary>
        /// 
        /// </summary>
        SelectReq = 1,
        /// <summary>
        /// 
        /// </summary>
        SelectRsp = 2,
        /// <summary>
        /// 
        /// </summary>
        DeselectReq = 3,
        /// <summary>
        /// 
        /// </summary>
        DeselectRsp = 4,
        /// <summary>
        /// 
        /// </summary>
        LinktestReq = 5,
        /// <summary>
        /// 
        /// </summary>
        LinktestRsp = 6,
        /// <summary>
        /// 
        /// </summary>
        RejectReq = 7,
        /// <summary>
        /// 
        /// </summary>
        SeparateReq = 9
        #endregion
    }
}