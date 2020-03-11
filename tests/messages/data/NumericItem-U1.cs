#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.data
{
	[TestClass]
	public class NumericItems_U1Tests
	{
		#region Initialization tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_CreateCorrectU1Item()
		{
			var numericItem = new U1 (1);

			Assert.IsTrue(numericItem.Value == 1);
			Assert.IsTrue(numericItem.Type == Format.U1);
		}
		#endregion

		#region Equality tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_BeEqualIfNumericItemsEqual()
		{
			var numericItem = new U1(1);
			var numericItem2 = new U1(1);
			Assert.IsTrue(numericItem.Equals(numericItem2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForNull()
		{
			var numericItem = new U1(1);
			Assert.IsFalse(numericItem.Equals(null));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferetObjectTypes()
		{
			var numericItem = new U1(1);
			var obj = "test";
			Assert.IsFalse(numericItem.Equals(obj));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentDataTypes()
		{
			var numericItem = new U1(1);
			var numericItem2 = new F4(5);
			Assert.IsFalse(numericItem.Equals(numericItem2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentNumericValues()
		{
			var numericItem = new U1(1);
			var numericItem2 = new U1(2);

			Assert.IsFalse(numericItem.Equals(numericItem2));
		}
		#endregion
	}
}
