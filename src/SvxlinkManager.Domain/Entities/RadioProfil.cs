using LanguageExt;
using static LanguageExt.Prelude;
using LanguageExt.Common;

namespace SvxlinkManager.Domain.Entities
{
    public class RadioProfil : Entity<Guid>
    {
        private string txCtcss;
        private string rxCtCss;
        private string name;
        private string rxFequency;
        private string txFrequency;
        private string squelch;

        public RadioProfil()
        {
        }

        internal RadioProfil(Guid id,
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
            this.name = name;
            this.rxFequency = rxFequency;
            this.txFrequency = txFrequency;
            Squelch = squelch;
            this.txCtcss = txCtcss;
            this.rxCtCss = rxCtCss;
            Volume = volume;
            PreEmph = preEmph;
            HightPass = hightPass;
            LowPass = lowPass;
            SquelchDetection = squelchDetection;
        }

        public static Validation<Error, RadioProfil> Create(Guid id,
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
                           string squelchDetection)
        {
            return (ValidateName(name), ValidateRxCtCss(rxCtCss), ValidateRxFequency(rxFequency), ValidateTxCtcss(txCtcss), ValidateTxFrequency(txFrequency))
                .Apply((vname, vrxCtCss, vrxFequency, vtxCtcss, vtxFrequency) => new RadioProfil(id,
                           vname,
                           vrxFequency,
                           vtxFrequency,
                           squelch,
                           vtxCtcss,
                           vrxCtCss,
                           volume,
                           preEmph,
                           hightPass,
                           lowPass,
                           squelchDetection));
        }

        public bool HasSa818 { get; set; } = true;

        public string Name => name;

        internal static Validation<Error, string> ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Error.New("Le nom du profil radio ne peut pas être vide.");

            return name;
        }

        public Validation<Error,string> SetName(string name) =>
            ValidateName(name).Map(vname => this.name = vname);

        public string RxFequency => rxFequency;

        internal static Validation<Error, string> ValidateRxFequency(string rxFequency)
        {
            if (string.IsNullOrWhiteSpace(rxFequency))
                return Error.New("La fréquence de réception ne peut pas être vide.");

            return rxFequency;
        }

        public Validation<Error, string> SetRxFequency(string rxFequency) =>
            ValidateRxFequency(rxFequency).Map(vrxFequency => this.rxFequency = vrxFequency);

        public string TxFrequency => txFrequency;

        internal static Validation<Error, string> ValidateTxFrequency(string txFrequency)
        {
            if (string.IsNullOrWhiteSpace(txFrequency))
                return Error.New("La fréquence de transmission ne peut pas être vide.");

            return txFrequency;
        }

        public Validation<Error, string> SetTxFrequency(string txFrequency) =>
            ValidateTxFrequency(txFrequency).Map(vtxFrequency => this.txFrequency = vtxFrequency);

        public string Squelch
        {
            get => squelch; set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le squelch ne peut pas être vide.", nameof(value));

                squelch = value;
            }
        }

        public string TxCtcss => txCtcss;

        internal static Validation<Error, string> ValidateTxCtcss(string txCtcss)
        {
            if (string.IsNullOrWhiteSpace(txCtcss))
                return Error.New("Le ton de CTCSS de transmission ne peut pas être vide.");

            var valueExist = Ctcss.ContainsValue(txCtcss);

            if(!valueExist)
                return Error.New("Le ton de CTCSS de transmission n'est pas valide.");

            var keyvalue = Ctcss.Single(x => x.Value == txCtcss);

            return keyvalue.Key;
        }

        public Validation<Error, string> SetTxCtcss(string txCtcss) =>
            ValidateTxCtcss(txCtcss).Map(x => this.txCtcss = x); 

        public string RxCtCss => rxCtCss;

        internal static Validation<Error, string> ValidateRxCtCss(string rxCtCss)
        {
            if (string.IsNullOrWhiteSpace(rxCtCss))
                return Error.New("Le ton de CTCSS de réception ne peut pas être vide.");

            var valueExist = Ctcss.ContainsValue(rxCtCss);

            if (!valueExist)
                return Error.New("Le ton de CTCSS de réception n'est pas valide.");

            var keyvalue = Ctcss.Single(x => x.Value == rxCtCss);

            return keyvalue.Key;
        }

        public Validation<Error, string> SetRxCtCss(string rxCtCss) =>
            ValidateRxCtCss(rxCtCss).Map(x => this.rxCtCss = x);

        private static Dictionary<string, string> Ctcss => new Dictionary<string, string>
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

        public string Volume { get; set; }

        public string PreEmph { get; set; }

        public string HightPass { get; set; }

        public string LowPass { get; set; }

        public string SquelchDetection { get; set; }

        public bool Enable { get; set; }

        public bool IsActive { get; set; }
    }
}
