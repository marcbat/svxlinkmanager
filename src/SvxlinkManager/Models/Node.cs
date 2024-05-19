namespace SvxlinkManager.Models
{
    public class Node
    {
        private string className = "node";

        public string Name { get; set; }

        public string ClassName
        {
            get => className;
            set => className = value;
        }

        public override bool Equals(object obj) => Name.Trim() == ((Node)obj).Name.Trim();

        public override int GetHashCode() => Name.Trim().GetHashCode();

        public static implicit operator Node(Domain.Entities.Node node)
        {
            return new Node
            {
                Name = node.Name
            };
        }
    }
}