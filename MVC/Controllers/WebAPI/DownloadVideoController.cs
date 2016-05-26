using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Vimeo.Types;
using CCC.Helpers;

namespace MVC.Controllers.WebAPI
{
    public class DownloadVideoController : ApiController
    {
        // GET download/video/<controller>
        public System.Web.Http.Results.RedirectResult Get(long id)
        {
            string url = VimeoHelpers.Api.GetVideoDownloadUrl(id);

            return Redirect(url);
        }
    }
}
