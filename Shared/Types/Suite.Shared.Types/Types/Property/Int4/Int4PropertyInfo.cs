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
  public class TInt4PropertyInfo : NotificationObject
  {
    #region Property
    public Collection<TInt4Item> Int4ItemsSource
    {
      get;
      private set;
    }

    public int MaxInt4
    {
      get;
    }

    public int Int4SelectedIndex
    {
      get
      {
        return (m_SelectedIndex);
      }

      set
      {
        m_SelectedIndex = value;

        if ((m_SelectedIndex > -1) && m_Blocked.IsFalse ()) {
          RaisePropertyChanged ("Int4Property");
        }
      }
    }

    public TInt4Item Int4
    {
      get
      {
        return (m_SelectedIndex.Equals (-1) ? TInt4Item.CreateDefault : Int4ItemsSource [m_SelectedIndex]);
      }
    }
    #endregion

    #region Constructor
    TInt4PropertyInfo (int maxInt4)
    {
      MaxInt4 = maxInt4;

      Populate ();

      m_Locked = false;
      m_Unlocked = false;
      m_Blocked = false;
    }
    #endregion

    #region Members
    public void Initialize ()
    {
      Int4SelectedIndex = (Int4ItemsSource.Count - 1);
    }

    public void Select (int int4Value)
    {
      for (int index = 0; index < Int4ItemsSource.Count; index++) {
        var int4Item = Int4ItemsSource [index];

        if (int4Item.Contains (int4Value)) {
          Int4SelectedIndex = index;
          break;
        }
      }
    }

    public void Lock ()
    {
      m_Blocked = true;

      if (m_Locked.IsFalse ()) {
        var current = new TInt4Item (Int4);

        Int4SelectedIndex = -1;
        Int4ItemsSource.Clear ();
        Int4ItemsSource.Add (current);

        Initialize ();

        m_Locked = true;
        m_Unlocked = false;
      }

      m_Blocked = false;
    }

    public void Unlock ()
    {
      m_Blocked = true;

      if (m_Unlocked.IsFalse ()) {
        var current = new TInt4Item (Int4);

        Populate ();
        Initialize ();

        for (int index = 0; index < Int4ItemsSource.Count; index++) {
          var int4Item = Int4ItemsSource [index];

          if (int4Item.Contains (current)) {
            Int4SelectedIndex = index;
            break;
          }
        }

        m_Locked = false;
        m_Unlocked = true;
      }

      m_Blocked = false;
    }
    #endregion

    #region Overrides
    public override string ToString () => (Int4.Int4String);
    #endregion

    #region Fields
    bool                                    m_Locked;
    bool                                    m_Unlocked;
    bool                                    m_Blocked;
    int                                     m_SelectedIndex;
    #endregion

    #region Static
    public static TInt4PropertyInfo Create (int maxInt) => new TInt4PropertyInfo (maxInt);
    #endregion

    #region Support
    void Populate ()
    {
      Int4SelectedIndex = -1;
      Int4ItemsSource = new Collection<TInt4Item> ();

      for (int i = 1; i <= MaxInt4; i++) {
        Int4ItemsSource.Add (new TInt4Item (i.ToString (), i));
      }
    }
    #endregion
  }
  //---------------------------//

}  // namespace