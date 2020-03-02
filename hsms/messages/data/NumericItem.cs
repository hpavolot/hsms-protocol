#region Usings
using System;
#endregion

namespace Semi.Hsms.Messages
{
  /// <summary>
  /// 
  /// </summary>
  public class NumericItem<T>: DataItem
	{
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		protected T _value;
		#endregion

		#region Class properties

		#endregion

		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		internal NumericItem( string n, T value )
		{
			_name = n;
			_value = value;

			switch( System.Type.GetTypeCode( typeof( T ) ) )
			{
				case TypeCode.Byte:
					_format = Format.U1;
					break;

				case TypeCode.UInt16:
					_format = Format.U2;
					break;

				case TypeCode.UInt32:
					_format = Format.U4;
					break;

				case TypeCode.UInt64:
					_format = Format.U8;
					break;

				case TypeCode.SByte:
					_format = Format.I1;
					break;

				case TypeCode.Int16:
					_format = Format.I2;
					break;

				case TypeCode.Int32:
					_format = Format.I4;
					break;

				case TypeCode.Int64:
					_format = Format.I8;
					break;

				case TypeCode.Double:
					_format = Format.F8;
					break;

				case TypeCode.Single:
					_format = Format.F4;
					break;

				case TypeCode.Boolean:
					_format = Format.Bool;
					break;
			}
		}
		#endregion
	}
}
