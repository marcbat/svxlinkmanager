namespace SvxlinkManager.Domain.Entities
{
    public abstract class ChannelBase : Entity<Guid>
    {
        private string name;

        public ChannelBase(Guid id, string name) : base(id)
        {
            Name = name;
        }

        public string Name
        {
            get => name; set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom du channel ne peut pas être vide.", nameof(value));
                }

                name = value;
            }
        }
    }
}
