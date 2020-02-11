#region Usings
using Semi.Hsms.Messages;
using System.IO;
#endregion

namespace Semi.Hsms.Coding
{
  /// <summary>
  /// 
  /// </summary>
  public class Encoder
  {
    #region Class public methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sr"></param>
    public static byte [] Encode( SelectReq sr ) 
    {
      byte [] arr = null;

      using( var ms = new MemoryStream() )
      {
        using( var writer = new BinaryWriter( ms ) )
        {
          writer.Write( sr.Device );




          writer.Write( sr.Context );

          arr = ms.ToArray();
        }
      }

      return arr;
    }
    #endregion
  }
}
