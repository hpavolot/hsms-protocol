#region Usings
using System.Collections.Generic;
#endregion

namespace Semi.Hsms.Messages
{
  public class DataList : DataItem
  {
    #region Class properties
    /// <summary>
    /// 
    /// </summary>
    public List<DataItem> Items { get; }
    #endregion

    #region Class initializations
    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    public DataList(params DataItem[] items)
    {
      _format = Format.List;
      Items = new List<DataItem>();

      foreach (var item in items)
      {
        Items.Add(item);
      }
    }
    #endregion

    #region Class public methods
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return $"[{_format.ToString()}] {Items}";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      if (!base.Equals(obj))
        return false;

      if (!(obj is DataList dl))
        return false;

      if (Items.Count != dl.Items.Count)
        return false;

      if (Items.Equals(dl.Items))
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
      hash = hash * 23 + Items.GetHashCode();
      return hash;
    }
    #endregion
  }
}