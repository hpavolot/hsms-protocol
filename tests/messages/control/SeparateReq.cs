#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.control
{
	[TestClass]
	public class SeparateReqTests
	{
		#region Initialization tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_CreateCorrectSelectReq()
		{
			var separateReq = new SeparateReq(1, 2);

			Assert.IsTrue(separateReq.Type == MessageType.SeparateReq);
			Assert.IsTrue(separateReq.Device == 1);
			Assert.IsTrue(separateReq.Context == 2);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsPrimaryToTrue()
		{
			var separateReq = new SeparateReq(1, 2);

			Assert.IsTrue(separateReq.IsPrimary);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsReplyRequiredToFalse()
		{
			var separateReq = new SeparateReq(1, 2);

			Assert.IsTrue(!separateReq.IsReplyRequired);
		}
		#endregion

		#region Equality tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_BeEqualIfSparateReqsEqual()
		{
			var separateReq = new SeparateReq(1, 2);
			var separateReq2 = new SeparateReq(1, 2);
			Assert.IsTrue(separateReq.Equals(separateReq2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForNull()
		{
			var separateReq = new SeparateReq(1, 2);
			Assert.IsFalse(separateReq.Equals(null));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferetObjectTypes()
		{
			var separateReq = new SeparateReq(1, 2);
			var obj = "test";
			Assert.IsFalse(separateReq.Equals(obj));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentMessageTypes()
		{
			var separateReq = new SeparateReq(1, 2);
			var selectRsp = new SelectRsp(1, 2, 1);
			Assert.IsFalse(separateReq.Equals(selectRsp));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentDevice()
		{
			var separateReq = new SeparateReq(2, 1);
			var separateReq2 = new SeparateReq(1, 1);
			Assert.IsFalse(separateReq.Equals(separateReq2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentContext()
		{
			var separateReq = new SeparateReq(1, 1);
			var separateReq2 = new SeparateReq(1, 2);
			Assert.IsFalse(separateReq.Equals(separateReq2));
		}
		#endregion
	}

}
