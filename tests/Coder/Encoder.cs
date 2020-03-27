#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Semi.Hsms.messages.data;
using Semi.Hsms.Messages;
using System;
using System.Linq;
#endregion

namespace hsms.tests
{
	[TestClass]
	public class EncoderTests
	{
		/// <summary>
		/// 
		/// </summary>
		#region Control Messages tests

		[TestMethod]
		public void Should_EncodeSelectReq()
		{
			var selectReq = new SelectReq(1, 9);

			var actual = Coder.Encode(selectReq).Skip(4).ToArray();
			var expected = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x09 };

			CollectionAssert.AreEqual(expected, actual);
		}
		#endregion

		#region Numeric Data Item tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_I1()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(9)
					.Stream(1)
					.Function(3)
					.Items(new I1(14))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x01, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09,
				0x65, 0x01, 0x0E};

			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_I2()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(17)
					.Stream(1)
					.Function(5)
					.Items(new I2(7))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x01, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11,
				0x69, 0x02, 0x00, 0x07};

			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_I4()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(18)
					.Stream(1)
					.Function(7)
					.Items(new I4(74))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x01, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12,
				0x71, 0x04, 0x00, 0x00, 0x00, 0x4A};

			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_I8()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(20)
					.Stream(3)
					.Function(5)
					.Items(new I8(300))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x03, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14,
				0x61, 0x08, 0x00,0x00,0x00,0x00,0x00,0x00, 0x01, 0x2C};

			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_U1()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(21)
					.Stream(9)
					.Function(9)
					.Items(new U1(255))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x09, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x15,
				0xA5, 0x01, 0xFF};

			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_U2()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(21)
					.Stream(7)
					.Function(9)
					.Items(new U2(1200))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x07, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x15,
				0xA9, 0x02, 0x04, 0xB0};

			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_U4()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(21)
					.Stream(3)
					.Function(5)
					.Items(new U4(229))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x03, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x15,
				0xB1, 0x04, 0x00,0x00, 0x00, 0xE5};

			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_U8()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(21)
					.Stream(5)
					.Function(7)
					.Items(new U8(500))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x05, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x15,
				0xA1, 0x08, 0x00,0x00, 0x00,0x00,0x00,0x00, 0x01, 0xF4};


			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_F4()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(21)
					.Stream(9)
					.Function(9)
					.Items(new F4(25.567f))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x09, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x15,
				0x91, 0x04, 0x41, 0xCC, 0x89, 0x37};

			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeNumeric_F8()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(21)
					.Stream(9)
					.Function(9)
					.Items(new F8(1234561))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x09, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x15,
				0x81, 0x08, 0x41, 0x32, 0xD6, 0x81, 0x00,0x00,0x00,0x00 };

			CollectionAssert.AreEqual(expected, actual);
		}

		#endregion

		#region String Data Item tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeStringItemWithSmallLength()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(9)
					.Stream(1)
					.Function(3)
					.Items(new A("lena", 4))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x01, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09,
				0x41, 0x04, 0x6C, 0x65, 0x6E, 0x61};

			CollectionAssert.AreEqual(expected, actual);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeStringItemWithMediumLength()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(9)
					.Stream(1)
					.Function(3)
					.Items(new A("test", 472))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();

			var length = new byte[3];

			Array.ConstrainedCopy(actual, 10, length, 0, 3);

			var expected = new byte[] { 0x42, 0x01, 0xD8 };

			CollectionAssert.AreEqual(expected, length);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeStringItemWithBigLength()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(9)
					.Stream(1)
					.Function(3)
					.Items(new A("test", 66000))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();

			var length = new byte[4];

			Array.ConstrainedCopy(actual, 10, length, 0, 4);

			var expected = new byte[] { 0x43, 0x01, 0x01, 0xd0 };

			CollectionAssert.AreEqual(expected, length);
		}
		#endregion

		#region Boolean Data Item tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeBooleanItem()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(9)
					.Stream(1)
					.Function(3)
					.Items(new Bool(true))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{ 0x00, 0x01, 0x01, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09,
				0x25, 0x01, 0x01};

			CollectionAssert.AreEqual(expected, actual);
		}
		#endregion

		#region Lists tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_EncodeOneListWith_String_Bool_I4_Items()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(9)
					.Stream(1)
					.Function(3)
					.Items(new ListItem(
						new A("test", 4),
						new Bool(true),
						new I4(320)))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{   0x00, 0x01, 0x01, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09,
				0x01, 0x03,
				0x41, 0x04, 0x74, 0x65, 0x73, 0x74,
				0x25, 0x01, 0x01,
				0x71, 0x04, 0x00,0x00,0x01, 0x40
			};

			CollectionAssert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Should_EncodeOneListWith_List_F4_U2_Items()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(9)
					.Stream(1)
					.Function(3)
					.Items(new ListItem(
						new ListItem(
						new F4(37.7878f),
						new U2(89))))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{   0x00, 0x01, 0x01, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09,
				0x01, 0x01,
				0x01, 0x02,
				0x91, 0x04, 0x42, 0x17, 0x26, 0xB5,
				0xA9, 0x02, 0x00, 0x59,
			};

			CollectionAssert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void Should_EncodeListOfMixedItems()
		{
			var m = DataMessage
				.Builder
					.Device(1)
					.Context(9)
					.Stream(1)
					.Function(3)
					.Items(
				new A("hello", 5),
				new ListItem(
						new ListItem(
							new F4(37.7878f),
							new U2(89)),
						new U1(255),
						new ListItem(
							new A("test", 4),
							new Bool(true),
							new I4(320)),
						new F8(1234561)))
					.Build();

			var actual = Coder.Encode(m).Skip(4).ToArray();
			var expected = new byte[]
			{   0x00, 0x01, 0x01, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09,
				0x41, 0x05, 0x68, 0x65, 0x6C, 0x6C, 0x6F,
				0x01, 0x04,
				0x01, 0x02,
				0x91, 0x04, 0x42, 0x17, 0x26, 0xB5,
				0xA9, 0x02, 0x00, 0x59,
				0xA5, 0x01, 0xFF,
				0x01, 0x03,
				0x41, 0x04, 0x74, 0x65, 0x73, 0x74,
				0x25, 0x01, 0x01,
				0x71, 0x04, 0x00,0x00,0x01, 0x40,
				0x81, 0x08, 0x41, 0x32, 0xD6, 0x81, 0x00,0x00,0x00,0x00
			};
			CollectionAssert.AreEqual(expected, actual);
		}
		#endregion

	}
}