#region Usings
using Semi.Hsms.Messages;
using System.Collections.Generic;
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
      var items = new List<DataItem>();
      items.Add(new I2(32));
      items.Add(new U2(2));
      items.Add(new A("lena", 10));
      items.Add(new A("denis", 3));
      items.Add(new DataList(
        new A("mama", 5),
        new DataList(
          new F8(10.56),
          new A("test", 12))));

      var message = DataMessage
        .Builder
        .Stream(1)
        .Function(5)
        .Context(522555)
        .Items(items)
        .Build();

      var copy = message.Items as List<DataItem>;
    }
    #endregion
  }
}