using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#region Usings
#endregion

namespace hsms.tests.messages.control
{
	[TestClass]
	public class SelectRspTests
	{
		#region Initialization tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_CreateCorrectSelectRsp()
		{
			var selectRsp = new SelectRsp(1, 2, 1);

			Assert.IsTrue(selectRsp.Type == MessageType.SelectRsp);
			Assert.IsTrue(selectRsp.Device == 1);
			Assert.IsTrue(selectRsp.Context == 2);
			Assert.IsTrue(selectRsp.Status == 1);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsPrimaryToFalse()
		{
			var selectRsp = new SelectRsp(1, 2, 3);

			Assert.IsTrue(!selectRsp.IsPrimary);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsReplyRequiredToFalse()
		{
			var selectRsp = new SelectRsp(1, 2, 3);

			Assert.IsTrue(!selectRsp.IsReplyRequired);
		}
#endregion

		#region Equality tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_BeEqualIfSelectRspsEqual()
		{
			var selectRsp = new SelectRsp(1, 2, 3);
			var selectRsp2 = new SelectRsp(1, 2, 3);
			Assert.IsTrue(selectRsp.Equals(selectRsp2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForNull()
		{
			var selectRsp = new SelectRsp(1, 2, 3);
			Assert.IsFalse(selectRsp.Equals(null));
		}

		[TestMethod]
		public void Should_FailEqualityForDifferetObjectTypes()
		{
			var selectRsp = new SelectRsp(1, 2, 3);
			var obj = "test";
			Assert.IsFalse(selectRsp.Equals(obj));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentMessageTypes()
		{
			var selectRsp = new SelectRsp(1, 2, 3);
			var linkTestReq = new LinkTestReq(1);
			Assert.IsFalse(selectRsp.Equals(linkTestReq));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentDevice()
		{
			var selectRsp = new SelectRsp(2, 1, 3);
			var selectRsp2 = new SelectRsp(1, 1, 3);
			Assert.IsFalse(selectRsp.Equals(selectRsp2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentContext()
		{
			var selectRsp = new SelectRsp(1, 1, 3);
			var selectRsp2 = new SelectRsp(1, 2, 3);
			Assert.IsFalse(selectRsp.Equals(selectRsp2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentStatus()
		{
			var selectRsp = new SelectRsp(1, 2, 3);
			var selectRsp2 = new SelectRsp(1, 2, 4);
			Assert.IsFalse(selectRsp.Equals(selectRsp2));
		}

#endregion

	}

}
