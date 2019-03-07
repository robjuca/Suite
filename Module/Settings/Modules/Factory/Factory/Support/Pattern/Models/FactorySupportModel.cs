/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Shared.ViewModel;
//---------------------------//

namespace Module.Settings.Factory.Support.Pattern.Models
{
  public class TFactorySupportModel
  {
    #region Property
    public ObservableCollection<TSupportInfo> SupportInfoCollection
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TFactorySupportModel ()
    {
      SupportInfoCollection = new ObservableCollection<TSupportInfo>
      {
        new TSupportInfo ("SettingsSupportIcon", "ColumnWidth", "(define o tamanho do style mini, min 250, max=600)")
      };
    }
    #endregion

    #region Members
    internal void Select (TComponentModelItem item)
    {
      item.ThrowNull ();

      foreach (var info in SupportInfoCollection) {
        info.Update (item);
      }
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
