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



  //  public class ControlMessage : Message
  //  {

  //      public ControlMessage(ushort sessionID) : base(sessionID)
  //      {
  //          this.msgLength = 10;
  //          this.header_2 = 0;   
  //      }
  //  }

  //  public class SelectMessage: ControlMessage
  //  {
  //      public SelectMessage(ushort sessionID, SelectStatus selectStatus) :base(sessionID)
  //      {
  //          this.header_3 = selectStatus.;

  //      }
  //  }

  //  public class DeselectMessage : ControlMessage
  //  {
  //      //change Stype for a replace/request
  //  }

  //  public class LinkTestMessage : ControlMessage
  //  {
  //      //change Stype for a replace/request
  //  }
  

  //  public enum PType : byte
  //  {
  //      // "SECS-II Encoding"  = 1
  //  }

  //  public enum SelectStatus : byte
  //  {
  //      Communication_Established = 0,
  //      CommunicationA_lreadyActive = 1,
  //      Connection_NotReady = 2,
  //      Connect_Exhaust = 3,
  //  }

  //  enum DeselectStatus : byte
  //  {
  //      Communication_Ended = 0,
  //      Communication_NotEstablished = 1,
  //      Communication_Busy = 2
  //  }

  //  enum ReasonCode : byte
  //  {
  //      STypeNotSupported = 1,
  //      PTypeNotSupported = 2,
  //      TransactionNotOpen = 3,
  //      EntityNotSelected = 4
  //  }
}
