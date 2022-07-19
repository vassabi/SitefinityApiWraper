using SitefinityWebApp.Services.Intrerfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Versioning;

namespace SitefinityWebApp.Services
{
    public class VersionService : IVersionService
    {
        private string transactionName;
        private VersionManager versionManager;

        public VersionService()
        {
            this.versionManager = VersionManager.GetManager(VersionManager.GetDefaultProviderName());
        }

        public VersionService(string transactionName)
        {
            this.transactionName = transactionName;
            this.versionManager = VersionManager.GetManager(VersionManager.GetDefaultProviderName(), this.transactionName);
        }

        public VersionService(string providerName, string transactionName)
        {
            this.versionManager = VersionManager.GetManager(providerName, transactionName);
        }

        public void BeginNewTransaction(string transactionName)
        {
            this.transactionName = transactionName;
            this.versionManager.TransactionName = this.transactionName;
        }

        /// <summary>
        /// Transaction name is designated by constructor parameter. Transaction can be committed as many times as necessary.
        /// </summary>
        /// <param name="transactionName"></param>
        public void CommitTransaction()
        {
            TransactionManager.CommitTransaction(this.transactionName);
        }

        public void CreateVersion(IDataItem item, bool isPublished)
        {
            this.versionManager.CreateVersion(item, isPublished);
        }
    }

}