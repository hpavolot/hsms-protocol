#region Usings
using System;
#endregion

namespace Semi.Hsms.Messages
{
  /// <summary>
  /// 
  /// </summary>
  public class NumericItem<T> : DataItem
		where T : IComparable, new()
	{
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		protected T _value;
		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public T Value => _value;
		#endregion

		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		protected NumericItem( T value, Format f )
		{
			_value = value;
			_format = f;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"[{_format.ToString()}] {Value}";
		}
		#endregion

		#region Class internal structs
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public class I2 : NumericItem<short> 
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public I2( short v ) 
			:base( v, Format.I2 )
		{

		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public class U2 : NumericItem<ushort>
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public U2( ushort v )
			: base( v, Format.U2 )
		{

		}
		#endregion
	}
}
