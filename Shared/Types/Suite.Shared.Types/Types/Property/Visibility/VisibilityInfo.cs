/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Types;
//---------------------------//

namespace Shared.Types
{
  public class TVisibilityInfo : NotificationObject
  {
    #region Property
    public bool VisibleChecked
    {
      get
      {
        return (m_IsVisible);
      }

      set
      {
        m_IsVisible = value;
        RaisePropertyChanged ($"{Client} visibility property");
      }
    }

    public bool CollapsedChecked
    {
      get
      {
        return (m_IsCollapsed);
      }

      set
      {
        m_IsCollapsed = value;
        RaisePropertyChanged ($"{Client} visibility property");
      }
    }

    public string Client
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TVisibilityInfo (string client)
    {
      Client = client;

      m_IsVisible = true;
      m_IsCollapsed = false;
    }
    #endregion

    #region Members
    public void Select (string headerVisibility, string footerVisibility)
    {
      switch (Client) {
        case "header":
          VisibleChecked = (headerVisibility.Equals ("visible"));
          CollapsedChecked = (headerVisibility.Equals ("collapsed"));
          break;

        case "footer":
          VisibleChecked = (footerVisibility.Equals ("visible"));
          CollapsedChecked = (footerVisibility.Equals ("collapsed"));
          break;
      }
    }
    #endregion

    #region Overrides
    public override string ToString () => (VisibleChecked ? "visible" : "collapsed");
    #endregion

    #region Fields
    bool                                    m_IsVisible;
    bool                                    m_IsCollapsed;
    #endregion
  };
  //---------------------------//

}  // namespace