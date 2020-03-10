using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
using Semi.Hsms.messages.control;

namespace hsms.tests.messages.control
{
    [TestClass]
    public class SelectReqTests
    {
        [TestMethod]
        public void Should_CreateCorrectSelectReq()
        {
            var selectReq = new SelectReq(1, 2);

            Assert.IsTrue(selectReq.Type == MessageType.SelectReq);
            Assert.IsTrue(selectReq.Device == 1);
            Assert.IsTrue(selectReq.Context == 2);
        }

        [TestMethod]
        public void Should_SetIsPrimaryToTrue()
        {
            var selectReq = new SelectReq(1, 2);

            Assert.IsTrue(selectReq.IsPrimary == true);
        }

        [TestMethod]
        public void Should_SetIsReplyRequiredToFalse()
        {
            var selectReq = new SelectReq(1, 2);

            Assert.IsTrue(selectReq.IsReplyRequired == true);
        }

        [TestMethod]
        public void Should_BeEqualIfSelectReqsEqual()
        {
            var selectReq = new SelectReq(1, 2);
            var selectReq2 = new SelectReq(1, 2);
            Assert.IsTrue(selectReq.Equals(selectReq2));
        }

        [TestMethod]
        public void Should_FailEqualityForNull()
        {
            var selectReq = new SelectReq(1, 2);
            Assert.IsFalse(selectReq.Equals(null));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferetObjectTypes()
        {
            var selectReq = new SelectReq(1, 2);
            var obj = "test";
            Assert.IsFalse(selectReq.Equals(obj));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentMessageTypes()
        {
            var selectReq = new SelectReq(1, 2);
            var selectRsp = new SelectRsp(1, 2, 1);
            Assert.IsFalse(selectReq.Equals(selectRsp));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentDevice()
        {
            var selectReq = new SelectReq(2, 1);
            var selectReq2 = new SelectReq(1, 1);
            Assert.IsFalse(selectReq.Equals(selectReq2));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentContext()
        {
            var selectReq = new SelectReq(1, 1);
            var selectReq2 = new SelectReq(1, 2);
            Assert.IsFalse(selectReq.Equals(selectReq2));
        }
    }

}
