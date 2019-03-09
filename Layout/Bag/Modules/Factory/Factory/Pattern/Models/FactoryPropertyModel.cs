/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel;

using rr.Library.Types;

using Server.Models.Component;

using Shared.Types;
//---------------------------//

namespace Module.Factory.Pattern.Models
{
  public class TFactoryPropertyModel : NotificationObject
  {
    #region Property
    public TModelProperty ComponentModelProperty
    {
      get;
      set;
    }

    public bool IsEnabledView
    {
      get;
      set;
    }

    public Guid Id
    {
      get
      {
        return (ComponentModelProperty.Id);
      }
    }
    #endregion

    #region Constructor
    public TFactoryPropertyModel ()
    {
      ComponentModelProperty = TModelProperty.Create (Server.Models.Infrastructure.TCategory.Bag);
      ComponentModelProperty.PropertyChanged += OnPropertyModelChanged;

      IsEnabledView = true;
    }
    #endregion

    #region Members
    internal void Initialize ()
    {
      ComponentModelProperty.Initialize ();
    }

    internal void ShowPanels ()
    {
      ComponentModelProperty.ShowPanels ();
    }

    internal void Cleanup ()
    {
      ComponentModelProperty.Cleanup ();
    }

    internal void RequestModel (TEntityAction action)
    {
      action.ThrowNull ();

      ComponentModelProperty.RequestModel (action);
    }

    internal void SelectModel (TEntityAction action)
    {
      action.ThrowNull ();

      ComponentModelProperty.SelectModel (action);
    }

    internal void LockEnter ()
    {
      IsEnabledView = false;
    }

    internal void LockLeave ()
    {
      IsEnabledView = true;
    }
    #endregion

    #region Event
    void OnPropertyModelChanged (object sender, PropertyChangedEventArgs e)
    {
      RaisePropertyChanged (e.PropertyName);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
