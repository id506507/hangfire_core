using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.SqlServer;
using System.Threading;

namespace hangfire_core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Configuration = configuration;
            GlobalConfiguration.Configuration.UseSqlServerStorage("Data Source=WOWCOMPUTER\\SQLEXPRESS;Initial Catalog=tempdb;Integrated Security=True;");
            
        }

        public IConfiguration Configuration { get; }

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
            services.AddHangfire(x => x.UseSqlServerStorage("Data Source=WOWCOMPUTER\\SQLEXPRESS;Initial Catalog=tempdb;Integrated Security=True;"));
            services.AddHangfireServer();
            //Hangfire

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //Hangfire
            //Fire and forget
            BackgroundJob.Enqueue(() => FireAndForget());
            //Scheduled jobs
            BackgroundJob.Schedule(() => ScheduleJob(), TimeSpan.FromMilliseconds(10000));
            //Recurring jobs
            RecurringJob.AddOrUpdate(() => Recurring(), Cron.Daily);
            //Hangfire
            //Hangfire
            app.UseHangfireDashboard();
            //Hangfire

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        public void FireAndForget()
        {
            Thread.Sleep(100);
            Console.WriteLine("Fire and forget");
        }
        public void ScheduleJob()
        {
            Console.WriteLine("Schedule Job");
        }
        public void Recurring()
        {
            Console.WriteLine("Recurring Job");
        }
    }
}
