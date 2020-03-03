#region Usings

using Semi.Hsms.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

#endregion

namespace Semi.Hsms.TestSuite
{
    class Program
    {
        #region Class public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var message = DataMessage
                .Builder
                .Stream(1)
                .Function(5)
                .Device(1)
                .Context(522555)
                .DataItems(
                    new I2(32),
                    new U2(2),
                    new A("Lena", 3))
                .Build();
        }

        #endregion
    }
}