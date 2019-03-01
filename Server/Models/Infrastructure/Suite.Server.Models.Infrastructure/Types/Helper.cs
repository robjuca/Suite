/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Helper;
//---------------------------//

namespace Server.Models.Infrastructure
{
  //----- THelper
  public static class THelper
  {
    public static void FormatException (string tag, Exception exception, IEntityAction entityAction)
    {
      string msg = $"[{entityAction.Operation.CategoryType.Category} {tag} - {entityAction.Operation.Operation}] {exception.Message}";

      if (exception.InnerException != null) {
        msg = $"[component: {entityAction.Operation.CategoryType} {tag} - operation: {entityAction.Operation.Operation}] {exception.Message}{Environment.NewLine}{exception.InnerException.Message}";
      }

      entityAction.Result.CopyFrom (new TValidationResult (msg));
    }

    public static void FormatExtensionMustExistException (IEntityAction entityAction)
    {
      string msg = $"[component: {entityAction.Operation.CategoryType.Category} - operation: {entityAction.Operation.Operation}] - extension: Extension MUST Exist!";

      entityAction.Result.CopyFrom (new TValidationResult (msg));
    }

    public static void FormatExtensionNotImplementedException (IEntityAction entityAction)
    {
      string msg = $"[component: {entityAction.Operation.CategoryType.Category} - operation: {entityAction.Operation.Operation}] - extension: {entityAction.Operation.Extension.ToString ()} Extension NOT implemented!";

      entityAction.Result.CopyFrom (new TValidationResult (msg));
    }
  };
  //---------------------------//

}  // namespace