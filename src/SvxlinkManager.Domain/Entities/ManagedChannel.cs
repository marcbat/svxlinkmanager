namespace SvxlinkManager.Domain.Entities
{
    public abstract class ManagedChannel : ChannelBase
    {
        protected ManagedChannel(Guid id, string name, string soundGuid) : base(id, name)
        {
            SoundGuid = soundGuid;
        }

        public bool IsDefault { get; set; }

        public bool IsTemporized { get; set; }

        public int TimerDelay { get; set; } = 180;

        public bool IsActive { get; set; }

        public string SoundGuid { get; }
    }
}
