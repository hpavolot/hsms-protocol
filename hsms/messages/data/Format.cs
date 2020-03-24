#region Usings
#endregion

namespace Semi.Hsms.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public enum Format
    {
        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        List = 0,
        /// <summary>
        /// Binary
        /// </summary>
        Bin = 32,
        /// <summary>
        /// Boolean
        /// </summary>
        Bool = 36,
        /// <summary>
        /// ASCII
        /// </summary>
        A = 64,
        /// <summary>
        /// 8 byte integer signed 
        /// </summary>
        I8 = 96,
        /// <summary>
        /// 4 byte integer signed
        /// </summary>
        I4 = 112,
        /// <summary>
        /// 2 byte integer signed
        /// </summary>
        I2 = 104,
        /// <summary>
        ///  1 byte integer signed 
        /// </summary>
        I1 = 100,
        /// <summary>
        /// 8 byte floating point
        /// </summary>
        F8 = 128,
        /// <summary>
        /// 4 byte floating point
        /// </summary>
        F4 = 144,
        /// <summary>
        /// 8 byte integer unsigned
        /// </summary>
        U8 = 160,
        /// <summary>
        ///  does not need need according to spec?
        /// </summary>
        U4 = 176,
        /// <summary>
        /// byte integer unsigned
        /// </summary>
        U2 = 168,
        /// <summary>
        /// 1 byte integer unsigned
        /// </summary>
        U1 = 164
        #endregion
    }    
}