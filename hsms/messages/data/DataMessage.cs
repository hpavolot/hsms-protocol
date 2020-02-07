#region Usings
#endregion

namespace Semi.Hsms.Messages
{
  /// <summary>
  /// 
  /// </summary>
  public class DataMessage : Message 
  {
    #region Class members
    /// <summary>
    /// 
    /// </summary>
    protected bool _isReplyRequired;
    #endregion

    #region Class properties
    /// <summary>
    /// 
    /// </summary>
    public byte Function { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public byte Stream { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public override MessageType Type => MessageType.DataMessage;
    /// <summary>
    /// 
    /// </summary>
    public override bool IsReplyRequired => _isReplyRequired;
    /// <summary>
    /// 
    /// </summary>
    public override bool IsPrimary
    {
      get
      {
        return ( 0 != ( Function % 2 ) );
      }
    }
    #endregion

    #region Class initialization
    /// <summary>
    /// 
    /// </summary>
    /// <param name="device"></param>
    /// <param name="context"></param>
    public DataMessage( ushort device, uint context )
      :base( device, context )
    {
      
    }
    #endregion
  }
}
