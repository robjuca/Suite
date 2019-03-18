/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using rr.Library.Types;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Module.Settings.Shell.Pattern.Models
{
  public class TShellReportModel
  {
    #region Property
    public TDatabaseAuthentication AuthenticationSQL
    {
      get;
    }

    public TDatabaseAuthentication AuthenticationWindows
    {
      get;
    }

    public TComponentModelItem ComponentModelItem
    {
      get;
    }

    public Collection<TPropertySettingsInfo> PropertySettingsInfoCollection
    {
      get;
    }

    public TAuthentication Authentication
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TShellReportModel ()
    {
      Authentication = TAuthentication.None;

      AuthenticationSQL = TDatabaseAuthentication.CreateDefault;
      AuthenticationWindows = TDatabaseAuthentication.CreateDefault;

      ComponentModelItem = TComponentModelItem.CreateDefault;

      PropertySettingsInfoCollection = new Collection<TPropertySettingsInfo> ();
    }
    #endregion

    #region Members
    internal void Select (TDatabaseAuthentication authentication)
    {
      if (authentication.NotNull ()) {
        switch (authentication.Authentication) {
          case TAuthentication.SQL:
            AuthenticationSQL.CopyFrom (authentication);

            if (authentication.IsActive) {
              Authentication = TAuthentication.SQL;
            }

            break;

          case TAuthentication.Windows:
            AuthenticationWindows.CopyFrom (authentication);

            if (authentication.IsActive) {
              Authentication = TAuthentication.Windows;
            }

            break;
        }
      }
    }

    internal void Select (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        ComponentModelItem.CopyFrom (item);
      }
    }

    internal void Refresh ()
    {
      PropertySettingsInfoCollection.Clear ();

      var contentStyle = TContentStyle.CreateDefault;
      contentStyle.SelectColumnWidth (ComponentModelItem.SettingsModel.ColumnWidth);

      // support
      var propertySettingsInfo = new TPropertySettingsInfo ("SettingsSupportIcon", "ColumnWidth");
      propertySettingsInfo.AddPropertyValue (new TPropertyValueInfo (ComponentModelItem.SettingsModel.ColumnWidth.ToString ()));
      propertySettingsInfo.AddPropertyValue (new TPropertyValueInfo (contentStyle.DashBoardSizeString));

      PropertySettingsInfoCollection.Add (propertySettingsInfo);

      // database
      if (Authentication.NotEquals (TAuthentication.None)) {
        var secondaryIcon = string.Empty;
        var propertyValues = new Collection<TPropertyValueInfo> ();

        switch (Authentication) {
          case TAuthentication.SQL: {
              secondaryIcon = "SQLAuthenticationIcon";

              // values
              // database server:
              propertyValues.Add (new TPropertyValueInfo ("SettingsDatabaseServerIcon", $"database server: {AuthenticationSQL.DatabaseServer}"));

              // database name:
              propertyValues.Add (new TPropertyValueInfo ("DatabaseNameMiniIcon", $"database name: {AuthenticationSQL.DatabaseName}"));

              // UserId
              propertyValues.Add (new TPropertyValueInfo ("SettingsUserIcon", $"user Id: {AuthenticationSQL.UserId}"));
            }
            
            break;

          case TAuthentication.Windows: {
              secondaryIcon = "WindowsAuthenticationIcon";

              // values
              // database server:
              propertyValues.Add (new TPropertyValueInfo ("SettingsDatabaseServerIcon", $"- database server: {AuthenticationWindows.DatabaseServer}"));

              // database name:
              propertyValues.Add (new TPropertyValueInfo ("DatabaseNameMiniIcon", $"- database name: {AuthenticationWindows.DatabaseName}"));
            }

            break;
        }

        propertySettingsInfo = new TPropertySettingsInfo ("SettingsDatabaseIcon", secondaryIcon, "Database");

        foreach (var item in propertyValues) {
          propertySettingsInfo.AddPropertyValue (item);
        }

        PropertySettingsInfoCollection.Add (propertySettingsInfo);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace