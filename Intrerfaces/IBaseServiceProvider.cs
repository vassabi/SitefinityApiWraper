using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Services.Intrerfaces
{
    public interface IBaseServiceProvider
    {
        string ServiceType { get; }
        string ServiceTypeShortName { get; }
        string GetApplicationName();
        string GetProviderName();
    }
}