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

        public string Name
        {
            get => name;
            protected set => name = value;
        }

        internal static Validation<Error, string> ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Error.New("Le nom du profil radio ne peut pas être vide.");

            return name;
        }

        public Validation<Error,string> SetName(string name) =>
            ValidateName(name).Map(vname => this.name = vname);

        public string RxFequency
        {
            get => rxFequency;
            protected set => rxFequency = value;
        }

        internal static Validation<Error, string> ValidateRxFequency(string rxFequency)
        {
            if (string.IsNullOrWhiteSpace(rxFequency))
                return Error.New("La fréquence de réception ne peut pas être vide.");

            return rxFequency;
        }

        public Validation<Error, string> SetRxFequency(string rxFequency) =>
            ValidateRxFequency(rxFequency).Map(vrxFequency => this.rxFequency = vrxFequency);

        public string TxFrequency
        {
            get => txFrequency;
            protected set => txFrequency = value;
        }

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

        public string TxCtcss
        {
            get => txCtcss;
            protected set => txCtcss = value;
        }

        internal static Validation<Error, string> ValidateTxCtcss(string txCtcss)
        {
            if (string.IsNullOrWhiteSpace(txCtcss))
                return Error.New("Le ton de CTCSS de transmission ne peut pas être vide.");

            var valueExist = Ctcss.ContainsKey(txCtcss);

            if(!valueExist)
                return Error.New("Le ton de CTCSS de transmission n'est pas valide.");

            return txCtcss;
        }

        public Validation<Error, string> SetTxCtcss(string txCtcss) =>
            ValidateTxCtcss(txCtcss).Map(x => this.txCtcss = x); 

        public string InternalTxCtcss => Ctcss[txCtcss];

        public string RxCtCss
        {
            get => rxCtCss;
            protected set => rxCtCss = value;
        }

        internal static Validation<Error, string> ValidateRxCtCss(string rxCtCss)
        {
            if (string.IsNullOrWhiteSpace(rxCtCss))
                return Error.New("Le ton de CTCSS de réception ne peut pas être vide.");

            var valueExist = Ctcss.ContainsKey(rxCtCss);

            if (!valueExist)
                return Error.New("Le ton de CTCSS de réception n'est pas valide.");

            return rxCtCss;
        }

        public Validation<Error, string> SetRxCtCss(string rxCtCss) =>
            ValidateRxCtCss(rxCtCss).Map(x => this.rxCtCss = x);

        public string InternalRxCtCss => Ctcss[rxCtCss];

        private static Dictionary<string, string> Ctcss => new()
        {
            { "Pas de tone", "0000" },
            { "67", "0001" },
            { "71.9", "0002" },
            { "74.4", "0003" },
            { "77", "0004" },
            { "79.7", "0005" },
            { "82.5", "0006" },
            { "85.4", "0007" },
            { "88.5", "0008" },
            { "91.5", "0009" },
            { "94.8", "0010" },
            { "97.4", "0011" },
            { "100", "0012" },
            { "103.5", "0013" },
            { "107.2", "0014" },
            { "110.9", "0015" },
            { "114.8", "0016" },
            { "118.8", "0017" },
            { "123", "0018" },
            { "127.3", "0019" },
            { "131.8", "0020" },
            { "136.5", "0021" },
            { "141.3", "0022" },
            { "146.2", "0023" },
            { "151.4", "0024" },
            { "156.7", "0025" },
            { "162.2", "0026" },
            { "167.9", "0027" },
            { "173.8", "0028" },
            { "179.9", "0029" },
            { "186.2", "0030" },
            { "192.8", "0031" },
            { "203.5", "0032" },
            { "210.7", "0033" },
            { "218.1", "0034" },
            { "225.7", "0035" },
            { "233.6", "0036" },
            { "241.8", "0037" },
            { "250.3", "0038" }
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
