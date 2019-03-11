/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using rr.Library.Types;
//---------------------------//

namespace Shared.Types
{
  public class TStylePropertyInfo : NotificationObject
  {
    #region Property
    public Collection<TStylePropertyItem> StyleItemsSource
    {
      get;
      private set;
    }

    public int StyleSelectedIndex
    {
      get
      {
        return (m_SelectedIndex);
      }

      set
      {
        m_SelectedIndex = value;

        RaisePropertyChanged ($"Style{Current.StyleInfo.StyleModeString}Property");
      }
    }

    public TStylePropertyItem Current
    {
      get
      {
        return (StyleItemsSource [m_SelectedIndex]);
      }
    }
    #endregion

    #region Constructor
    public TStylePropertyInfo ()
    {
      Populate (TContentStyle.Mode.None);
    }
    #endregion

    #region Members
    public void Initialize (TContentStyle.Mode styleMode)
    {
      Populate (styleMode);

      Select (TStyleInfo.CreateDefault, false);

      StyleSelectedIndex = StyleItemsSource.IsNull () ? -1 : StyleItemsSource.Count.Equals (0) ? -1 : 0;
    }

    public void Select (TStyleInfo styleInfo, bool locked)
    {
      TStylePropertyItem styleItem = null;

      for (int index = 0; index < StyleItemsSource.Count; index++) {
        styleItem = StyleItemsSource [index];

        if (styleInfo.Contains (styleItem.StyleInfo)) {
          StyleSelectedIndex = index;
          break;
        }
      }

      if (styleItem.NotNull () && locked) {
        StyleItemsSource = new Collection<TStylePropertyItem>
        {
          styleItem
        };

        StyleSelectedIndex = 0;
      }
    }
    #endregion

    #region Overrides
    public override string ToString () => ($"{StyleItemsSource [m_SelectedIndex].StyleInfo.StyleString} : {StyleItemsSource [m_SelectedIndex].SizeString}");
    #endregion

    #region Fields
    int                                     m_SelectedIndex;
    #endregion

    #region Support
    void Populate (TContentStyle.Mode styleMode)
    {
      StyleItemsSource = new Collection<TStylePropertyItem>
      {
        new TStylePropertyItem (styleMode, TContentStyle.Style.mini),
        new TStylePropertyItem (styleMode, TContentStyle.Style.small),
        new TStylePropertyItem (styleMode, TContentStyle.Style.large),
        new TStylePropertyItem (styleMode, TContentStyle.Style.big)
      };

      m_SelectedIndex = -1;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace