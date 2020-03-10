#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
#endregion

namespace hsms.tests.messages.control
{
    [TestClass]
    public class LinkTestRspTests
    {
        [TestMethod]
        public void Should_CreateCorrectLinkTestRsp()
        {
            var linkTestRsp = new LinkTestRsp(1);

            Assert.IsTrue(linkTestRsp.Type == MessageType.LinktestRsp);
            Assert.IsTrue(linkTestRsp.Context == 1);
        }

        [TestMethod]
        public void Should_SetIsPrimaryToTrue()
        {
            var linkTestRsp = new LinkTestRsp(1);

            Assert.IsTrue(linkTestRsp.IsPrimary == false);
        }

        [TestMethod]
        public void Should_SetIsReplyRequiredToFalse()
        {
            var linkTestRsp = new LinkTestRsp(1);

            Assert.IsTrue(linkTestRsp.IsReplyRequired == false);
        }

        [TestMethod]
        public void Should_BeEqualIfLinkTestRspsEqual()
        {
            var linkTestRsp = new LinkTestRsp(1);
            var linkTestRsp2 = new LinkTestRsp(1);
            Assert.IsTrue(linkTestRsp.Equals(linkTestRsp2));
        }

        [TestMethod]
        public void Should_FailEqualityForNull()
        {
            var linkTestRsp = new LinkTestRsp(1);
            Assert.IsFalse(linkTestRsp.Equals(null));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferetObjectTypes()
        {
            var linkTestRsp = new LinkTestRsp(1);
            var obj = "test";
            Assert.IsFalse(linkTestRsp.Equals(obj));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentMessageTypes()
        {
            var linkTestRsp = new LinkTestRsp(1);
            var selectRsp = new SelectRsp(1, 2, 1);
            Assert.IsFalse(linkTestRsp.Equals(selectRsp));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentContext()
        {
            var linkTestRsp = new LinkTestRsp(1);
            var linkTestRsp2 = new LinkTestRsp(2);
            Assert.IsFalse(linkTestRsp.Equals(linkTestRsp2));
        }
    }

}
