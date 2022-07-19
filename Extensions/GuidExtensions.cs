using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Services.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsNotNullOrEmpty(this Guid? value)
        {
            return value.HasValue && value.Value != Guid.Empty;
        }
        public static bool IsNotNullOrEmpty(this Guid value)
        {
            return value != null && value != Guid.Empty;
        }

        public static Guid ToGuidOrEmptyGuid(this string source)
        {
            Guid guid;
            if (Guid.TryParse(source, out guid))
                return guid;
            else
                return Guid.Empty;
        }
    }
}