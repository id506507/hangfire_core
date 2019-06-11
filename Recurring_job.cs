using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.HttpJob.Agent;

namespace test
{
  public class Recurring_job:JobAgent
  {
    protected override async Task OnStart(JobContext jobContext)
    {
      await Task.Delay(100);
      System.Diagnostics.Debug.WriteLine("This is a recurring job");
    }
    protected override void OnException(Exception ex)
    {
      throw new NotImplementedException();
    }
    protected override void OnStop(JobContext jobContext)
    {
      throw new NotImplementedException();
    }
  }
}
