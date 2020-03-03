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
	public class I1 : NumericItem<sbyte> 
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public I1( sbyte v ) 
			:base( v, Format.I1 )
		{

		}
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
	public class I4 : NumericItem<int> 
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public I4( int v ) 
			:base( v, Format.I4 )
		{

		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class I8 : NumericItem<long> 
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public I8( long v ) 
			:base( v, Format.I8 )
		{

		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class F4 : NumericItem<float> 
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public F4( float v ) 
			:base( v, Format.F4 )
		{

		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class F8 : NumericItem<double> 
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public F8( double v ) 
			:base( v, Format.F8 )
		{

		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class U1 : NumericItem<byte>
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public U1( byte v )
			: base( v, Format.U1 )
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
	/// <summary>
	/// 
	/// </summary>
	public class U4 : NumericItem<uint>
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public U4( uint v )
			: base( v, Format.U4 )
		{

		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class U8 : NumericItem<ulong>
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public U8( ulong v )
			: base( v, Format.U8 )
		{

		}
		#endregion
	}
}
