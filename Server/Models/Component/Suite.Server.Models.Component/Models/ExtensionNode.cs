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
  public partial class ExtensionNode
  {
    [Key]
    public Guid         ChildId { get; set; }

    public Guid         ParentId { get; set; }
    public int          ChildCategory { get; set; }
    public int          ParentCategory { get; set; }
    public string       Position { get; set; }
    public bool         Locked { get; set; }
  };
  //---------------------------//

}  // namespace