using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.HttpJob.Agent;
using Microsoft.Extensions.Logging;

namespace test
{
  public class testJob:JobAgent
  {
    private readonly ILogger<testJob> _logger;
    public testJob(ILogger<testJob> logger)
    {
      _logger = logger;
      _logger.LogInformation($"Create {nameof(testJob)} Instance Success");
    }
    protected override async Task OnStart(JobContext jobContext)
    {
      await Task.Delay(100);
      System.Diagnostics.Debug.WriteLine("This is JobContext by testJob.cs");
      _logger.LogWarning(nameof(OnStart) + jobContext.Param ?? string.Empty);

    }
    protected override void OnStop(JobContext jobContext)
    {
      _logger.LogInformation(nameof(OnStop));
    }
    protected override void OnException(Exception ex)
    {
      _logger.LogError(ex, nameof(OnException) + (ex.Data["Method"] ?? string.Empty));
    }

  }
}
