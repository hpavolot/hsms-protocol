#region Usings
using System;
using System.IO;
using System.Linq;
#endregion

namespace Semi.Hsms.Messages
{
  /// <summary>
  /// 
  /// </summary>
  public class Coder
  {
    #region Class 'Encode' methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sr"></param>
    public static byte [] Encode( Message m ) 
    {
      byte [] arr = null;

      using( var ms = new MemoryStream() )
      {
        using( var writer = new BinaryWriter( ms ) )
        {
          switch( m )
          {
            case SelectReq sreq:
              Encode( writer, sreq );
              break;

            case SelectRsp srsp:
              Encode( writer, srsp );
              break;
          }

          arr = ms.ToArray();
        }
      }

      return arr;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="w"></param>
    /// <param name="sr"></param>
    private static void Encode( BinaryWriter w, SelectReq sr ) 
    {
      // SessionId
      w.Write( BitConverter
        .GetBytes( sr.Device )
        .Reverse()
        .ToArray());

      // Byte 2
      w.Write( byte.MinValue );

      // Byte 3
      w.Write( byte.MinValue );

      // PType
      w.Write( byte.MinValue );

      // SType
      w.Write( ( byte )sr.Type );

      // System bytes
      w.Write( BitConverter
        .GetBytes( sr.Context )
        .Reverse()
        .ToArray() );
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="sr"></param>
    /// <returns></returns>
    private static void Encode( BinaryWriter w, SelectRsp sr )
    {
      // SessionId
      w.Write( BitConverter
        .GetBytes( sr.Device )
        .Reverse()
        .ToArray() );

      // Byte 2
      w.Write( byte.MinValue );

      // Byte 3
      w.Write( sr.Status );

      // PType
      w.Write( byte.MinValue );

      // SType
      w.Write( ( byte )sr.Type );

      // System bytes
      w.Write( BitConverter
        .GetBytes( sr.Context )
        .Reverse()
        .ToArray() );
    }
    #endregion

    #region Class 'Decode' methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
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
