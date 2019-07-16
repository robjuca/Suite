/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;
//---------------------------//

namespace Shared.Types
{
  public class TImageInfo
  {
    #region Property
    public string Path
    {
      get;
    }

    public TSize Size
    {
      get;
    }

    public string Caption
    {
      get;
    }

    public string Description
    {
      get;
    }
    #endregion

    #region Constructor
    public TImageInfo (Suite.Core.ViewModel.TComponentModelItem item)
    {
      if (item.NotNull ()) {
        Size = TSize.CreateDefault;
        Size.Width = item.ImageModel.Width;
        Size.Height = item.ImageModel.Height;

        Caption = item.TextModel.Caption;
        Description = item.TextModel.Description;

        item.WriteImageToFile ();

        Path = item.ImagePath;
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
