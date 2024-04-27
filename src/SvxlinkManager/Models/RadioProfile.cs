using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SvxlinkManager.Models
{
    public class RadioProfile : IModelEntity, INotifyPropertyChanged
    {
        private bool enable;
        private string trx = "interne";

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Enable
        {
            get => enable;
            set
            {
                enable = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enable)));
            }
        }

        [RegularExpression("^[0-9]{3}.[0-9]{3}")]
        public string RxFequ { get; set; }

        [RegularExpression("^[0-9]{3}.[0-9]{3}")]
        public string TxFrequ { get; set; }

        [Required]
        public string Squelch { get; set; } = "2";

        [Required]
        public string Volume { get; set; } = "4";

        [Required]
        public string PreEmph { get; set; } = "0";

        [Required]
        public string HightPass { get; set; } = "0";

        [Required]
        public string LowPass { get; set; } = "0";

        [Required]
        public string SquelchDetection { get; set; } = "GPIO";

        public bool HasSa818 { get; set; } = true;

        /// <summary>CTCSS au format classique</summary>
        /// <value>The rx tone.</value>
        public string TxTone { get; set; }

        /// <summary>CTCSS au format classique</summary>
        /// <value>The rx tone.</value>
        public string RxTone { get;set; }

        public List<string> Ctcss => new List<string>
    {
      {"Pas de tone" },
      { "67" },
      { "71.9" },
      { "74.4" },
      { "77" },
      { "79.7" },
      { "82.5" },
      { "85.4" },
      { "88.5" },
      { "91.5" },
      { "94.8" },
      { "97.4" },
      { "100" },
      { "103.5" },
      { "107.2" },
      { "110.9" },
      { "114.8" },
      { "118.8" },
      { "123" },
      { "127.3" },
      { "131.8" },
      { "136.5" },
      { "141.3" },
      { "146.2" },
      { "151.4" },
      { "156.7" },
      { "162.2" },
      { "167.9" },
      { "173.8" },
      { "179.9" },
      { "186.2" },
      { "192.8" },
      { "203.5" },
      { "210.7" },
      { "218.1" },
      { "225.7" },
      { "233.6" },
      { "241.8" },
      { "250.3" }
    };

        [NotMapped]
        public Dictionary<string, string> TrackProperties => new Dictionary<string, string> {
        { nameof(Name), Name },
        { nameof(RxFequ), RxFequ },
        { nameof(TxFrequ), TxFrequ },
        { nameof(RxTone), RxTone },
        { nameof(TxTone), TxTone },
        { nameof(Squelch), Squelch },
        { nameof(SquelchDetection), SquelchDetection },
        { nameof(Volume), Volume },
        { nameof(PreEmph), PreEmph },
        { nameof(HightPass), HightPass },
        { nameof(LowPass), LowPass },
        { nameof(HasSa818), HasSa818.ToString() },
        };

        [NotMapped]
        public Dictionary<string, string> TrxTypes { get; } = new Dictionary<string, string> { { "interne", "Hotspot" }, { "externe", "Externe" } };

        [NotMapped]
        public string Trx
        {
            get
            {
                if (HasSa818)
                    return "interne";
                else
                    return "externe";
            }
            set
            {
                switch (value)
                {
                    case "interne":
                        SquelchDetection = "GPIO";
                        HasSa818 = true;
                        break;

                    default:
                        SquelchDetection = "CTCSS";
                        HasSa818 = false;
                        break;
                }

                trx = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public static implicit operator RadioProfile(Domain.Entities.RadioProfil v)
        {
            return new RadioProfile
            {
                Id = v.Id,
                Name = v.Name,
                RxFequ = v.RxFequency,
                TxFrequ = v.TxFrequency,
                Squelch = v.Squelch,
                TxTone = v.TxCtcss,
                RxTone = v.RxCtCss,
                Volume = v.Volume,
                PreEmph = v.PreEmph,
                HightPass = v.HightPass,
                LowPass = v.LowPass
            };
        }
    }
}