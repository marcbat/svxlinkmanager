namespace SvxlinkManager.Domain.Entities
{
    public abstract class ManagedChannel : ChannelBase
    {
        protected ManagedChannel(Guid id, string name, Sound sound) : base(id, name)
        {
            Sound = sound;
        }

        public bool IsDefault { get; set; }

        public bool IsTemporized { get; set; }

        public int TimerDelay { get; set; } = 180;

        public Sound Sound { get; }
    }
}
