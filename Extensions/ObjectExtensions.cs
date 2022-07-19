using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Services.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNotNull(this object item)
        {
            return item != null;
        }

        public static bool IsNull(this object item)
        {
            return item == null;
        }
    }
}