using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

using ChurchCommunityBuilder.Api;

namespace CCB
{
    public partial class Api
    {
        public static ApiClient Client
        {
            get
            {
                return new ApiClient(
                    ConfigurationHelper.GetConnectionString("CCBInstanceName"), 
                    ConfigurationHelper.GetConnectionString("CCBUsername"),
                    ConfigurationHelper.GetConnectionString("CCBPassword"));
            }
        }

    }
}