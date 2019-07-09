/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.Types
{
  //-----TWhere
  public enum TWhere
  {
    Collection,
    Create,
    Edit,
    Factory,
    None,
  };
  //---------------------------//

  //-----TWhat
  public enum TWhat
  {
    Database,
    Model,
    Settings,
    None,
  };
  //---------------------------//

  //-----TModuleName
  public enum TModuleName
  {
    Bag,
    Edit,
    None,
    Services,
    Settings,
    Shell,
    Collection,
    Factory,
  };
  //---------------------------//

  //-----TMessageAction
  public enum TMessageAction
  {
    Authentication,
    Changed,
    Confirm,
    DatabaseLocked,
    DatabaseValidated,
    DatabaseValidating,
    Edit,
    EditEnter,
    EditLeave,
    Error,
    Focus,
    Locked,
    MenuSelected,
    ModalEnter,
    ModalLeave,
    Modify,
    None,
    RefreshProcess,
    Release,
    Reload,
    Remove,
    ReportClear,
    ReportShow,
    Request,
    Response,
    Select,
    SettingsClosed,
    SettingsValidated,
    SettingsValidating,
    Size,
    ShutDown,
    Style,
    Summary,
    Unlocked,
    Update,
  };
  //---------------------------// 

  //----- TInternalMessageAction
  public enum TInternalMessageAction
  {
    Back,
    Change,
    Cleanup,
    Commit,
    Craft,
    DatabaseLocked,
    DatabaseRequest,
    DatabaseResponse,
    DatabaseValidated,
    DatabaseValidating,
    Delete,
    DeployEnter,
    Drop,
    Duplicated,
    Edit,
    EditEnter,
    EditLeave,
    Empty,
    Filter,
    FilterEnter,
    FilterLeave,
    Insert,
    Key,
    LockEnter,
    LockLeave,
    Modify,
    Move,
    NavigateForm,
    None,
    PropertySelect,
    Refresh,
    Reload,
    Remove,
    RemoveEnter,
    Report,
    Request,
    Response,
    Select,
    SettingsValidated,
    SettingsValidating,
    Size,
    Style,
    Summary,
    Update,
  };
  //---------------------------// 

  //----- TNavigateForm
  public enum TNavigateForm
  {
    Back,
    Front,
    None,
  };
  //---------------------------//

  //----- TChild
  public enum TChild
  {
    Back,
    Board,
    Create,
    Design,
    Display,
    Edit,
    Filter,
    Frame,
    Front,
    List,
    None,
    Property,
  };
  //---------------------------//

  //----- TFilterEnabled
  public enum TFilterEnabled
  {
    all,
    enable,
    disable
  };
  //---------------------------//

  //----- TFilterPicture
  public enum TFilterPicture
  {
    all,
    distorted,
    picture,
  };
  //---------------------------//

  //----- TControlMode
  public enum TControlMode
  {
    None,
    Display,
    Design,
  };
  //---------------------------//

  //----- TControlModelMode
  public enum TControlModelMode
  {
    None,
    Local,
    Default,
  };
  //---------------------------//

  //----- TContentStatus
  public enum TContentStatus
  {
    Standby,
    Busy,
  };
  //---------------------------//

  //-----TImagePosition
  public enum TImagePosition
  {
    Left,
    Right,
    Top,
    Bottom,
    Full,
    None,
  };
  //---------------------------//

}  // namespace