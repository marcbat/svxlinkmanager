using LanguageExt;
using LanguageExt.Common;

using LiteDB;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SvxlinkManager.Application;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System.Text;

namespace SvxlinkManager.Infrastructure
{
    public class SvxlinkManagerConfigRepository : ISvxlinkManagerConfigRepository
    {
        private readonly SvxlinkManagerOptions options;
        private readonly ILogger<SvxlinkManagerConfigRepository> logger;

        public SvxlinkManagerConfigRepository(IOptions<SvxlinkManagerOptions> options, ILogger<SvxlinkManagerConfigRepository> logger)
        {
            this.options = options.Value;
            this.logger = logger;
        }

        public Validation<Error, Guid> Create(SvxlinkManagerConfigAggregate config)
        {
            using var db = new LiteDatabase(options.LiteDbFile);

            var collection = db.GetCollection<SvxlinkManagerConfigAggregate>("svxlinkManagerConfigs");

            collection.Insert(config);

            collection.EnsureIndex(x => x.Id, true);

            return config.Id;
        }

        public Validation<Error, SvxlinkManagerConfigAggregate> GetConfig(Guid configId)
        {
            try
            {
                using var db = new LiteDatabase(options.LiteDbFile);

                var collection = db.GetCollection<SvxlinkManagerConfigAggregate>("svxlinkManagerConfigs");

                collection.EnsureIndex(x => x.Id, true);

                var config = collection.FindById(configId);

                if (config is null)
                    return Error.New($"La configuration {configId} n'a pas été trouvée.");

                return config;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Erreur lors de la récupération de la configuration.");
                return Error.New("Erreur lors de la récupération de la configuration.", ex);
            }
        
        }

        public Validation<Error, Option<SvxlinkManagerConfigAggregate>> FindConfig(Guid configId)
        {
            try
            {
                using var db = new LiteDatabase(options.LiteDbFile);

                if(db.CollectionExists("svxlinkManagerConfigs"))
                    return Option<SvxlinkManagerConfigAggregate>.None;

                var collection = db.GetCollection<SvxlinkManagerConfigAggregate>("svxlinkManagerConfigs");

                collection.EnsureIndex(x => x.Id, true);

                var config = collection.FindById(configId);

                if (config is null)
                    return Option<SvxlinkManagerConfigAggregate>.None;

                return Option<SvxlinkManagerConfigAggregate>.Some(config);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Erreur lors de la récupération de la configuration.");
                return Error.New("Erreur lors de la récupération de la configuration.",ex);
            }

        }

        public Validation<Error, Unit> UpdateAsync(SvxlinkManagerConfigAggregate config)
        {
            try
            {
                using var db = new LiteDatabase(options.LiteDbFile);

                var collection = db.GetCollection<SvxlinkManagerConfigAggregate>("svxlinkManagerConfigs");

                collection.EnsureIndex(x => x.Id, true);

                collection.Update(config);

                return Unit.Default;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la mise à jour de la configuration.");
                return Error.New("Erreur lors de la mise à jour de la configuration.", ex);
            }
        }

        public Validation<Error, List<SvxlinkChannel>> GetAllOriginalChannels()
        {
            return from rrf in SvxlinkChannel.Create(Guid.Parse("235a4521-15a1-4e02-a540-91ee600452ac"), "Réseau des Répéteurs Francophones", "rrf2.f5nlg.ovh", 5300, "Fake", "Magnifique123456789!", "Fake")
                       from ssr in SvxlinkChannel.Create(Guid.Parse("1f2e87b8-d984-4c05-8a4a-ffad65c829a9"), "Salon Suisse Romand", "salonsuisseromand.hbspot.ch", 5300, "Fake", "xD9wW5gO7yD9hN5o", "Fake")
                       from fop in SvxlinkChannel.Create(Guid.Parse("0f669a03-dcf1-4277-9b07-54f6a0fd3037"), "French Open Network", "serveur.f1tzo.com", 5300, "Fake", "FON-F1TZO", "Fake")
                       from st in SvxlinkChannel.Create(Guid.Parse("a749ffe5-16c7-45da-809d-c048908f115c"), "Salon Technique", "rrf3.f5nlg.ovh", 5301, "Fake", "Magnifique123456789!", "Fake")
                       from si in SvxlinkChannel.Create(Guid.Parse("dd03fd9e-aeed-457e-97bf-973837a5fcec"), "Salon International","rrf3.f5nlg.ovh", 5302, "Fake", "Magnifique123456789!", "Fake")
                       from sb in SvxlinkChannel.Create(Guid.Parse("d4c59d86-947c-4b1d-831a-807c1877d426"), "Salon Bavardage", "serveur.f1tzo.com", 5301, "Fake", "FON-F1TZO", "Fake")
                       from sl in SvxlinkChannel.Create(Guid.Parse("9f99b18b-96ea-453d-b07a-7923c09c939f"), "Salon Local", "serveur.f1tzo.com", 5302, "Fake", "FON-F1TZO", "Fake")
                       from se in SvxlinkChannel.Create(Guid.Parse("dcc5afa2-790f-40ca-b24d-bf91e90b1ac7"), "Salon Expérimental", "rrf3.f5nlg.ovh", 5303, "Fake", "Magnifique123456789!", "Fake")
                       select new List<SvxlinkChannel> { { rrf }, { ssr }, { fop}, { st},{ si }, { sb}, { sl}, { se } };

           
        }

        public string GetDefaultSvxlinkConfig()
        {
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

            return sb.ToString();
        }
    }

}
