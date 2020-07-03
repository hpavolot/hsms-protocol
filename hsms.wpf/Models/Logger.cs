using Semi.Hsms;
using Semi.Hsms.Messages;
using System;
using System.Linq;
using System.Text;

namespace hsms.wpf.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Logger
    {
        #region  Class members
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler LogLineChanged;
        #endregion

        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public string LogLine { get; private set; } = string.Empty;
        #endregion

        #region Class methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void WriteLine(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(DateTime.Now);
            sb.Append("]  ");
            sb.Append(s);
            sb.Append("\r\n");

            LogLine += sb.ToString();
            LogLineChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="port"></param>
        public void LogEvent(EventType eventType, int port)
        {
            string s = null;

            switch (eventType)
            {
                case EventType.Connected:
                    s = "HSMS CONNECTED : ";
                    break;

                case EventType.Disconnected:
                    s = " HSMS DISCONNECTED : ";
                    break;

                case EventType.Connecting:
                    s = "HSMS CONNECTING ACTIVE : ";
                    break;

                case EventType.Listening:
                    s = "HSMS LISTENING PASSIVE : ";
                    break;
            }

            WriteLine($"{s}{port}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="ea"></param>
        public void LogEvent(EventType eventType, Tuple<byte[], Message> ea)
        {
            StringBuilder sb = new StringBuilder();

            string hex = null; ;
            int len = 0;

            if (eventType == EventType.Sent)
            {
                sb.Append("SEND : ");

                hex = string.Join(" ", ea.Item1.Skip(4).Select(b => b.ToString("X2")));

                var prefix = ea.Item1.Take(4).ToArray();
                Array.Reverse(prefix);
                len = BitConverter.ToInt32(prefix, 0);
            }
            else if (eventType == EventType.Received)
            {
                sb.Append("RECV : ");

                hex = string.Join(" ", ea.Item1.Select(b => b.ToString("X2")));

                len = ea.Item1.Length;
            }

            sb.Append(hex);

            var m = ea.Item2;
            if (m is DataMessage)
            {
                var dm = m as DataMessage;
                sb.Append($" S{dm.Stream}F{dm.Function} ");
            }

            sb.Append($"  [len={len}] ");
            sb.Append($"  SB=[{m.Context}]");
            sb.Append($"  (({m}))");

            WriteLine(sb.ToString());

        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            LogLine = string.Empty;
            LogLineChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
