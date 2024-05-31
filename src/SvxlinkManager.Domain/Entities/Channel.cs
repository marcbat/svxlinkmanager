namespace SvxlinkManager.Domain.Entities
{
    public abstract class Channel : ManagedChannel
    {
        private string host;
        private string callSign;

        public Channel(Guid id, string name, string host, string callSign) : base(id, name)
        {

            Host = host;
            CallSign = callSign;
        }

        public string Host
        {
            get => host; set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom du host ne peut pas être vide.", nameof(value));
                }

                host = value;
            }
        }

        public string CallSign
        {
            get => callSign; set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom du callSign ne peut pas être vide.", nameof(value));
                }

                callSign = value;
            }
        }
    }
}
