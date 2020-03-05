namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public class StringItem : DataItem
	{
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		private int _length;
		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public string Value { get; } 
		/// <summary>
		/// 
		/// </summary>
		public int Length => _length;
		#endregion

		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="length"></param>
		public StringItem( string value, int length )
		{
			Value = ( value.Length > length ) ?
				value.Substring( 0, length ) :
				value.PadRight( length );

			_length = length;
			_format = Format.A;
		}
		#endregion

		#region Class public methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"[{_format.ToString()}] {Value}";
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals( object obj )
		{
			if( !base.Equals( obj ) )
				return false;

			if( !( obj is StringItem si ) )
				return false;

			if( Value != si.Value )
				return false;

			if( Length != si.Length )
				return false;

			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			var hash = 17;
			hash = hash * 23 + Value.GetHashCode();
			hash = hash * 23 + Length.GetHashCode();
			return hash;
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class A : StringItem
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public A( string v, int len )
				: base( v, len )
		{
		}
		#endregion
	}
}