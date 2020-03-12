#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.data
{
	[TestClass]
	public class ListItemTests
	{
		#region Initialization tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_CreateCorrectListItem()
		{
			var listItem = new ListItem(
				new I2(2),
				new A("hello", 5),
				new ListItem(
					new F8(12.56)));

			Assert.IsTrue(listItem.Type == Format.List);
		}
		#endregion

		#region Equality tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_BeEqualIfNumericItemsEqual()
		{
			var listItem = new ListItem(
				new I2(2),
				new A("hello", 5),
				new ListItem(
					new F8(12.56)));

			var listItem2 = new ListItem(
				new I2(2),
				new A("hello", 5),
				new ListItem(
					new F8(12.56)));


			Assert.IsTrue(listItem.Equals(listItem2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForNull()
		{
			var listItem = new ListItem(
				new I2(2),
				new A("hello", 5),
				new ListItem(
					new F8(12.56)));

			Assert.IsFalse(listItem.Equals(null));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentSubItemsCount()
		{
			var listItem = new ListItem(
				new I2(2),
				new A("hello", 5),
				new ListItem(
					new F8(12.56)));

			var listItem2 = new ListItem(
				new I2(2),
				new A("hello", 5),
				new ListItem(
					new F8(12.56),
					new I8(7)));

			Assert.IsFalse(listItem.Equals(listItem2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentSubItems()
		{
			var listItem = new ListItem(
				new I2(2),
				new A("hello", 5),
				new ListItem(
					new F8(12.56)));

			var listItem2 = new ListItem(
				new I2(2),
				new A("hello world", 10),
				new U2(3));

			Assert.IsFalse(listItem.Equals(listItem2));
		}
		#endregion
	}
}
