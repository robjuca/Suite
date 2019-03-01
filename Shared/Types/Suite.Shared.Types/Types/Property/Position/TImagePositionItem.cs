/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.Types
{
  public class TImagePositionItem
  {
    #region Property
    public string Position
    {
      get;
      set;
    }

    public string Pixel
    {
      get;
      set;
    }

    public int Width
    {
      get;
      set;
    }

    public int Height
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TImagePositionItem (string position, string pixel, int width, int height)
    {
      Position = position;
      Pixel = pixel;
      Width = width;
      Height = height;
    }

    public TImagePositionItem (string position)
    {
      Position = position;
      Pixel = "0x0";
      Width = 0;
      Height = 0;
    }
    #endregion
  };
  //---------------------------//

}  // namespace