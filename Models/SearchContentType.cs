using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitefinityWebApp.Services.Models
{
    public class SearchContentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssemblyName { get; set; }

        public static List<SearchContentType> GetList()
        {
            var list = new List<SearchContentType>();
            var pages = new SearchContentType() {Id = 1, Name = "Pages", AssemblyName = "Telerik.Sitefinity.Pages.Model.PageNode"};
            var cgms = new SearchContentType() { Id = 2, Name = "CGM", AssemblyName = "Telerik.Sitefinity.DynamicTypes.Model.DanatechProducts.Cgm" };
            var ims = new SearchContentType() { Id = 3, Name = "Insulinandmedicine", AssemblyName = "Telerik.Sitefinity.DynamicTypes.Model.DanatechProducts.Insulinandmedicine" };
            var ips = new SearchContentType() { Id = 4, Name = "InsulinPump", AssemblyName = "Telerik.Sitefinity.DynamicTypes.Model.DanatechProducts.InsulinPump" };
            var bgms = new SearchContentType() { Id = 5, Name = "BGM", AssemblyName = "Telerik.Sitefinity.DynamicTypes.Model.DanatechProducts.Bgm" };
            var appdtxs = new SearchContentType() { Id = 6, Name = "AppsAndDtx", AssemblyName = "Telerik.Sitefinity.DynamicTypes.Model.DanatechProducts.AppsAndDtx" };
            var iss = new SearchContentType() { Id = 7, Name = "Infusionset", AssemblyName = "Telerik.Sitefinity.DynamicTypes.Model.DanatechProducts.Infusionset" };
            list.Add(pages);
            list.Add(cgms);
            list.Add(ims);
            list.Add(ips);
            list.Add(bgms);
            list.Add(appdtxs);
            list.Add(iss);
            return list;
        }
    }
}
