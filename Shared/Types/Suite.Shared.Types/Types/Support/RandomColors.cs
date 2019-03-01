/*----------------------------------------------------------------
Copyright (C) 2001 R&R Soft - All rights reserved.
author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Drawing;
using System.Linq;
//---------------------------//

namespace Shared.Types
{
  public static  class TRandomColors
  {
    //public static string RequestColor ()
    //{
    //  string [] colors = new string [] {
    //    "#e0ffe0", "#fff7c7", "#ffcccc", "#c2ffff", "#ebebff", "#efd9c2", "#ffe6ff", "#48d1cc", "#e0f670", "#ff34c7", "#ff00cc", "#c233ff", "#ebebaa", "#ef39c2", "#fde6ff", "#48d1ac",
    //    "#e0ffe2", "#fff5c7", "#ffcc4c", "#c2f3ff", "#ebeb4f", "#efd9ca", "#ffe60f", "#4831cc", "#e0f67a", "#ff84c7", "#ff001c", "#c235ff", "#ebe1aa", "#ef3ac2", "#fde9ff", "#48d9ac",
    //    "#e0ffe3", "#fff767", "#ffc6cc", "#c2ff4f", "#ebebaf", "#efd9cf", "#ffe63f", "#48d5cc", "#e0f6b0", "#ff35c7", "#ff0fcc", "#c2336f", "#ebeb2a", "#ef39b2", "#fde60f", "#48d12c",
    //    "#e0ffe4", "#fff8c7", "#ffc9cc", "#c2ff5f", "#ebe6ff", "#efd9cd", "#ffe5ff", "#48d12c", "#e0f570", "#ff3417", "#ff50cc", "#c233f8", "#ebeba3", "#ef39cc", "#fde6f6", "#48d1a5",
    //  };

    //  Random rand = new Random ();
    //  var index = rand.Next (100, 6300);

    //  for (int i = 0; i < 10; i++) {
    //    index = rand.Next (2000, 66663);
    //  }

    //  index = rand.Next (0, 63); // use this//????????

    //  return (colors [index]);
    //}

    public static string ColorToHtml ()
    {
      return (ColorTranslator.ToHtml (GetRandomColor ()));
    }

    public static Color GetRandomColor ()
    {
      System.Threading.Thread.Sleep (10);

      Random randonGen = new Random (DateTime.Now.Millisecond);

      //Color randomColor = Color.FromArgb (
      //  (byte) randonGen.Next (255),
      //  (byte) randonGen.Next (255),
      //  (byte) randonGen.Next (255),
      //  (byte) randonGen.Next (255)
      //);

      return (Colors [randonGen.Next (Colors.Length)]);
    }

    #region Fields
    static readonly Color [] Colors =
      typeof (Color).GetProperties (System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
      .Select (propInfo => propInfo.GetValue (null, null))
      .Cast<Color> ()
      .ToArray ()
    ; 
    #endregion

  };
  //---------------------------//

}  // namespace