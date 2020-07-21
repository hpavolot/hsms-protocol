#region Usings
using Semi.Hsms.config;
using System;
using System.Windows.Input;
#endregion

namespace hsms.wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class Configuration
    {
        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; } 
        /// <summary>
        /// 
        /// </summary>
        public ConnectionMode Mode { get; set; }

        public Configurator Configurator
        {
            get
            {
                return Configurator
                 .Builder
                 .IP(IP)
                 .Port(Port)
                 .Mode(Mode)
                 .T5(2)
                 .Build();
            }
        }

        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        public Configuration()
        {
            IP = "127.0.0.1";
            Port = 11005;
            Mode = ConnectionMode.Active;
        }

        #endregion
    }
}
