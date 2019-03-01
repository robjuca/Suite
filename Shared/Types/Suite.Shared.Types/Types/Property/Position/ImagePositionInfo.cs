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
  public class TImagePositionInfo : NotificationObject
  {
    #region Property
    public Collection<TImagePositionItem> ImagePositionItemsSource
    {
      get;
      private set;
    }

    public int ImagePositionSelectedIndex
    {
      get
      {
        return (m_SelectedIndex);
      }

      set
      {
        m_SelectedIndex = value;
        RaisePropertyChanged ("ImagePositionProperty");
      }
    }

    public TImagePositionItem Current
    {
      get
      {
        return (ImagePositionItemsSource [m_SelectedIndex]);
      }
    }
    #endregion

    #region Constructor
    public TImagePositionInfo ()
    {
      ImagePositionItemsSource = new Collection<TImagePositionItem> ();
      m_SelectedIndex = -1;
    }
    #endregion

    #region Members
    public void SetupCollection (string style)
    {
      ImagePositionItemsSource = new Collection<TImagePositionItem> ();

      switch (Enum.Parse (typeof (TContentStyle.Style), style)) {
        case TContentStyle.Style.mini:
          ImagePositionItemsSource.Add (new TImagePositionItem ("left", "100x116", 100, 116));
          ImagePositionItemsSource.Add (new TImagePositionItem ("right", "100x116", 100, 116));
          ImagePositionItemsSource.Add (new TImagePositionItem ("full", "300x116", 300, 116));
          ImagePositionItemsSource.Add (new TImagePositionItem ("none"));
          break;

        case TContentStyle.Style.small:
          ImagePositionItemsSource.Add (new TImagePositionItem ("left", "130x232", 130, 232));
          ImagePositionItemsSource.Add (new TImagePositionItem ("right", "130x232", 130, 232));
          ImagePositionItemsSource.Add (new TImagePositionItem ("full", "300x232", 300, 232));
          ImagePositionItemsSource.Add (new TImagePositionItem ("none"));
          break;

        case TContentStyle.Style.large:
          ImagePositionItemsSource.Add (new TImagePositionItem ("top", "300x160", 300, 160));
          ImagePositionItemsSource.Add (new TImagePositionItem ("bottom", "300x160", 300, 160));
          ImagePositionItemsSource.Add (new TImagePositionItem ("full", "300x348", 300, 348));
          ImagePositionItemsSource.Add (new TImagePositionItem ("none"));
          break;

        case TContentStyle.Style.big:
          ImagePositionItemsSource.Add (new TImagePositionItem ("left", "300x348", 300, 348));
          ImagePositionItemsSource.Add (new TImagePositionItem ("right", "300x348", 300, 348));
          ImagePositionItemsSource.Add (new TImagePositionItem ("full", "600x348", 600, 348));
          ImagePositionItemsSource.Add (new TImagePositionItem ("none"));
          break;
      }

      m_SelectedIndex = 0;
    }

    public void Select (string imagePosition)
    {
      for (int index = 0; index < ImagePositionItemsSource.Count; index++) {
        var position = ImagePositionItemsSource [index].Position;

        if (imagePosition.Equals (position)) {
          ImagePositionSelectedIndex = index;
          break;
        }
      }
    }
    #endregion

    #region Overrides
    public override string ToString () => ($"{ImagePositionItemsSource [m_SelectedIndex].Position} {ImagePositionItemsSource [m_SelectedIndex].Pixel}");
    #endregion

    #region Fields
    int                                     m_SelectedIndex;
    #endregion
  };
  //---------------------------//

}  // namespace