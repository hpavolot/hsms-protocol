#region Usings
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Semi.Hsms.Messages
{
  /// <summary>
  /// 
  /// </summary>
  public class ListItem : DataItem
  {
    #region Class members
    /// <summary>
    /// 
    /// </summary>
    private List<DataItem> _items;
    #endregion

    #region Class properties
    /// <summary>
    /// 
    /// </summary>
    public IList<DataItem> Items => _items.ToList();
    #endregion

    #region Class initializations
    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    public ListItem(params DataItem[] items)
    {
      _format = Format.List;
      _items = new List<DataItem>();

      _items.AddRange( items );
    }
    #endregion

    #region Class public methods
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return $"L<{Items.Count}>";
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

      if (!(obj is ListItem dl))
        return false;

      if (Items.Count != dl.Items.Count)
        return false;

      for( int i = 0, len = _items.Count; i < len; ++i )
      {
        if( !dl._items [ i ].Equals( _items [ i ] ) )
          return false;
      }

      return true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
      int hash = base.GetHashCode();

      _items.ForEach( x => hash = hash * 23 + x.GetHashCode() );

      return hash;
    }
    #endregion
  }
}