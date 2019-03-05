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
    public TStyleItem (string style)
      : base (style)
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
            //TODO :review
            //if (item.Value.ExtensionLayoutModel.Style.Equals (ItemStyleString)) {
            //  var model = Server.Models.Component.TComponentModel.Create (item.Value);

            //  var modelItem = TComponentModelItem.Create (model);
            //  modelItem.Select (action.CategoryType.Category);

            //  ItemsCollection.Add (modelItem);
            //}
          }
        }

        if (HasItems) {
          var item = ItemsCollection [0];

          if (item.NotNull ()) {
            //TODO: review
            //MyStyleString = $"{item.LayoutModel.Style} : {item.LayoutModel.Width} x {item.LayoutModel.Height}";
          }
        }
      }
    }
    #endregion

    #region Property
    public static TStyleItem CreateMini => new TStyleItem (TContentStyle.MINI);
    public static TStyleItem CreateSmall => new TStyleItem (TContentStyle.SMALL);
    public static TStyleItem CreateLarge => new TStyleItem (TContentStyle.LARGE);
    public static TStyleItem CreateBig => new TStyleItem (TContentStyle.BIG);
    public static TStyleItem CreateNone => new TStyleItem (TContentStyle.NONE);
    #endregion
  };
  //---------------------------//

}  // namespace