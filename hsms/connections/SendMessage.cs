#region Usings
using Semi.Hsms.Messages;
using System;
using System.Threading;
#endregion

namespace Semi.Hsms.connections
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Connection
    {
        #region Class 'Send' methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        public void Send(Message m)
        {
            lock (_syncObject)
            {
                if (!_bRun)
                    return;

                _queueToSend.Enqueue(m);
                Monitor.Pulse(_syncObject);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected void MessageProcessor()
        {
            while (true)
            {
                lock (_syncObject)
                {
                    if (_queueToSend.Count != 0)
                    {
                        var m = _queueToSend.Dequeue();

                        CompleteSend(m);
                    }
                    else
                    {
                        Monitor.Wait(_syncObject);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected void CompleteSend(Message m)
        {
            try
            {
                var arr = Coder.Encode(m);

                if (m.IsReplyRequired)
                {
                    _transactions.Add(m.Context, new Transaction(m));
                }

                _socket.Send(arr);

                Events.Add(EventType.Sent, new Tuple<byte[], Message>(arr,m));
            }
            catch
            {
                CloseConnection();
            }
        }
        #endregion
    }
}
