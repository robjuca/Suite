/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//---------------------------//

namespace Suite.Core
{
  public class Startup
  {
    #region Property
    public IConfiguration Configuration
    {
      get;
    }
    #endregion

    #region Constructor
    public Startup (IConfiguration configuration)
    {
      Configuration = configuration;
    }
    #endregion


    #region members
    public void ConfigureServices (IServiceCollection services)
    {
#if LOCAL
      var connection = @"Data Source=.\SQLEXPRESS;Initial Catalog=Suite;Integrated Security=True;";
#else
      var connection = "Data Source=mssql7.websitelive.net;Initial Catalog=robjuca_Suite18;Persist Security Info=True;User ID=robjuca_sqldb;Password=ShallyCherry12@";
#endif

      services
        .AddDbContext<Server.Context.Component.TModelContext> (options => options.UseSqlServer (connection)
      );

      // This method gets called by the runtime. Use this method to add services to the container.

      services.Configure<CookiePolicyOptions> (options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });


      services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
    }

    public void Configure (IApplicationBuilder app, IHostingEnvironment env)
    {
      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

      if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
      }

      else {
        app.UseExceptionHandler ("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts ();
      }

      app.UseHttpsRedirection ();
      app.UseStaticFiles ();
      app.UseCookiePolicy ();

      app.UseMvc (routes =>
      {
        routes.MapRoute (
                 name: "default",
                 template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
    #endregion
  };
  //---------------------------//

}  // namespace