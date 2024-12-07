namespace SvxlinkManager.Domain.Entities
{
    public abstract class ManagedChannel : ChannelBase
    {
        protected ManagedChannel()
        {
            
        }

        protected ManagedChannel(Guid id, string name) : base(id, name)
        {
            
        }

        public bool IsDefault { get; set; }

        public bool IsTemporized { get; set; }

        public int TimerDelay { get; set; } = 180;

        public bool IsActive { get; set; }

        public string? SoundGuid { get; set; }


    }
}
