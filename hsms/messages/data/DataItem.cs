#region Usings
#endregion

namespace Semi.Hsms.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataItem
    {
        #region Class members
        /// <summary>
        /// 
        /// </summary>
        protected Format _format;
        #endregion

        #region Class properties
        /// <summary>
        /// 
        /// </summary>
        public Format Type => _format;
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

            if (!(obj is DataItem di))
                return false;

            if (Type != di.Type)
                return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + Type.GetHashCode();

            return hash;
        }
        #endregion
    }
}