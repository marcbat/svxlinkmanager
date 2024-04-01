using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class DefaultParameter(Guid id, string name, string content)
    {
        public Guid Id { get; } = id;
        public string Name { get; } = name;
        public string Content { get; } = content;
    }
}
