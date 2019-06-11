# Setup

### Installing
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
### Installing
`Install-Package Hangfire.HttpJob.Agent`

`Install-Package Hangfire.HttpJob.Client`
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
      var hangfireStartUpPath = "/job";
      app.UseHangfireDashboard(hangfireStartUpPath,new DashboardOptions {
        AppPath = "#",
        DisplayStorageConnectionString = false,
        IsReadOnlyFunc = Context => false,
      });
      app.UseHangfireServer();
      app.UseHangfireDashboard("/hangfire");
      app.UseHangfireHttpJobAgent();
}
```
## appsetting.json
```
{
  "JobAgent": {
    "Enabled": true,
    "SitemapUrl": "/jobagent"
  }
}
```
### Step
1. 在project裡創一個.cs，.cs需要inherit JobAgent的OnStart()、OnStop()、OnException()
#### Test.cs
```
protected override async Task OnStart(JobContext jobContext)
    {
      await Task.Delay(100);
      System.Diagnostics.Debug.WriteLine("This is a recurring job2");
      System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString("F"));

    }
    protected override void OnException(Exception ex)
    {
      throw new NotImplementedException();
    }
    protected override void OnStop(JobContext jobContext)
    {
      throw new NotImplementedException();
    }
```
2. 在Hangfire新增job
3. 填寫參數如下：
#### Scheduled Job
```
{
  "JobName": "TestJob",                     //Job的名字，不一定是.cs名稱
  "Method": "POST",                         //POST or GET 
  "ContentType": "application/json",        
  "Url": "http://localhost:5002/jobagent",  //Agent的地址，在之前appsetting.json裡已經設定好
  "DelayFromMinutes": -1,                   //-1:手動，0:立即執行，>1:延遲執行
  "Data": "",                               //當method=post時指定post的內容
  "Timeout": 5000,                          //http超時設定
  "BasicUserName": "",                  //Agent设置的basicAuth
  "BasicPassword": "",                //Agent设置的basicAuth

  "EnableRetry": false,                     //失敗時是否需要重試
  "SendSucMail": false,                     //成功時send email通知
  "SendFaiMail": true,                      //失敗時send email通知
  "Mail": "",              //email address
  "AgentClass": "test.testJob,test" //.cs名稱(namespace.class name,project name)
}
```
#### Recurring job
```
{
  "JobName": "",
  "Method": "GET",
  "ContentType": "application/json",
  "Url": "http://",
  "Data": {},
  "Timeout": 5000,
  "Cron": "",                               //重覆的時間，可以先用「Corn表達式生成」
  "BasicUserName": "",
  "BasicPassword": "",
  "QueueName": "",
  "EnableRetry": false,
  "SendSucMail": false,
  "SendFaiMail": true,
  "Mail": "",
  "AgentClass": ""
}
```
4. 在Startup.cs新增的job都沒辦法使用HttpJob的暫停或開始，也沒法得到狀態。但.cs的都可以正常運作
## HttpJob Reference
* [開源分佈式Job系統,調度與業務分離-HttpJob.Agent組件介紹以及如何使用](https://article.itxueyuan.com/98PZkR)
* [Hangfire.HttpJob](https://github.com/yuzd/Hangfire.HttpJob)
* [02.如何创建一个周期性的HttpJob](https://github.com/yuzd/Hangfire.HttpJob/wiki/02.%E5%A6%82%E4%BD%95%E5%88%9B%E5%BB%BA%E4%B8%80%E4%B8%AA%E5%91%A8%E6%9C%9F%E6%80%A7%E7%9A%84HttpJob)
* [Cron Expression Generator & Explainer - Quartz](https://www.freeformatter.com/cron-expression-generator-quartz.html)
