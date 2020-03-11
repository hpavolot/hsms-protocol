#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.data
{
	[TestClass]
	public class StringItemTests
	{
		#region Initialization tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_CreateCorrectStringItem()
		{
			var stringItem = new StringItem("lena", 4);

			Assert.IsTrue(stringItem.Value == "lena");
			Assert.IsTrue(stringItem.Length == 4);
			Assert.IsTrue(stringItem.Type == Format.A);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_TrimStringIfProvidedLengthIsBigger()
		{
			var stringItem = new StringItem("lena", 2);

			Assert.IsTrue(stringItem.Value == "le");
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_TrimStringIfProvidedLengthIsSmaller()
		{
			var stringItem = new StringItem("lena", 8);

			Assert.IsTrue(stringItem.Value == "lena    ") ;
		}
		#endregion

		#region Equality tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_BeEqualIfStringItemsEqual()
		{
			var stringItem = new StringItem("lena", 4);
			var stringItem2 = new StringItem("lena", 4);
			Assert.IsTrue(stringItem.Equals(stringItem2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForNull()
		{
			var stringItem = new StringItem("lena", 4);
			Assert.IsFalse(stringItem.Equals(null));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferetObjectTypes()
		{
			var stringItem = new StringItem("lena", 4);
			var obj = "test";
			Assert.IsFalse(stringItem.Equals(obj));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentDataTypes()
		{
			var stringItem = new StringItem("lena", 4);
			var numericItem = new F4(5);
			Assert.IsFalse(stringItem.Equals(numericItem));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentStringValues()
		{
			var stringItem = new StringItem("lena", 4);
			var stringItem2 = new StringItem("test", 4);

			Assert.IsFalse(stringItem.Equals(stringItem2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentStringLength()
		{
			var stringItem = new StringItem("lena", 5);
			var stringItem2 = new StringItem("lena", 2);

			Assert.IsFalse(stringItem.Equals(stringItem2));
		}
		#endregion
	}
}
