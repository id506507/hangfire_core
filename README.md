# hangfire_core

#Setup

#Installing

`Install-Package Hangfire`

`Install-Package Hangfire.Core`

`Install-Package Hangfire.SqlServer`

###Startup.cs
```csharp
using Hangfire;
using Hangfire.SqlServer;
using System.Threading;

public Startup(IConfiguration configuration)
        {
            //Configuration = configuration;
            GlobalConfiguration.Configuration.UseSqlServerStorage("connect string");
        }
public void ConfigureServices(IServiceCollection services)
        {
	    services.AddHangfire(x => x.UseSqlServerStorage("connect string"));
            services.AddHangfireServer();
        }
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			//Fire and forget
            BackgroundJob.Enqueue(() => FireAndForget());
            //Scheduled jobs
            BackgroundJob.Schedule(() => ScheduleJob(), TimeSpan.FromMilliseconds(10000));
            //Recurring jobs
            RecurringJob.AddOrUpdate(() => Recurring(), Cron.Daily);
	
			app.UseHangfireDashboard();
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

```
##Result
###Database
