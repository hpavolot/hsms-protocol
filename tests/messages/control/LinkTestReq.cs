#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.control
{

	[TestClass]
	public class LinkTestReqTests
	{
		#region Initialization tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_CreateCorrectLinkTestReq()
		{
			var linkTestReq = new LinkTestReq(1);

			Assert.IsTrue(linkTestReq.Type == MessageType.LinktestReq);
			Assert.IsTrue(linkTestReq.Context == 1);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsPrimaryToTrue()
		{
			var linkTestReq = new LinkTestReq(1);

			Assert.IsTrue(linkTestReq.IsPrimary);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsReplyRequiredToTrue()
		{
			var linkTestReq = new LinkTestReq(1);

			Assert.IsTrue(linkTestReq.IsReplyRequired);
		}

		#endregion

		#region Equality tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_BeEqualIfLinkTestReqsEqual()
		{
			var linkTestReq = new LinkTestReq(1);
			var linkTestReq2 = new LinkTestReq(1);
			Assert.IsTrue(linkTestReq.Equals(linkTestReq2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForNull()
		{
			var linkTestReq = new LinkTestReq(1);
			Assert.IsFalse(linkTestReq.Equals(null));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferetObjectTypes()
		{
			var linkTestReq = new LinkTestReq(1);
			var obj = "test";
			Assert.IsFalse(linkTestReq.Equals(obj));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentMessageTypes()
		{
			var linkTestReq = new LinkTestReq(1);
			var selectRsp = new SelectRsp(1, 2, 1);
			Assert.IsFalse(linkTestReq.Equals(selectRsp));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentContext()
		{
			var linkTestReq = new LinkTestReq(1);
			var linkTestReq2 = new LinkTestReq(2);
			Assert.IsFalse(linkTestReq.Equals(linkTestReq2));
		}
		#endregion
	}
}
