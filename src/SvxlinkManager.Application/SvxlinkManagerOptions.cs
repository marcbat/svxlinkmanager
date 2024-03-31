using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application
{
    public class SvxlinkManagerOptions
    {
        public SvxlinkManagerOptions()
        {
            
        }

        public SvxlinkManagerOptions(string liteDbFile)
        {
            LiteDbFile = liteDbFile;
        }

        public string LiteDbFile { get; set; }

        public Guid ConfigId { get; set; }
    }
}
