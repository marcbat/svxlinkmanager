using Microsoft.Extensions.Logging;

using SvxlinkManager.Models;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.ServiceMockup
{
  public class Sa818ServiceMockup : ISa818Service
  {
    private ILogger<Sa818ServiceMockup> logger;

    public Sa818ServiceMockup(ILogger<Sa818ServiceMockup> logger)
    {
      this.logger = logger;
    }

    public void WriteRadioProfile(RadioProfile radioProfile) => logger.LogInformation($"Write {radioProfile.Name} radio profile.");
  }
}