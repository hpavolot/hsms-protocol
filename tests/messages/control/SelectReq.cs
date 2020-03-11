#region Usings 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.messages.control;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.control
{
	[TestClass]
	public class SelectReqTests
	{
		#region Initialization tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_CreateCorrectSelectReq()
		{
			var selectReq = new SelectReq(1, 2);

			Assert.IsTrue(selectReq.Type == MessageType.SelectReq);
			Assert.IsTrue(selectReq.Device == 1);
			Assert.IsTrue(selectReq.Context == 2);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsPrimaryToTrue()
		{
			var selectReq = new SelectReq(1, 2);

			Assert.IsTrue(selectReq.IsPrimary);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsReplyRequiredToTrue()
		{
			var selectReq = new SelectReq(1, 2);

			Assert.IsTrue(selectReq.IsReplyRequired);
		}

		#endregion

		#region Equality tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_BeEqualIfSelectReqsEqual()
		{
			var selectReq = new SelectReq(1, 2);
			var selectReq2 = new SelectReq(1, 2);
			Assert.IsTrue(selectReq.Equals(selectReq2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForNull()
		{
			var selectReq = new SelectReq(1, 2);
			Assert.IsFalse(selectReq.Equals(null));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferetObjectTypes()
		{
			var selectReq = new SelectReq(1, 2);
			var obj = "test";
			Assert.IsFalse(selectReq.Equals(obj));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentMessageTypes()
		{
			var selectReq = new SelectReq(1, 2);
			var selectRsp = new SelectRsp(1, 2, 1);
			Assert.IsFalse(selectReq.Equals(selectRsp));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentDevice()
		{
			var selectReq = new SelectReq(2, 1);
			var selectReq2 = new SelectReq(1, 1);
			Assert.IsFalse(selectReq.Equals(selectReq2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentContext()
		{
			var selectReq = new SelectReq(1, 1);
			var selectReq2 = new SelectReq(1, 2);
			Assert.IsFalse(selectReq.Equals(selectReq2));
		}
		#endregion
	}

}
