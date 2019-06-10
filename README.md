# Setup

### Installing

`Install-Package Hangfire`

`Install-Package Hangfire.Core`

`Install-Package Hangfire.SqlServer`

### Startup.cs
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
     RecurringJob.AddOrUpdate("id2",() => Recurring2(), Cron.Minutely);

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
public void Recurring2()
{
    Console.WriteLine("Recurring Job2");
}

```
## Result
### Database
![DB](https://i.imgur.com/hmzzV7h.png)
### Website
![DB](https://i.imgur.com/BfIsrQK.png)

## Reference
* [Background Jobs com HangFire e ASP.NET MVC | Background jobs with Hangfire and MVC](https://www.youtube.com/watch?v=_X_0YoGbceg) 
* [Hangfire Documentation](https://buildmedia.readthedocs.org/media/pdf/hangfire/latest/hangfire.pdf)
* [ASP.NET Core Applications](https://docs.hangfire.io/en/latest/getting-started/aspnet-core-applications.html#)
* [[ASP.NET]使用 Hangfire 來處理非同步的工作](https://dotblogs.com.tw/rainmaker/2015/08/19/153169)
* [Background jobs in ASP.NET Core made easy with Hangfire](https://crosscuttingconcerns.com/Background-jobs-ASP-NET-Core-Hangfire)
* [BlogsRelatedCodes](https://github.com/JamesYing/BlogsRelatedCodes/blob/master/hangfireDemo/HangfireWeb/Startup.cs)
* [在Asp.Net Core中使用DI的方式使用Hangfire構建後台執行腳本- James.Ying - 博客園](https://www.cnblogs.com/inday/p/hangfire-di-on-dot-net-core.html)
* [Hangfire中文文檔](https://www.bookstack.cn/read/Hangfire-zh-official/4.md)
* [[Hangfire]01-使用hangfire來執行background job](https://bryanyu.github.io/2018/09/03/Hangfire01/)

# HttpJob
## Startup.cs
```csharp
using Hangfire.HttpJob;
public void Configuration(IGlobalConfiguration globalConfiguration)
{
    globalConfiguration.UseSqlServerStorage("connect string").UseHangfireHttpJob();
}
public void ConfigureServices(IServiceCollection services)
{
    services.AddHangfireHttpJobAgent();
}
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseHangfireHttpJobAgent();
}

```
## HttpJob Reference
* [開源分佈式Job系統,調度與業務分離-HttpJob.Agent組件介紹以及如何使用](https://article.itxueyuan.com/98PZkR)
* [Hangfire.HttpJob](https://github.com/yuzd/Hangfire.HttpJob)
