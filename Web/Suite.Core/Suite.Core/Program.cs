/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
//---------------------------//

namespace Suite.Core
{
  public class Program
  {
    #region Constructor
    public static void Main (string [] args)
    {
      CreateWebHostBuilder (args).Build ().Run ();
    }
    #endregion

    public static IWebHostBuilder CreateWebHostBuilder (string [] args) => WebHost.CreateDefaultBuilder (args).UseStartup<Startup> ();
  };
  //---------------------------//

}  // namespace