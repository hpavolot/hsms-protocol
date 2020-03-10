using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.Messages;

namespace hsms.tests
{
	[TestClass]
	public class SelectReqTests
	{
		[TestMethod]
		public void Should_CreateCorrectSelectReq()
		{
			var sr = new SelectReq( 1, 2 );

			Assert.IsTrue( sr.Type == MessageType.SelectReq );
			Assert.IsTrue( sr.Device == 1 );
			Assert.IsTrue( sr.Context == 2 );
		}
	}
}
