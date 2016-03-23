using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vimeo.Types
{
    public partial class Pictures
    {
        public string uri { get; set; }
        public bool active { get; set; }
        public string type { get; set; }
        public List<Size> sizes { get; set; }

        public class Size
        {
            public int width { get; set; }
            public int height { get; set; }
            public string link { get; set; }
        }
    }
}
