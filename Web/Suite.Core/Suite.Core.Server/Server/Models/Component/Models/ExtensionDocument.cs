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
  public partial class ExtensionDocument
  {
    [Key]
    public Guid         Id { get; set; }

    public string       Header { get; set; }
    public string       Footer { get; set; }
    public string       Paragraph { get; set; }
    public string       HeaderVisibility { get; set; }
    public string       FooterVisibility { get; set; }
    public string       HtmlHeader { get; set; }
    public string       HtmlFooter { get; set; }
    public string       HtmlParagraph { get; set; }
    public string       ExternalLink { get; set; }
  }
  //---------------------------//

}  // namespace