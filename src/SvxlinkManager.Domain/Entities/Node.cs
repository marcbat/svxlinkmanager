using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class Node
    {
        public string Name { get; }

        public Node(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Node otherNode = (Node)obj;
            return Name == otherNode.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
