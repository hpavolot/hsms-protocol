#region Usings
using System;
#endregion

namespace Semi.Hsms.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Message
    {
        #region Class members
        #endregion

        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public ushort Device { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public uint Context { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public abstract MessageType Type { get; }
        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsReplyRequired { get; }
        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsPrimary { get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Time { get; protected set; }
        #endregion

        #region Class initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="context"></param>
        public Message(ushort device, uint context)
        {
            Device = device;
            Context = context;

            Time = DateTime.Now;
        }
        #endregion

        #region Class public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            var m = obj as Message;

            if (null == m)
                return false;

            if (Device != m.Device)
                return false;

            if (Context != m.Context)
                return false;

            if (Type != m.Type)
                return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + Device.GetHashCode();
            hash = hash * 23 + Context.GetHashCode();
            hash = hash * 23 + Type.GetHashCode();

            return hash;
        }
        #endregion
    }
}