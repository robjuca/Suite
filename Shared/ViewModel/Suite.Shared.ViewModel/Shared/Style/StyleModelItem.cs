/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Linq;

using Server.Models.Infrastructure;

using Shared.Types;
//---------------------------//

namespace Shared.ViewModel
{
  public sealed class TStyleModelItem : TStyleItem
  {
    #region Constructor
    public TStyleModelItem (TContentStyle.Mode styleMode, string style)
      : base (styleMode, style)
    {
    }
    #endregion

    #region Overrides
    // TODO: serve para que????
    public override void SelectItem (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        var categoryValue = TCategoryType.ToValue (action.CategoryType.Category);

        // Extension (CategoryRelationCollection)
        var categoryRelationList = action.CollectionAction.CategoryRelationCollection
          .Where (p => p.Category.Equals (categoryValue))
          .ToList ()
        ;

        if (categoryRelationList.Count.Equals (1)) {
          var categoryRelation = categoryRelationList [0]; // get extension using TComponentExtension

          var extension = TComponentExtension.Create (categoryRelation.Extension);
          extension.Request ();

          foreach (var item in action.CollectionAction.ModelCollection) {
            var modelStyle = TContentStyle.NONE;

            switch (StyleInfo.StyleMode) {
              case TContentStyle.Mode.Horizontal:
                modelStyle = item.Value.ExtensionLayoutModel.StyleHorizontal;
                break;

              case TContentStyle.Mode.Vertical:
                modelStyle = item.Value.ExtensionLayoutModel.StyleVertical;
                break;
            }

            if (modelStyle.Equals (StyleInfo.StyleString)) {
            }
          }
        }
      }
    }
    #endregion

    #region Property
    public static TStyleModelItem CreateMini (TContentStyle.Mode styleMode) => new TStyleModelItem (styleMode, TContentStyle.MINI);
    public static TStyleModelItem CreateSmall (TContentStyle.Mode styleMode) => new TStyleModelItem (styleMode, TContentStyle.SMALL);
    public static TStyleModelItem CreateLarge (TContentStyle.Mode styleMode) => new TStyleModelItem (styleMode, TContentStyle.LARGE);
    public static TStyleModelItem CreateBig (TContentStyle.Mode styleMode) => new TStyleModelItem (styleMode, TContentStyle.BIG);
    public static TStyleModelItem CreateNone (TContentStyle.Mode styleMode) => new TStyleModelItem (styleMode, TContentStyle.NONE);
    #endregion
  };
  //---------------------------//

}  // namespace