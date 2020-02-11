#region Usings
using Semi.Hsms.Messages;
using System.IO;
#endregion

namespace Semi.Hsms.Coding
{
  /// <summary>
  /// 
  /// </summary>
  public class Decoder
  {
    #region Class public methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sr"></param>
    public static SelectReq Decode( byte [] buffer ) 
    {
      SelectReq sr = null;

      using( var ms = new MemoryStream( buffer ) )
      {
        using( var reader = new BinaryReader( ms ) )
        {
          var device = reader.ReadUInt16();

          var context = reader.ReadUInt32();

          sr = new SelectReq( device, context );
        }
      }

      return sr;
    }
    #endregion
  }
}
