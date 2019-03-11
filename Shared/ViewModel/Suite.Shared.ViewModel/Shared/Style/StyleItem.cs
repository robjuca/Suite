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
  public sealed class TStyleItem : TStyleItem<TComponentModelItem>
  {
    #region Constructor
    public TStyleItem (TContentStyle.Mode styleMode, string style)
      : base (styleMode, style)
    {
    }
    #endregion

    #region Overrides
    public override TComponentModelItem GetCurrent ()
    {
      return (SelectedIndex.Equals (-1) ? null : ItemsCollection [SelectedIndex]);
    }

    public override void SelectItem (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        ItemsCollection.Clear ();
        SelectedIndex = -1;

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

            switch (StyleMode) {
              case TContentStyle.Mode.Horizontal:
                modelStyle = item.Value.ExtensionLayoutModel.StyleHorizontal;
                break;

              case TContentStyle.Mode.Vertical:
                modelStyle = item.Value.ExtensionLayoutModel.StyleVertical;
                
                break;
            }

            if (modelStyle.Equals (StyleInfo.StyleString)) {
              var model = Server.Models.Component.TComponentModel.Create (item.Value);

              var modelItem = TComponentModelItem.Create (model);
              modelItem.Select (action.CategoryType.Category);

              ItemsCollection.Add (modelItem);
            }
          }
        }
      }
    }
    #endregion

    #region Property
    public static TStyleItem CreateMini (TContentStyle.Mode styleMode) => new TStyleItem (styleMode, TContentStyle.MINI);
    public static TStyleItem CreateSmall (TContentStyle.Mode styleMode) => new TStyleItem (styleMode, TContentStyle.SMALL);
    public static TStyleItem CreateLarge (TContentStyle.Mode styleMode) => new TStyleItem (styleMode, TContentStyle.LARGE);
    public static TStyleItem CreateBig (TContentStyle.Mode styleMode) => new TStyleItem (styleMode, TContentStyle.BIG);
    public static TStyleItem CreateNone (TContentStyle.Mode styleMode) => new TStyleItem (styleMode, TContentStyle.NONE);
    #endregion
  };
  //---------------------------//

}  // namespace