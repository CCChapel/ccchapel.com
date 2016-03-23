using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCC.Helpers
{
    public static partial class BrandingHelpers
    {
        public static string DateFormat(bool includeYear = true)
        {
            if (includeYear == true)
            {
                return "M.d.yyyy";
            }
            else
            {
                return "M.d";
            }
        }
    }
}