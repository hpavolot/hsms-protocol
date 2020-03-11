#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.control
{
	#region Initialization tests
	[TestClass]
	public class DeselectRspTests
	{
		[TestMethod]
		public void Should_CreateCorrectDeselectRsp()
		{
			var deselectRsp = new DeselectRsp(1, 2, 3);

			Assert.IsTrue(deselectRsp.Type == MessageType.DeselectRsp);
			Assert.IsTrue(deselectRsp.Device == 1);
			Assert.IsTrue(deselectRsp.Context == 2);
			Assert.IsTrue(deselectRsp.Status == 3);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsPrimaryToFalse()
		{
			var deselectRsp = new DeselectRsp(1, 2, 3);

			Assert.IsTrue(!deselectRsp.IsPrimary);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIsReplyRequiredToFalse()
		{
			var deselectRsp = new DeselectRsp(1, 2, 3);

			Assert.IsTrue(!deselectRsp.IsReplyRequired);
		}
		#endregion

		#region Equality tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_BeEqualIfDeselectRspsEqual()
		{
			var deselectRsp = new DeselectRsp(1, 2, 3);
			var deselectRsp2 = new DeselectRsp(1, 2, 3);
			Assert.IsTrue(deselectRsp.Equals(deselectRsp2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForNull()
		{
			var deselectRsp = new DeselectRsp(1, 2, 3);
			Assert.IsFalse(deselectRsp.Equals(null));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferetObjectTypes()
		{
			var deselectRsp = new DeselectRsp(1, 2, 3);
			var obj = "test";
			Assert.IsFalse(deselectRsp.Equals(obj));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentMessageTypes()
		{
			var deselectRsp = new DeselectRsp(1, 2, 3);
			var linkTestRsp = new LinkTestRsp(1);
			Assert.IsFalse(deselectRsp.Equals(linkTestRsp));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentDevice()
		{
			var deselectRsp = new DeselectRsp(2, 1, 3);
			var deselectRsp2 = new DeselectRsp(1, 1, 3);
			Assert.IsFalse(deselectRsp.Equals(deselectRsp2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentContext()
		{
			var deselectRsp = new DeselectRsp(1, 1, 3);
			var deselectRsp2 = new DeselectRsp(1, 2, 3);
			Assert.IsFalse(deselectRsp.Equals(deselectRsp2));
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_FailEqualityForDifferentStatus()
		{
			var deselectRsp = new DeselectRsp(1, 2, 3);
			var deselectRsp2 = new DeselectRsp(1, 2, 4);
			Assert.IsFalse(deselectRsp.Equals(deselectRsp2));
		}

		#endregion
	}

}
