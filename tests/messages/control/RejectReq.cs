#region usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.control
{
    [TestClass]
    public class RejectReqTests
    {
		#region Initialization tests
        /// <summary>
        /// 
        /// </summary>
		[TestMethod]
        public void Should_CreateCorrectRejectReq()
        {
            var rejectReq = new RejectReq(1, 2, 1);

            Assert.IsTrue(rejectReq.Type == MessageType.RejectReq);
            Assert.IsTrue(rejectReq.Device == 1);
            Assert.IsTrue(rejectReq.Context == 2);
            Assert.IsTrue(rejectReq.Reason == 1);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Should_SetIsPrimaryToTrue()
        {
            var rejectReq = new RejectReq(1, 2, 1);

            Assert.IsTrue(rejectReq.IsPrimary);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Should_SetIsReplyRequiredToFalse()
        {
            var rejectReq = new RejectReq(1, 2, 1);

            Assert.IsTrue(!rejectReq.IsReplyRequired);
        }

		#endregion

		#region Equality tests
        /// <summary>
        /// 
        /// </summary>
		[TestMethod]
        public void Should_BeEqualIfRejectReqsEqual()
        {
            var rejectReq = new RejectReq(1, 2, 3);
            var rejectReq2 = new RejectReq(1, 2, 3);
            Assert.IsTrue(rejectReq.Equals(rejectReq2));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Should_FailEqualityForNull()
        {
            var rejectReq = new RejectReq(1, 2, 3);
            Assert.IsFalse(rejectReq.Equals(null));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Should_FailEqualityForDifferetObjectTypes()
        {
            var rejectReq = new RejectReq(1, 2, 3);
            var obj = "test";
            Assert.IsFalse(rejectReq.Equals(obj));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Should_FailEqualityForDifferentMessageTypes()
        {
            var rejectReq = new RejectReq(1, 2, 3);
            var linkTestReq = new LinkTestReq(1);
            Assert.IsFalse(rejectReq.Equals(linkTestReq));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Should_FailEqualityForDifferentDevice()
        {
            var rejectReq = new RejectReq(2, 1, 3);
            var rejectReq2 = new RejectReq(1, 1, 3);
            Assert.IsFalse(rejectReq.Equals(rejectReq2));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentContext()
        {
            var rejectReq = new RejectReq(1, 1, 3);
            var rejectReq2 = new RejectReq(1, 2, 3);
            Assert.IsFalse(rejectReq.Equals(rejectReq2));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Should_FailEqualityForDifferentReason()
        {
            var rejectReq = new RejectReq(1, 2, 3);
            var rejectReq2 = new RejectReq(1, 2, 4);
            Assert.IsFalse(rejectReq.Equals(rejectReq2));
        }
		#endregion
	}
}
