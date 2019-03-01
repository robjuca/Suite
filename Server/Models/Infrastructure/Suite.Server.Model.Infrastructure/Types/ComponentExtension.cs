/*--------------------- -------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TComponentExtension
  {
    #region Property
    public Collection<TComponentExtensionName> ExtensionList
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TComponentExtension (int extension)
      : this ()
    {
      Extension = extension;
    }

    TComponentExtension ()
    {
      Extension = 0;
      ExtensionList = new Collection<TComponentExtensionName> ();
    }
    #endregion

    #region Members
    public void Request ()
    {
      ExtensionList.Clear ();

      foreach (short extensionNameValue in Enum.GetValues (typeof (TComponentExtensionName))) {
        if ((extensionNameValue & Extension) != 0) {
          ExtensionList.Add ((TComponentExtensionName) extensionNameValue);
        }
      }
    }

    public void CopyFrom (TComponentExtension alias)
    {
      if (alias.NotNull ()) {
        Extension = alias.Extension;
        ExtensionList = new Collection<TComponentExtensionName> (alias.ExtensionList);
      }
    }
    #endregion

    #region Property
    int Extension
    {
      get;
      set;
    } 
    #endregion

    #region Static
    public static TComponentExtension Create (int extension) => new TComponentExtension (extension);

    public static TComponentExtension CreateDefault => new TComponentExtension ();
    #endregion
  };
  //---------------------------//

}  // namespace