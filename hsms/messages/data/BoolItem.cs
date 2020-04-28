using Semi.Hsms.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public class BoolItem : DataItem
	{
		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public bool Value { get; }
		#endregion

		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="length"></param>
		public BoolItem(bool value)
		{
			Value = value;
			_format = Format.Bool;
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
		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
				return false;

			if (!(obj is BoolItem bi))
				return false;

			if (Value != bi.Value)
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
			return hash;
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class Bool : BoolItem
	{
		#region Class initializations
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		public Bool(bool v)
				: base(v)
		{
		}
		#endregion
	}
}
