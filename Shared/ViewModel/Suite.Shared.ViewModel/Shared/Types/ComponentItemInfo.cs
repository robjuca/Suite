/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using rr.Library.Helper;
//---------------------------//

namespace Shared.ViewModel
{
  public class TComponentItemInfo
  {
    #region Property
    public Server.Models.Infrastructure.TCategory Category
    {
      get
      {
        return (Model.Category);
      }
    }

    public int ChildCategoryValue
    {
      get;
      private set;
    }

    public Server.Models.Infrastructure.TCategory ChildCategory
    {
      get;
      private set;
    }

    public TComponentModelItem Model
    {
      get;
      set;
    }

    public DataTemplate IconTemplate
    {
      get
      {
        return (TDataTemplateXaml.RequestTemplate (m_IconXmlTemplate));
      }
    }

    public DataTemplate ChildIconTemplate
    {
      get
      {
        return (TDataTemplateXaml.RequestTemplate (m_ChildIconXmlTemplate));
      }
    }

    public string Name
    {
      get
      {
        return (Model.Name);
      }
    }

    public string StyleHorizontal
    {
      get
      {
        return (Model.StyleHorizontal);
      }
    }

    public string StyleVertical
    {
      get
      {
        return (Model.StyleVertical);
      }
    }

    public string SizeString
    {
      get
      {
        return (Model.SizeString);
      }
    }

    public Guid Id
    {
      get
      {
        return (Model.Id);
      }
    }
    #endregion

    #region Constructor
    TComponentItemInfo (TComponentModelItem model)
      : this ()
    {
      CopyFrom (model);
    }

    TComponentItemInfo ()
    {
      Model = TComponentModelItem.CreateDefault;

      ChildCategory = Server.Models.Infrastructure.TCategory.None;
      ChildCategoryValue = Server.Models.Infrastructure.TCategoryType.ToValue (ChildCategory);

      m_IconXmlTemplate = string.Empty;
    }
    #endregion

    #region Members
    public void SelectIconResource ()
    {
      m_IconXmlTemplate = "<ContentControl Style='{DynamicResource ";
      m_IconXmlTemplate += $"Content{Category}Icon";
      m_IconXmlTemplate += "}' />";

      m_ChildIconXmlTemplate = "<ContentControl Style='{DynamicResource ";
      m_ChildIconXmlTemplate += $"Content{ChildCategory}Icon";
      m_ChildIconXmlTemplate += "}' />";
    }

    public void CopyFrom (TComponentModelItem model)
    {
      if (model.NotNull ()) {
        Model.CopyFrom (model);

        if (Model.NodeModelCollection.Count > 0) {
          ChildCategoryValue = Model.NodeModelCollection [0].ChildCategory;
          ChildCategory = Server.Models.Infrastructure.TCategoryType.FromValue (ChildCategoryValue);
        }

        SelectIconResource ();
      }
    }
    #endregion

    #region Fields
    string                                  m_IconXmlTemplate;
    string                                  m_ChildIconXmlTemplate; 
    #endregion

    #region Static
    public static TComponentItemInfo Create (TComponentModelItem model) => new TComponentItemInfo (model);

    public static TComponentItemInfo CreateDefault => new TComponentItemInfo (); 
    #endregion
  };
  //---------------------------//

}  // namespace