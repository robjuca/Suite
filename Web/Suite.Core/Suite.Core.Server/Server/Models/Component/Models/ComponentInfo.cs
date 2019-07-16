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
  public partial class ComponentInfo
  {
    [Key]
    public Guid         Id { get; set; }

    public string       Name { get; set; }
    public bool         Enabled { get; set; }
  };
  //---------------------------//

}  // namespace