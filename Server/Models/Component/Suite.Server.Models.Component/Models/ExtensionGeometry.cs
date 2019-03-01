/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.DataAnnotations;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionGeometry
  {
    [Key]
    public Guid         Id { get; set; }


    public int          PositionCol { get; set; }
    public int          PositionRow { get; set; }
    public int          SizeCols { get; set; }
    public int          SizeRows { get; set; }
    public string       PositionImage { get; set; }
    public int          PositionIndex { get; set; }
  };
  //---------------------------//

}  // namespace