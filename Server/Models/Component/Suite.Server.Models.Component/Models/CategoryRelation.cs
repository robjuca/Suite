/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.DataAnnotations;
//---------------------------//

namespace Server.Models.Component
{
  public partial class CategoryRelation
  {
    [Key]
    public int         Category { get; set; }

    public int         Extension { get; set; }
  };
  //---------------------------//

}  // namespace