/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using rr.Library.Types;

using Server.Models.Component;
//---------------------------//

namespace Shared.Types
{
  public class TPropertyComponentModel : NotificationObject
  {
    #region Properties
    #region Info
    [Category ("1 - Info")]
    [DisplayName ("Enabled")]
    [Description ("Component visibility")]
    public bool EnabledProperty
    {
      get
      {
        return (m_InfoModel.Enabled);
      }

      set
      {
        m_InfoModel.Enabled = value;
        RaisePropertyChanged ("EnabledProperty");
      }
    }

    [Category ("1 - Info")]
    [DisplayName ("Name")]
    [MaxLength (40)]
    [Description ("max length = 40")]
    public string NameProperty
    {
      get
      {
        return (m_InfoModel.Name);
      }

      set
      {
        m_InfoModel.Name = value;
        RaisePropertyChanged ("NameProperty");
      }
    }
    #endregion
    #endregion

    #region Constructor
    TPropertyComponentModel ()
    {
      m_InfoModel = ComponentInfo.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        m_InfoModel.CopyFrom (action.ModelAction.ComponentInfoModel);

        //EnabledProperty = m_InfoModel.Enabled;
        //NameProperty = m_InfoModel.Name;
      }
    }

    public void RequestModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        action.ModelAction.ComponentInfoModel.CopyFrom (m_InfoModel);
      }
    }

    public void Cleanup ()
    {
      m_InfoModel = ComponentInfo.CreateDefault;
    }
    #endregion

    #region Fields
    ComponentInfo                           m_InfoModel;
    #endregion

    #region Static
    public static TPropertyComponentModel CreateDefault => new TPropertyComponentModel ();
    #endregion
  };
  //---------------------------//

}  // namespace