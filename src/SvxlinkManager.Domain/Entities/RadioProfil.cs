namespace SvxlinkManager.Domain.Entities
{
    public class RadioProfil : Entity
    {
        private string txCtcss;
        private string rxCtCss;

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

        public string TxCtcss { 
            get {
                return Ctcss[txCtcss];
            } 
            private set {
                var ctcss = Ctcss.SingleOrDefault(x => x.Value == value);
                if (ctcss.Key == null)
                    throw new ArgumentException("Invalid value for TxCtcss");

                txCtcss = ctcss.Key;
            } 
        }

        public string RxCtCss
        {
            get
            {
                return Ctcss[rxCtCss];
            }

            private set
            {
                var ctcss = Ctcss.SingleOrDefault(x => x.Value == value);
                if (ctcss.Key == null)
                    throw new ArgumentException("Invalid value for RxCtCss");

                rxCtCss = ctcss.Key;
            }
        }

        private Dictionary<string, string> Ctcss => new Dictionary<string, string>
        {
          {"0000", "Pas de tone" },
          {"0001","67"},
          {"0002","71.9"},
          {"0003","74.4"},
          {"0004","77"},
          {"0005","79.7"},
          {"0006","82.5"},
          {"0007","85.4"},
          {"0008","88.5"},
          {"0009","91.5"},
          {"0010","94.8"},
          {"0011","97.4"},
          {"0012","100"},
          {"0013","103.5"},
          {"0014","107.2"},
          {"0015","110.9"},
          {"0016","114.8"},
          {"0017","118.8"},
          {"0018","123"},
          {"0019","127.3"},
          {"0020","131.8"},
          {"0021","136.5"},
          {"0022","141.3"},
          {"0023","146.2"},
          {"0024","151.4"},
          {"0025","156.7"},
          {"0026","162.2"},
          {"0027","167.9"},
          {"0028","173.8"},
          {"0029","179.9"},
          {"0030","186.2"},
          {"0031","192.8"},
          {"0032","203.5"},
          {"0033","210.7"},
          {"0034","218.1"},
          {"0035","225.7"},
          {"0036","233.6"},
          {"0037","241.8"},
          {"0038","250.3"}
        };

        public string Volume { get; }
        public string PreEmph { get; }
        public string HightPass { get; }
        public string LowPass { get; }
        public string SquelchDetection { get; }
        public bool Enable { get; set; }
        public bool IsActive { get; set; }
    }
}
