/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionDocument
  {
    #region Constructor
    public ExtensionDocument ()
    {
      Id = Guid.Empty;

      Header = string.Empty;
      Footer = string.Empty;
      Paragraph = string.Empty;

      HeaderVisibility = string.Empty;
      FooterVisibility = string.Empty;

      HtmlHeader = string.Empty;
      HtmlFooter = string.Empty;
      HtmlParagraph = string.Empty;

      ExternalLink = string.Empty;
    }

    public ExtensionDocument (ExtensionDocument alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ExtensionDocument alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Header = alias.Header;
        Footer = alias.Footer;
        Paragraph = alias.Paragraph;
        HeaderVisibility = alias.HeaderVisibility;
        FooterVisibility = alias.FooterVisibility;
        HtmlHeader = alias.HtmlHeader;
        HtmlFooter = alias.HtmlFooter;
        HtmlParagraph = alias.HtmlParagraph;
        ExternalLink = alias.ExternalLink;
      }
    }

    public void Change (ExtensionDocument alias)
    {
      if (alias.NotNull ()) {
        Header = alias.Header;
        Footer = alias.Footer;
        Paragraph = alias.Paragraph;
        HeaderVisibility = alias.HeaderVisibility;
        FooterVisibility = alias.FooterVisibility;
        HtmlHeader = alias.HtmlHeader;
        HtmlFooter = alias.HtmlFooter;
        HtmlParagraph = alias.HtmlParagraph;
        ExternalLink = alias.ExternalLink;
      }
    }
    #endregion

    #region Static
    public static ExtensionDocument CreateDefault => (new ExtensionDocument ());
    #endregion
  };
  //---------------------------//

}  // namespace