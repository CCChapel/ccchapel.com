using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;

namespace CCC.Helpers
{
    public static partial class ConfigurationHelper
    {
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}