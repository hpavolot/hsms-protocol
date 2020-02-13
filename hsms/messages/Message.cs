#region Usings
using System;
#endregion

namespace Semi.Hsms.Messages
{
  /// <summary>
  /// 
  /// </summary>
  public abstract class Message
  {
    #region Class members
    #endregion

    #region Class properties
    /// <summary>
    /// 
    /// </summary>
    public ushort Device { get; protected set; }
    /// <summary>
    /// 
    /// </summary>
    public uint Context { get; protected set; }
    /// <summary>
    /// 
    /// </summary>
    public abstract MessageType Type { get; }
    /// <summary>
		/// 
		/// </summary>
		public abstract bool IsReplyRequired { get; }
    /// <summary>
    /// 
    /// </summary>
    public abstract bool IsPrimary { get; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime Time { get; protected set; }
    #endregion

    #region Class initialization
    /// <summary>
    /// 
    /// </summary>
    /// <param name="device"></param>
    /// <param name="context"></param>
    public Message( ushort device, uint context )
    {
      Device = device;
      Context = context;

      Time = DateTime.Now;
    }
    #endregion
  } 
}
