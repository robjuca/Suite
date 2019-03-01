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
        RaisePropertyChanged ("StyleProperty");
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
      Populate ();
    }
    #endregion

    #region Members
    public void Initialize ()
    {
      Populate ();

      Select ("mini", false);

      StyleSelectedIndex = StyleItemsSource.IsNull () ? -1 : StyleItemsSource.Count.Equals (0) ? -1 : 0;
    }

    public void Select (string style, bool locked)
    {
      TStylePropertyItem styleItem = null;

      for (int index = 0; index < StyleItemsSource.Count; index++) {
        styleItem = StyleItemsSource [index];

        if (style.Equals (styleItem.Style)) {
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
    public override string ToString () => ($"{StyleItemsSource [m_SelectedIndex].Style} {StyleItemsSource [m_SelectedIndex].Pixel}");
    #endregion

    #region Fields
    int                                     m_SelectedIndex;
    #endregion

    #region Support
    void Populate ()
    {
      StyleItemsSource = new Collection<TStylePropertyItem>
      {
        new TStylePropertyItem ("mini", "300x116", 300, 116),
        new TStylePropertyItem ("small", "300x232", 300, 232),
        new TStylePropertyItem ("large", "300x348", 300, 348),
        new TStylePropertyItem ("big", "600x348", 600, 348)
      };

      m_SelectedIndex = -1;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace