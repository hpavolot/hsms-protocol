namespace Semi.Hsms.Messages
{
    public class StringItem : DataItem
    {
        #region Class members

        /// <summary>
        /// 
        /// </summary>
        private string _value;

        /// <summary>
        /// 
        /// </summary>
        private int _length;

        #endregion

        #region Class properties

        /// <summary>
        /// 
        /// </summary>
        public string Value => _value;

        /// <summary>
        /// 
        /// </summary>
        public int Length => _length;

        #endregion

        #region Class initializations

        public StringItem(string value, int length)
        {
            _value = value.Substring(0, length);
            _length = length;
            _format = Format.A;
        }

        #endregion

        #region Class public methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{_format.ToString()}] {Value}";
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class A : StringItem
    {
        #region Class initializations

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public A(string v, int len)
            : base(v, len)
        {
        }

        #endregion
    }
}