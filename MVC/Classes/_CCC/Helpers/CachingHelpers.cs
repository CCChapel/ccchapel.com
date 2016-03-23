using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace CCC.Helpers
{
    public static partial class CachingHelpers
    {
        private static DateTimeOffset defaultExpiration = DateTimeOffset.Now.AddMinutes(30);

        private static ObjectCache cache = MemoryCache.Default;
        public static ObjectCache Cache
        {
            get
            {
                return cache;
            }
        }

        private static CacheItemPolicy policy = new CacheItemPolicy();
        public static CacheItemPolicy Policy
        {
            get
            {
                return policy;
            }
        }

        public static string CachingID(string className, object ID)
        {
            return string.Format("{0}_{1}", className, ID);
        }
    }
}