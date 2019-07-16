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
  public partial class ComponentStatus
  {
    [Key]
    public Guid         Id { get; set; }

    public bool         Locked { get; set; }
    public bool         Busy { get; set; }
    public bool         Active { get; set; }
  };
  //---------------------------//

}  // namespace