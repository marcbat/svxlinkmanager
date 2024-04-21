namespace SvxlinkManager.Domain.Entities
{
    public class RadioProfil : Entity
    {
        public RadioProfil(Guid id,
                           string name,
                           string rxFequency,
                           string txFrequency,
                           string squelch,
                           string txCtcss,
                           string rxCtCss,
                           string volume,
                           string preEmph,
                           string hightPass,
                           string lowPass,
                           string squelchDetection) : base(id)
        {
            Name = name;
            RxFequency = rxFequency;
            TxFrequency = txFrequency;
            Squelch = squelch;
            TxCtcss = txCtcss;
            RxCtCss = rxCtCss;
            Volume = volume;
            PreEmph = preEmph;
            HightPass = hightPass;
            LowPass = lowPass;
            SquelchDetection = squelchDetection;
        }

        public bool HasSa818 { get; set; } = true;

        public string Name { get; }
        public string RxFequency { get; }
        public string TxFrequency { get; }
        public string Squelch { get; }
        public string TxCtcss { get; }
        public string RxCtCss { get; }
        public string Volume { get; }
        public string PreEmph { get; }
        public string HightPass { get; }
        public string LowPass { get; }
        public string SquelchDetection { get; }
        public bool Enable { get; set; }

        internal bool IsActive { get; set; }
    }
}
