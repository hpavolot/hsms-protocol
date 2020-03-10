using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;
using Semi.Hsms.messages.control;

namespace hsms.tests.messages.control
{
    [TestClass]
    public class SeparateReqTests
    {
        [TestMethod]
        public void Should_CreateCorrectSelectReq()
        {
            var separateReq = new SeparateReq(1, 2);

            Assert.IsTrue(separateReq.Type == MessageType.SeparateReq);
            Assert.IsTrue(separateReq.Device == 1);
            Assert.IsTrue(separateReq.Context == 2);
        }

        [TestMethod]
        public void Should_SetIsPrimaryToTrue()
        {
            var separateReq = new SeparateReq(1, 2);

            Assert.IsTrue(separateReq.IsPrimary == true);
        }

        [TestMethod]
        public void Should_SetIsReplyRequiredToFalse()
        {
            var separateReq = new SeparateReq(1, 2);

            Assert.IsTrue(separateReq.IsReplyRequired == false);
        }


        [TestMethod]
        public void Should_BeEqualIfSparateReqsEqual()
        {
            var separateReq = new SeparateReq(1, 2);
            var separateReq2 = new SeparateReq(1, 2);
            Assert.IsTrue(separateReq.Equals(separateReq2));
        }

        [TestMethod]
        public void Should_FailEqualityForNull()
        {
            var separateReq = new SeparateReq(1, 2);
            Assert.IsFalse(separateReq.Equals(null));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferetObjectTypes()
        {
            var separateReq = new SeparateReq(1, 2);
            var obj = "test";
            Assert.IsFalse(separateReq.Equals(obj));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentMessageTypes()
        {
            var separateReq = new SeparateReq(1, 2);
            var selectRsp = new SelectRsp(1, 2, 1);
            Assert.IsFalse(separateReq.Equals(selectRsp));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentDevice()
        {
            var separateReq = new SeparateReq(2, 1);
            var separateReq2 = new SeparateReq(1, 1);
            Assert.IsFalse(separateReq.Equals(separateReq2));
        }

        [TestMethod]
        public void Should_FailEqualityForDifferentContext()
        {
            var separateReq = new SeparateReq(1, 1);
            var separateReq2 = new SeparateReq(1, 2);
            Assert.IsFalse(separateReq.Equals(separateReq2));
        }
    }

}
