/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Microsoft.EntityFrameworkCore;

using Server.Models.Component;
//---------------------------//

namespace Server.Context.Component
{
  public partial class TModelContext : DbContext
  {
    #region Property
    #region Category
    public virtual DbSet<CategoryRelation> CategoryRelation
    {
      get; set;
    }
    #endregion

    #region Component
    public virtual DbSet<ComponentDescriptor> ComponentDescriptor
    {
      get; set;
    }

    public virtual DbSet<ComponentInfo> ComponentInfo
    {
      get; set;
    }

    public virtual DbSet<ComponentStatus> ComponentStatus
    {
      get; set;
    }

    public virtual DbSet<ComponentRelation> ComponentRelation
    {
      get; set;
    }
    #endregion

    #region Extension
    public virtual DbSet<ExtensionDocument> ExtensionDocument
    {
      get; set;
    }

    public virtual DbSet<ExtensionGeometry> ExtensionGeometry
    {
      get; set;
    }

    public virtual DbSet<ExtensionImage> ExtensionImage
    {
      get; set;
    }

    public virtual DbSet<ExtensionLayout> ExtensionLayout
    {
      get; set;
    }

    public virtual DbSet<ExtensionNode> ExtensionNode
    {
      get; set;
    }

    public virtual DbSet<ExtensionText> ExtensionText
    {
      get; set;
    }
    #endregion
    #endregion

    #region Constructor
    public TModelContext (DbContextOptions<TModelContext> options)
      : base (options)
    {
    }
    #endregion
  };
  //---------------------------//

}  // namespace