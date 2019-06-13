/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
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
    public void SetupCollection (TStyleInfo styleHorizontalInfo, TStyleInfo styleVerticalInfo)
    {
      ImagePositionItemsSource = new Collection<TImagePositionItem>
      {
        new TImagePositionItem (styleHorizontalInfo, styleVerticalInfo, TImagePosition.None),
        new TImagePositionItem (styleHorizontalInfo, styleVerticalInfo, TImagePosition.Left),
        new TImagePositionItem (styleHorizontalInfo, styleVerticalInfo, TImagePosition.Right),
        new TImagePositionItem (styleHorizontalInfo, styleVerticalInfo, TImagePosition.Top),
        new TImagePositionItem (styleHorizontalInfo, styleVerticalInfo, TImagePosition.Bottom),
        new TImagePositionItem (styleHorizontalInfo, styleVerticalInfo, TImagePosition.Full),
      };

      m_SelectedIndex = 0;
    }

    public void Select (string imagePosition)
    {
      for (int index = 0; index < ImagePositionItemsSource.Count; index++) {
        var position = ImagePositionItemsSource [index].Position.ToString ();

        if (imagePosition.Equals (position)) {
          ImagePositionSelectedIndex = index;
          break;
        }
      }
    }
    #endregion

    #region Overrides
    public override string ToString () => ($"{ImagePositionItemsSource [m_SelectedIndex].Position} {ImagePositionItemsSource [m_SelectedIndex].SizeString}");
    #endregion

    #region Fields
    int                                     m_SelectedIndex;
    #endregion
  };
  //---------------------------//

}  // namespace