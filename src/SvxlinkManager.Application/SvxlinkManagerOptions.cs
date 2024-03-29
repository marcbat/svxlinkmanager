using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application
{
    public class SvxlinkManagerOptions(string liteDbFile)
    {
        public string LiteDbFile { get; } = liteDbFile;
    }
}
