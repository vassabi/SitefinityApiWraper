using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Model;

namespace SitefinityWebApp.Services.Intrerfaces
{
    public interface IVersionService
    {
        void BeginNewTransaction(string transactionName);
        void CommitTransaction();
        void CreateVersion(IDataItem item, bool isPublished);
    }
}