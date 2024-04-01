using SvxlinkManager.Domain.Entities;

using System.Text;

namespace SvxlinkManager.Domain.Aggregates
{

    public class SvxlinkManagerConfigAggregate : AggregateRoot
    {
        private List<SvxlinkChannel> svxlinkChannels = [];
        private List<EcholinkChannel> echolinkChannels = [];
        private List<AdvanceSvxlinkChannel> advanceSvxlinkChannels = [];
        private List<Reflector> reflectors = [];
        private List<RadioProfil> radioProfils = [];

        protected SvxlinkManagerConfigAggregate(Guid id) : base(id)
        {
        }

        public SvxlinkManagerConfigAggregate()
        {
        }

        public static SvxlinkManagerConfigAggregate Create(Guid id)
        {
            return new SvxlinkManagerConfigAggregate(id);
        }

        public IReadOnlyCollection<SvxlinkChannel> SvxlinkChannels
        {
            get => svxlinkChannels.AsReadOnly();
            private set => svxlinkChannels = value.ToList();
        }

        public void AddSvxlinkChannel(SvxlinkChannel svxlinkChannel)
        {
            svxlinkChannels.Add(svxlinkChannel);
        }

        public void DeleteSvxlinkChannel(Guid channelId)
        {
            var svxlinkChannel = svxlinkChannels.FirstOrDefault(x => x.Id == channelId) ?? throw new Exception("Svxlink channel not found");

            svxlinkChannels.Remove(svxlinkChannel);
        }

        public IReadOnlyCollection<EcholinkChannel> EcholinkChannels
        {
            get => echolinkChannels.AsReadOnly();
            private set => echolinkChannels = value.ToList();
        }

        public void AddEcholinkChannel(EcholinkChannel echolinkChannel)
        {
            echolinkChannels.Add(echolinkChannel);
        }

        public void DeleteEcholinkChannel(Guid channelId)
        {
            var echolinkChannel = echolinkChannels.FirstOrDefault(x => x.Id == channelId) ?? throw new Exception("Echolink channel not found");

            echolinkChannels.Remove(echolinkChannel);
        }

        public IReadOnlyCollection<Reflector> Reflectors
        {
            get => reflectors.AsReadOnly();
            private set => reflectors = value.ToList();
        }

        public void AddReflector(Reflector reflector)
        {
            reflectors.Add(reflector);
        }

        public void DeleteReflector(Guid reflectorId)
        {
            var reflector = reflectors.FirstOrDefault(x => x.Id == reflectorId) ?? throw new Exception("Reflector not found");

            reflectors.Remove(reflector);
        }

        public IReadOnlyCollection<RadioProfil> RadioProfils
        {
            get => radioProfils.AsReadOnly();
            private set => radioProfils = value.ToList();
        }

        public void AddRadioProfil(RadioProfil radioProfil)
        {
            radioProfils.Add(radioProfil);
        }

        public void DeleteRadioProfil(Guid radioProfilId)
        {
            var radioProfil = radioProfils.FirstOrDefault(x => x.Id == radioProfilId) ?? throw new Exception("Radio profil not found");

            radioProfils.Remove(radioProfil);
        }

        public void DeleteAllRadioProfils()
        {
            radioProfils.Clear();
        }

        public IReadOnlyCollection<AdvanceSvxlinkChannel> AdvanceSvxlinkChannels
        {
            get => advanceSvxlinkChannels.AsReadOnly();
            private set => advanceSvxlinkChannels = value.ToList();
        }

        public void AddAdvanceSvxlinkChannel(AdvanceSvxlinkChannel advanceSvxlinkChannel)
        {
            advanceSvxlinkChannels.Add(advanceSvxlinkChannel);
        }

        public void DeleteAdvanceSvxlinkChannel(Guid channelId)
        {
            var avanceSvxlinkChannel = advanceSvxlinkChannels.FirstOrDefault(x => x.Id == channelId) ?? throw new Exception("AvanceSvxlink channel not found");

            advanceSvxlinkChannels.Remove(avanceSvxlinkChannel);
        }

        public void DeleteManagedChannel(Guid channelId)
        {
            var managedChannel = GetManagedChannel(channelId) ?? throw new Exception("Managed channel not found");

            if (managedChannel is SvxlinkChannel)
            {
                DeleteSvxlinkChannel(channelId);
            }
            else if (managedChannel is EcholinkChannel)
            {
                DeleteEcholinkChannel(channelId);
            }
            else if (managedChannel is AdvanceSvxlinkChannel)
            {
                DeleteAdvanceSvxlinkChannel(channelId);
            }
        }

        public IEnumerable<ManagedChannel> GetManagedChannels()
        {
            var managedChannels = new List<ManagedChannel>();

            managedChannels.AddRange(svxlinkChannels);
            managedChannels.AddRange(echolinkChannels);
            managedChannels.AddRange(advanceSvxlinkChannels);

            return managedChannels;
        }

        public IEnumerable<ManagedChannel> GetSvxlinkAndEcholinkChannels()
        {
            var managedChannels = new List<ManagedChannel>();

            managedChannels.AddRange(svxlinkChannels);
            managedChannels.AddRange(echolinkChannels);

            return managedChannels;
        }

        public ManagedChannel? GetManagedChannel(Guid channelId)
        {
            return GetManagedChannels().FirstOrDefault(x => x.Id == channelId);
        }

        public static IReadOnlyDictionary<string,string> GetDefaultParameters()
        {
            var defaultParameters = new Dictionary<string, string>();

            var sb = new StringBuilder();
            sb.AppendLine("[GLOBAL]");
            sb.AppendLine("LOGICS=LOGICS");
            sb.AppendLine("CFG_DIR = svxlink.d");
            sb.AppendLine("TIMESTAMP_FORMAT =% c");
            sb.AppendLine("CARD_SAMPLE_RATE = 16000");
            sb.AppendLine("CARD_CHANNELS=1");
            sb.AppendLine("LINKS=ALLlink");
            sb.AppendLine("");
            sb.AppendLine("[SimplexLogic]");
            sb.AppendLine("TYPE=Simplex");
            sb.AppendLine("RX=Rx1");
            sb.AppendLine("TX=Tx1");
            sb.AppendLine("MODULES=ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor,ModuleParrot");
            sb.AppendLine("CALLSIGN=REPORTCALLSIGN");
            sb.AppendLine("SHORT_IDENT_INTERVAL=15");
            sb.AppendLine("LONG_IDENT_INTERVAL=60");
            sb.AppendLine("IDENT_ONLY_AFTER_TX=10");
            sb.AppendLine("EXEC_CMD_ON_SQL_CLOSE=500");
            sb.AppendLine("EVENT_HANDLER=/usr/share/svxlink/events.tcl");
            sb.AppendLine("DEFAULT_LANG=fr_FR");
            sb.AppendLine("RGR_SOUND_ALWAYS=1");
            sb.AppendLine("RGR_SOUND_DELAY=0");
            sb.AppendLine("REPORT_CTCSS=REPORT_CTCSS");
            sb.AppendLine("TX_CTCSS=ALWAYS");
            sb.AppendLine("MACROS=Macros");
            sb.AppendLine("FX_GAIN_NORMAL=0");
            sb.AppendLine("FX_GAIN_LOW=-12");
            sb.AppendLine("ACTIVATE_MODULE_ON_LONG_CMD=10:PropagationMonitor");
            sb.AppendLine("MUTE_RX_ON_TX=1");
            sb.AppendLine("DTMF_CTRL_PTY=/tmp/dtmf_uhf");
            sb.AppendLine("");
            sb.AppendLine("[ALLlink]");
            sb.AppendLine("CONNECT_LOGICS=SimplexLogic:434MHZ:945,ReflectorLogic");
            sb.AppendLine("DEFAULT_ACTIVE=1");
            sb.AppendLine("TIMEOUT=0");
            sb.AppendLine("");
            sb.AppendLine("[Rx1]");
            sb.AppendLine("TYPE=Local");
            sb.AppendLine("AUDIO_DEV=alsa:plughw:0");
            sb.AppendLine("AUDIO_CHANNEL=0");
            sb.AppendLine("SQL_DET=GPIO");
            sb.AppendLine("SQL_START_DELAY=500");
            sb.AppendLine("SQL_DELAY=150");
            sb.AppendLine("SQL_HANGTIME=20");
            sb.AppendLine("SQL_EXTENDED_HANGTIME=1000");
            sb.AppendLine("SQL_EXTENDED_HANGTIME_THRESH=13");
            sb.AppendLine("SQL_TIMEOUT=600");
            sb.AppendLine("VOX_FILTER_DEPTH=300");
            sb.AppendLine("VOX_THRESH=1000");
            sb.AppendLine("CTCSS_MODE=2");
            sb.AppendLine("CTCSS_FQ=71.9");
            sb.AppendLine("CTCSS_SNR_OFFSET=0");
            sb.AppendLine("CTCSS_OPEN_THRESH=15");
            sb.AppendLine("CTCSS_CLOSE_THRESH=9");
            sb.AppendLine("CTCSS_BPF_LOW=60");
            sb.AppendLine("CTCSS_BPF_HIGH=260");
            sb.AppendLine("GPIO_PATH=/sys/class/gpio");
            sb.AppendLine("GPIO_SQL_PIN=gpio10");
            sb.AppendLine("DEEMPHASIS=0");
            sb.AppendLine("SQL_TAIL_ELIM=0");
            sb.AppendLine("PREAMP=-4");
            sb.AppendLine("PEAK_METER=1");
            sb.AppendLine("DTMF_DEC_TYPE=INTERNAL");
            sb.AppendLine("DTMF_MUTING=1");
            sb.AppendLine("DTMF_HANGTIME=40");
            sb.AppendLine("1750_MUTING=1");
            sb.AppendLine("");
            sb.AppendLine("[Tx1]");
            sb.AppendLine("TYPE=Local");
            sb.AppendLine("AUDIO_DEV=alsa:plughw:0");
            sb.AppendLine("AUDIO_CHANNEL=0");
            sb.AppendLine("PTT_TYPE=GPIO");
            sb.AppendLine("GPIO_PATH=/sys/class/gpio");
            sb.AppendLine("PTT_PIN=gpio7");
            sb.AppendLine("TIMEOUT=300");
            sb.AppendLine("TX_DELAY=900");
            sb.AppendLine("PREAMP=0");
            sb.AppendLine("CTCSS_FQ=71.9");
            sb.AppendLine("CTCSS_LEVEL=9");
            sb.AppendLine("PREEMPHASIS=0");
            sb.AppendLine("DTMF_TONE_LENGTH=100");
            sb.AppendLine("DTMF_TONE_SPACING=50");
            sb.AppendLine("DTMF_DIGIT_PWR=-15");
            sb.AppendLine("");
            sb.AppendLine("[ReflectorLogic]");
            sb.AppendLine("TYPE=Reflector");
            sb.AppendLine("AUDIO_CODEC=OPUS");
            sb.AppendLine("JITTER_BUFFER_DELAY=2");
            sb.AppendLine("CALLSIGN=CALLSIGN");
            sb.AppendLine("HOST=HOST");
            sb.AppendLine("AUTH_KEY=AUTH_KEY");
            sb.AppendLine("PORT=PORT");

            defaultParameters.Add("default.svxlink.conf", sb.ToString());

            sb = new StringBuilder();

            sb.AppendLine("[ModuleEchoLink]");
            sb.AppendLine("NAME=EchoLink");
            sb.AppendLine("ID=2");
            sb.AppendLine("SERVERS=europe.echolink.org");
            sb.AppendLine("CALLSIGN=CALLSIGN");
            sb.AppendLine("PASSWORD=PASSWORD");
            sb.AppendLine("SYSOPNAME=SYSOPNAME");
            sb.AppendLine("LOCATION=LOCATION");
            sb.AppendLine("MAX_QSOS=4");
            sb.AppendLine("MAX_CONNECTIONS=5");
            sb.AppendLine("LINK_IDLE_TIMEOUT=300");
            sb.AppendLine("USE_GSM_ONLY=0");
            sb.AppendLine("DESCRIPTION=DESCRIPTION");
            sb.AppendLine("DEFAULT_LANG=fr_FR");

            defaultParameters.Add("default.echolink.conf", sb.ToString());

            sb = new StringBuilder();

            sb.AppendLine("[ModuleParrot]");
            sb.AppendLine("NAME=Parrot");
            sb.AppendLine("ID=1");
            sb.AppendLine("TIMEOUT=600");
            sb.AppendLine("FIFO_LEN=60");
            sb.AppendLine("REPEAT_DELAY=1000");

            defaultParameters.Add("default.parrot.conf", sb.ToString());

            return defaultParameters;
        }


    }
}
