/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.Types
{
  public class TStylePropertyItem
  {
    #region Property
    public string Style
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
    public TStylePropertyItem (string style, string pixel, int width, int height)
    {
      Style = style;
      Pixel = pixel;
      Width = width;
      Height = height;
    }
    #endregion
  };
  //---------------------------//

}  // namespace