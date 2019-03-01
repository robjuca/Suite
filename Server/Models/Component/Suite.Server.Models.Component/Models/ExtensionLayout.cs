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
  public partial class ExtensionLayout
  {
    [Key]
    public Guid         Id { get; set; }

    public string       Style { get; set; }
    public int          Width { get; set; }
    public int          Height { get; set; }
  };
  //---------------------------//

}  // namespace