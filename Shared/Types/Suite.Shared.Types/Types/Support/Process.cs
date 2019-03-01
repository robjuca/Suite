/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.Types
{
  public static class TProcess
  {
    #region Data
    public enum TName
    {
      Document,
      Image,
      Video,
      Bag,
      Shelf,
      Drawer,
      Chest,
      Settings,
    };
    #endregion

    #region Property
    public static string DOCUMENT => TName.Document.ToString ();

    public static string IMAGE => TName.Image.ToString ();

    public static string VIDEO => TName.Video.ToString ();

    public static string BAG => TName.Bag.ToString ();

    public static string SHELF => TName.Shelf.ToString ();

    public static string DRAWER => TName.Drawer.ToString ();

    public static string CHEST => TName.Chest.ToString ();

    public static string SETTINGS => TName.Settings.ToString ();
    #endregion
  };
  //---------------------------//

}  // namespace