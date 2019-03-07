/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.DataAnnotations;
//---------------------------//

namespace Server.Models.Component
{
  public partial class Settings
  {
    [Key]
    public string       MyName { get; set; }
    public int          ColumnWidth { get; set; }
  }
  //---------------------------//

}  // namespace