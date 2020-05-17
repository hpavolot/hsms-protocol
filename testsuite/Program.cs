#region Usings
using Semi.Hsms.Messages;
using System;
using System.Security.Cryptography;
using static Semi.Hsms.Messages.Configurator;
#endregion

namespace Semi.Hsms.TestSuite
{
    class Program
    {
        private static RNGCryptoServiceProvider _random = new RNGCryptoServiceProvider();

        #region Class public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                    .IP("127.0.0.1")
                    .Port(11000)
                    .Mode(ConnectionMode.Passive)
                    .T3(5)
                    .Build();

            var connection = new Connection(config);

            connection.T3Timeout += (s, ea) => Console.WriteLine("Message was not delivered");

            connection.Connected += (s, ea) => Console.WriteLine("Connection established");

            byte[] uintBuffer = new byte[sizeof(uint)];

            _random.GetBytes(uintBuffer);
            uint context = BitConverter.ToUInt32(uintBuffer, 0);

            var m = DataMessage
                .Builder
                .Context(context)
                .Device(1)
                .Stream(1)
                .Function(101)
                .Items(new I1(14))
                .Build();


            while (true)
            {
                var cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "start":
                        connection.Start();
                        break;

                    case "send":
                        connection.Send(m);
                        break;

                    case "stop":
                        connection.Stop();
                        break;

                    case "exit":
                        return;


                }
            }

        }


        #endregion
    }
}