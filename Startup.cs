using Hangfire;
using Hangfire.HttpJob;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Hangfire.HttpJob.Agent;


namespace test
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      //Configuration = configuration;
      GlobalConfiguration.Configuration.UseSqlServerStorage("connect string");
    }


    public void Configuration(IGlobalConfiguration globalConfiguration)
    {
      globalConfiguration.UseSqlServerStorage("connect string").UseHangfireHttpJob();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });


      //Hangfire
      //services.AddHangfire(x => x.UseSqlServerStorage("Data Source=HKCC-WITITW01\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;"));
      services.AddHangfire(Configuration);
      services.AddHangfireServer();
      //Hangfire
      //HttpJob
      services.AddHangfireHttpJobAgent();
      //HttpJob
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      

    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }
      var hangfireStartUpPath = "/job";
      app.UseHangfireDashboard(hangfireStartUpPath,new DashboardOptions {
        AppPath = "#",
        DisplayStorageConnectionString = false,
        IsReadOnlyFunc = Context => false,
      });
      //Hangfire
      //Scheduled jobs
      //BackgroundJob.Schedule(() => ScheduleJob(), TimeSpan.FromSeconds(10));
      //Fire and forget
      //BackgroundJob.Enqueue(() => FireAndForget());
      //Recurring jobs
      //RecurringJob.AddOrUpdate(() => Recurring(), Cron.Minutely);
      //RecurringJob.AddOrUpdate("id2",() => Recurring_job(), Cron.Minutely);
  
      app.UseHangfireServer();
      app.UseHangfireDashboard("/hangfire");

      app.UseHangfireHttpJobAgent();




      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });



    }
/*
    public void FireAndForget()
    {
      //Thread.Sleep(10000);
      System.Diagnostics.Debug.WriteLine("Fire And Forget");
    }
    public void ScheduleJob()
    {
      System.Diagnostics.Debug.WriteLine("Schedule Job");
    }
    
    public void Recurring()
    {
      System.Diagnostics.Debug.WriteLine("Recurring Job");
    }
    
    public void Recurring_job()
    {
      System.Diagnostics.Debug.WriteLine("Output Recurring Job");
      System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString("F"));
    }
    */
  }

}
