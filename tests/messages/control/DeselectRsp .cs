using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
using Semi.Hsms.messages.control;

namespace hsms.tests.messages.control
{
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

        [TestMethod]
        public void Should_SetIsPrimaryToTrue()
        {
            var deselectRsp = new DeselectRsp(1, 2, 3);

            Assert.IsTrue(deselectRsp.IsPrimary == false);
        }

        [TestMethod]
        public void Should_SetIsReplyRequiredToFalse()
        {
            var deselectRsp = new DeselectRsp(1, 2, 3);

            Assert.IsTrue(deselectRsp.IsReplyRequired == false);
        }

        [TestMethod]
        public void Should_BeEqualIfDeselectRspsEqual()
        {
            var deselectRsp = new DeselectRsp(1, 2, 3);
            var deselectRsp2 = new DeselectRsp(1, 2, 3);
            Assert.IsTrue(deselectRsp.Equals(deselectRsp2));
        }

        [TestMethod]
        public void Should_FailEqualityForNull()
        {
            var deselectRsp = new DeselectRsp(1, 2, 3);
            Assert.IsFalse(deselectRsp.Equals(null));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferetObjectTypes()
        {
            var deselectRsp = new DeselectRsp(1, 2, 3);
            var obj = "test";
            Assert.IsFalse(deselectRsp.Equals(obj));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentMessageTypes()
        {
            var deselectRsp = new DeselectRsp(1, 2, 3);
            var linkTestRsp = new LinkTestRsp(1);
            Assert.IsFalse(deselectRsp.Equals(linkTestRsp));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentDevice()
        {
            var deselectRsp = new DeselectRsp(2, 1, 3);
            var deselectRsp2 = new DeselectRsp(1, 1, 3);
            Assert.IsFalse(deselectRsp.Equals(deselectRsp2));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentContext()
        {
            var deselectRsp = new DeselectRsp(1, 1, 3);
            var deselectRsp2 = new DeselectRsp(1, 2, 3);
            Assert.IsFalse(deselectRsp.Equals(deselectRsp2));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentStatus()
        {
            var deselectRsp = new DeselectRsp(1, 2, 3);
            var deselectRsp2 = new DeselectRsp(1, 2, 4);
            Assert.IsFalse(deselectRsp.Equals(deselectRsp2));
        }



    }

}
