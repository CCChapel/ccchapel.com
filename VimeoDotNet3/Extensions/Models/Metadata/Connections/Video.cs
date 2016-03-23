using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vimeo.Types
{
    public partial class Metadata
    {
        public partial class Connections
        {
            public partial class Video
            {
                public string uri { get; set; }
                public List<string> options { get; set; }
                public int total { get; set; }
            }
        }
    }
   
}
