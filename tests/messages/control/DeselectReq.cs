using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
using Semi.Hsms.messages.control;

namespace hsms.tests.messages.control
{
    [TestClass]
    public class DeselectReqTests
    {
        [TestMethod]
        public void Should_CreateCorrectDeselectReq()
        {
            var deselectReq = new DeselectReq(1, 2);

            Assert.IsTrue(deselectReq.Type == MessageType.DeselectReq);
            Assert.IsTrue(deselectReq.Device == 1);
            Assert.IsTrue(deselectReq.Context == 2);
        }

        [TestMethod]
        public void Should_SetIsPrimaryToTrue()
        {
            var deselectReq = new DeselectReq(1, 2);

            Assert.IsTrue(deselectReq.IsPrimary == true);
        }

        [TestMethod]
        public void Should_SetIsReplyRequiredToFalse()
        {
            var deselectReq = new DeselectReq(1, 2);

            Assert.IsTrue(deselectReq.IsReplyRequired == true);
        }

        [TestMethod]
        public void Should_BeEqualIfDeselectReqsEqual()
        {
            var deselectReq = new DeselectReq(1, 2);
            var deselectReq2 = new DeselectReq(1, 2);
            Assert.IsTrue(deselectReq.Equals(deselectReq2));
        }

        [TestMethod]
        public void Should_FailEqualityForNull()
        {
            var deselectReq = new DeselectReq(1, 2);
            Assert.IsFalse(deselectReq.Equals(null));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferetObjectTypes()
        {
            var deselectReq = new DeselectReq(1, 2);
            var obj = "test";
            Assert.IsFalse(deselectReq.Equals(obj));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentMessageTypes()
        {
            var deselectReq = new DeselectReq(1, 2);
            var deselectRsp = new DeselectRsp(1, 2, 1);
            Assert.IsFalse(deselectReq.Equals(deselectRsp));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentDevice()
        {
            var deselectReq = new DeselectReq(2, 1);
            var deselectReq2 = new DeselectReq(1, 1);
            Assert.IsFalse(deselectReq.Equals(deselectReq2));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentContext()
        {
            var deselectReq = new DeselectReq(1, 1);
            var deselectReq2 = new DeselectReq(1, 2);
            Assert.IsFalse(deselectReq.Equals(deselectReq2));
        }
    }

}
