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
  public partial class ComponentRelation
  {
    [Key]
    public Guid         ChildId { get; set; }

    public Guid         ParentId { get; set; }
    public int          ChildCategory { get; set; }
    public int          ParentCategory { get; set; }
    public bool         Locked { get; set; }
    public int          PositionColumn { get; set; }
    public int          PositionRow { get; set; }
    public int          PositionIndex { get; set; }
  };
  //---------------------------//

}  // namespace