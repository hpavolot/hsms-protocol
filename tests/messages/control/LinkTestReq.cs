#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.control
{
    [TestClass]
    public class LinkTestReqTests
    {
        [TestMethod]
        public void Should_CreateCorrectLinkTestReq()
        {
            var linkTestReq = new LinkTestReq(1);

            Assert.IsTrue(linkTestReq.Type == MessageType.LinktestReq);
            Assert.IsTrue(linkTestReq.Context == 1);
        }

        [TestMethod]
        public void Should_SetIsPrimaryToTrue()
        {
            var linkTestReq = new LinkTestReq(1);

            Assert.IsTrue(linkTestReq.IsPrimary == true);
        }

        [TestMethod]
        public void Should_SetIsReplyRequiredToFalse()
        {
            var linkTestReq = new LinkTestReq(1);

            Assert.IsTrue(linkTestReq.IsReplyRequired == true);
        }

        [TestMethod]
        public void Should_BeEqualIfLinkTestReqsEqual()
        {
            var linkTestReq = new LinkTestReq(1);
            var linkTestReq2 = new LinkTestReq(1);
            Assert.IsTrue(linkTestReq.Equals(linkTestReq2));
        }

        [TestMethod]
        public void Should_FailEqualityForNull()
        {
            var linkTestReq = new LinkTestReq(1);
            Assert.IsFalse(linkTestReq.Equals(null));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferetObjectTypes()
        {
            var linkTestReq = new LinkTestReq(1);
            var obj = "test";
            Assert.IsFalse(linkTestReq.Equals(obj));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentMessageTypes()
        {
            var linkTestReq = new LinkTestReq(1);
            var selectRsp = new SelectRsp(1, 2, 1);
            Assert.IsFalse(linkTestReq.Equals(selectRsp));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentContext()
        {
            var linkTestReq = new LinkTestReq(1);
            var linkTestReq2 = new LinkTestReq(2);
            Assert.IsFalse(linkTestReq.Equals(linkTestReq2));
        }
    }

}
