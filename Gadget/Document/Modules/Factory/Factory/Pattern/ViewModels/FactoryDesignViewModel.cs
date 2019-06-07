/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Shared.Gadget.Document;

using Gadget.Factory.Presentation;
using Gadget.Factory.Pattern.Models;
//---------------------------//

namespace Gadget.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryDesignViewModel", typeof (IFactoryDesignViewModel))]
  public class TFactoryDesignViewModel : TViewModelAware<TFactoryDesignModel>, IHandleMessageInternal, IFactoryDesignViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryDesignViewModel (IFactoryPresentation presentation)
      : base (new TFactoryDesignModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.Design)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            Model.SelectModel (message.Support.Argument.Args.PropertyName, message.Support.Argument.Types.EntityAction);
            TDispatcher.Invoke (RefreshDesignDispatcher);
          }

          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.BeginInvoke (RequestDesignDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            Model.Cleanup ();
            m_DesignControl.Cleanup ();

            TDispatcher.Invoke (RefreshDesignDispatcher);
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnDesignLoaded (object control)
    {
      if (control is TComponentDesignControl) {
        m_DesignControl = m_DesignControl ?? (TComponentDesignControl) control;
      }
    }
    #endregion

    #region Dispatcher
    void RefreshDesignDispatcher ()
    {
      if (m_DesignControl.NotNull ()) {
        m_DesignControl.RefreshDesign ();
        RaiseChanged ();
      }
    }

    void RequestDesignDispatcher (Server.Models.Component.TEntityAction action)
    {
      var header = m_DesignControl.RequestHeader ();
      var footer = m_DesignControl.RequestFooter ();
      var paragraph = m_DesignControl.RequestParagraph ();

      action.ModelAction.ExtensionDocumentModel.Header = header;
      action.ModelAction.ExtensionDocumentModel.Footer = footer;
      action.ModelAction.ExtensionDocumentModel.Paragraph = paragraph;
      action.ModelAction.ExtensionImageModel.Distorted = m_DesignControl.Model.ImageDistorted;

      // RTF to HTML Converter
      MarkupConverter.IMarkupConverter markupConverter = new MarkupConverter.MarkupConverter ();

      action.ModelAction.ExtensionDocumentModel.HtmlHeader = markupConverter.ConvertRtfToHtml (header);
      action.ModelAction.ExtensionDocumentModel.HtmlFooter = markupConverter.ConvertRtfToHtml (footer);
      action.ModelAction.ExtensionDocumentModel.HtmlParagraph = markupConverter.ConvertRtfToHtml (paragraph);

      // Problem : font-size attribute must be in pixel like fonte-size:12px
      CorrectFontSize (action);

      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Response, TChild.Design, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Property
    IDelegateCommand DelegateCommand
    {
      get
      {
        return (PresentationCommand as IDelegateCommand);
      }
    }
    #endregion

    #region Fields
    TComponentDesignControl                                     m_DesignControl;
    #endregion

    #region Support
    void CorrectFontSize (Server.Models.Component.TEntityAction action)
    {
      // header
      string s = action.ModelAction.ExtensionDocumentModel.HtmlHeader;
      action.ModelAction.ExtensionDocumentModel.HtmlHeader = ReplaceAction (s);

      // footer
      s = action.ModelAction.ExtensionDocumentModel.HtmlFooter;
      action.ModelAction.ExtensionDocumentModel.HtmlFooter = ReplaceAction (s);

      // paragrapf
      s = action.ModelAction.ExtensionDocumentModel.HtmlParagraph;
      action.ModelAction.ExtensionDocumentModel.HtmlParagraph = ReplaceAction (s);
    }

    string ReplaceAction (string html)
    {
      // replace: font-size:??; for font-size:??px;
      string towatch = "font-size:";

      string s = html;
      int index = s.IndexOf (towatch);

      while (index > -1) {
        int pos = index + towatch.Length + 2;
        s = s.Insert (pos, "px");

        index = s.IndexOf (towatch, pos);

        // ;font-size:10px.666666666666666;margin zap this
        var toRemove = pos + 2;
        var ss = s [toRemove];

        while (ss.Equals (';').IsFalse ()) {
          s = s.Remove (toRemove, 1);
          ss = s [toRemove];
        }
      }

      return (s);
    }
    #endregion
  };
  //---------------------------//

}  // namespace