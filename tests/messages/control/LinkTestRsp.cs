#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.control
{
	[TestClass]
	public class LinkTestRspTests
	{
		#region Initialization tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_CreateCorrectLinkTestRsp()
		{
			var linkTestRsp = new LinkTestRsp(1);

			Assert.IsTrue(linkTestRsp.Type == MessageType.LinktestRsp);
			Assert.IsTrue(linkTestRsp.Context == 1);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsPrimaryToFalse()
		{
			var linkTestRsp = new LinkTestRsp(1);

			Assert.IsTrue(!linkTestRsp.IsPrimary);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsReplyRequiredToFalse()
		{
			var linkTestRsp = new LinkTestRsp(1);

			Assert.IsTrue(!linkTestRsp.IsReplyRequired);
		}
		#endregion

		#region Equality tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_BeEqualIfLinkTestRspsEqual()
		{
			var linkTestRsp = new LinkTestRsp(1);
			var linkTestRsp2 = new LinkTestRsp(1);
			Assert.IsTrue(linkTestRsp.Equals(linkTestRsp2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForNull()
		{
			var linkTestRsp = new LinkTestRsp(1);
			Assert.IsFalse(linkTestRsp.Equals(null));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferetObjectTypes()
		{
			var linkTestRsp = new LinkTestRsp(1);
			var obj = "test";
			Assert.IsFalse(linkTestRsp.Equals(obj));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentMessageTypes()
		{
			var linkTestRsp = new LinkTestRsp(1);
			var selectRsp = new SelectRsp(1, 2, 1);
			Assert.IsFalse(linkTestRsp.Equals(selectRsp));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentContext()
		{
			var linkTestRsp = new LinkTestRsp(1);
			var linkTestRsp2 = new LinkTestRsp(2);
			Assert.IsFalse(linkTestRsp.Equals(linkTestRsp2));
		}
		#endregion
	}
}
