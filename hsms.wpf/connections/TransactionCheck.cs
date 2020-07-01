using Semi.Hsms.Messages;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Semi.Hsms.connections
{
    public partial class Connection
    {
        /// <summary>
        /// 
        /// </summary>
        private void CheckTransactions()
        {
            lock (_syncObject)
            {
                if (!_bRun)
                    return;

                var now = DateTime.Now;
                var toRemove = new List<Transaction>();

                try
                {
                    foreach (var p in _transactions)
                    {
                        var t = p.Value;
                        var m = t.Message;

                        if (m.Type == MessageType.DataMessage)
                        {
                            if (TimeSpan.FromSeconds(_config.T3) < now - t.Timestamp)
                            {
                                toRemove.Add(t);
                            }
                        }
                    }

                    foreach (var key in toRemove)
                    {
                        _transactions.Remove(key.Message.Context);

                        Events.Add(EventType.T3Timeout, key.Message);
                    }
                }
                finally
                {
                    _timerT36.Change(1000, Timeout.Infinite);
                }
            }
        }
    }
}
