using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Services.Intrerfaces
{
    public interface IServiceTypeName
    {
        string GetProviderName();
        string ServiceType { get; }
        string ServiceTypeShortName { get; }
    }
}