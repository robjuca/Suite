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
  public partial class TModelContext : DbContext, Server.Models.Infrastructure.IModelContext
  {
    #region Property
    #region Settings
    public virtual DbSet<Settings> Settings
    {
      get; set;
    }
    #endregion

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

    public static string ConnectionString
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TModelContext ()
    {
    }

    public TModelContext (string connectionString)
    {
      ConnectionString = connectionString;
    }
    #endregion

    #region Interface
    void Server.Models.Infrastructure.IModelContext.DisposeNow ()
    {
      Dispose ();
    }
    #endregion

    #region Overrides
    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured) {
        //ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=Suite18;Trusted_Connection=True;";///////////////////migration

        optionsBuilder.UseSqlServer (ConnectionString);
      }
    }
    #endregion

    #region Property
    public static TModelContext CastTo (Server.Models.Infrastructure.IModelContext modelContext) => (modelContext as TModelContext);
    #endregion
  };
  //---------------------------//

}  // namespace