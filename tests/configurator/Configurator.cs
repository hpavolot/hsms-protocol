#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using static Semi.Hsms.config.Configurator;
#endregion

namespace hsms.tests.configurator
{
	[TestClass]
	public class ConfiguratorTests
	{
		#region IP & Port tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetIpAddress()
		{
			IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.IP(ipAddress);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.IP == ipAddress);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetPort()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.Port(11000);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.Port == 11000);
		}
		#endregion

		#region T3 tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT3VAlueToDefaultIfNotWithinRange()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T3(121);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T3 == ConfigurationBuilder.DEFAULT_T3);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT3IfWithinRAnge()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T3(100);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T3 == 100);
		}
		#endregion

		#region T5 tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT5VAlueToDefaultIfNotWithinRange()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T5(241);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T5 == ConfigurationBuilder.DEFAULT_T5);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT5IfWithinRAnge()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T5(100);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T5 == 100);
		}

		#endregion

		#region T6 tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT6VAlueToDefaultIfNotWithinRange()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T6(241);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T6 == ConfigurationBuilder.DEFAULT_T6);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT6VAlueIfWithinRAnge()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T6(100);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T6 == 100);
		}

		#endregion

		#region T7 tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT7VAlueToDefaultIfNotWithinRange()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T7(241);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T7 == ConfigurationBuilder.DEFAULT_T7);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT7VAlueIfWithinRAnge()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T7(100);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T7 == 100);
		}

		#endregion

		#region T8 tests
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT8VAlueToDefaultIfNotWithinRange()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T8(121);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T8 == ConfigurationBuilder.DEFAULT_T8);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Should_SetT8VAlueIfWithinRange()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.T8(100);

			var configurator = builder.Build();

			Assert.IsTrue(configurator.T8 == 100);
		}

		#endregion

	}
}
