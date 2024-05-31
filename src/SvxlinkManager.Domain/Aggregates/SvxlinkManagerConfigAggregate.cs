using SvxlinkManager.Domain.Entities;
using SvxlinkManager.Domain.Exceptions;

using System.Text;

namespace SvxlinkManager.Domain.Aggregates
{

    /// <summary>
    /// Représente l'agrégat de configuration du gestionnaire Svxlink.
    /// </summary>
    public class SvxlinkManagerConfigAggregate : AggregateRoot<Guid>
    {
        private List<SvxlinkChannel> svxlinkChannels = [];
        private List<EcholinkChannel> echolinkChannels = [];
        private List<AdvanceSvxlinkChannel> advanceSvxlinkChannels = [];
        private List<Reflector> reflectors = [];
        private List<RadioProfil> radioProfils = [];

        protected SvxlinkManagerConfigAggregate(Guid id) : base(id)
        {
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="SvxlinkManagerConfigAggregate"/>.
        /// </summary>
        public SvxlinkManagerConfigAggregate()
        {
        }

        /// <summary>
        /// Crée une nouvelle instance de la classe <see cref="SvxlinkManagerConfigAggregate"/> avec l'identifiant spécifié.
        /// </summary>
        /// <param name="id">L'identifiant de l'agrégat.</param>
        /// <returns>Une nouvelle instance de la classe <see cref="SvxlinkManagerConfigAggregate"/>.</returns>
        public static SvxlinkManagerConfigAggregate Create(Guid id)
        {
            return new SvxlinkManagerConfigAggregate(id);
        }

        /// <summary>
        /// Obtient la liste des canaux Svxlink.
        /// </summary>
        public IReadOnlyCollection<SvxlinkChannel> SvxlinkChannels
        {
            get => svxlinkChannels.AsReadOnly();
            private set => svxlinkChannels = value.ToList();
        }

        /// <summary>
        /// Ajoute un canal Svxlink.
        /// </summary>
        /// <param name="svxlinkChannel">Le canal Svxlink à ajouter.</param>
        public void AddSvxlinkChannel(SvxlinkChannel svxlinkChannel)
        {
            if(svxlinkChannels.Any(c => c.Name == svxlinkChannel.Name))
                throw new DomainException("Un canal Svxlink avec le même nom existe déjà.");

            svxlinkChannels.Add(svxlinkChannel);
        }

        /// <summary>
        /// Supprime un canal Svxlink.
        /// </summary>
        /// <param name="channelId">L'identifiant du canal à supprimer.</param>
        public void DeleteSvxlinkChannel(Guid channelId)
        {
            var svxlinkChannel = svxlinkChannels.FirstOrDefault(x => x.Id == channelId) ?? throw new Exception("Canal Svxlink introuvable");

            svxlinkChannels.Remove(svxlinkChannel);
        }

        /// <summary>
        /// Obtient la liste des canaux Echolink.
        /// </summary>
        public IReadOnlyCollection<EcholinkChannel> EcholinkChannels
        {
            get => echolinkChannels.AsReadOnly();
            private set => echolinkChannels = value.ToList();
        }

        /// <summary>
        /// Ajoute un canal Echolink.
        /// </summary>
        /// <param name="echolinkChannel">Le canal Echolink à ajouter.</param>
        public void AddEcholinkChannel(EcholinkChannel echolinkChannel)
        {
            if (echolinkChannels.Any(c => c.Name == echolinkChannel.Name))
                throw new DomainException("Un canal Echolink avec le même nom existe déjà.");

            echolinkChannels.Add(echolinkChannel);
        }

        /// <summary>
        /// Supprime un canal Echolink.
        /// </summary>
        /// <param name="channelId">L'identifiant du canal à supprimer.</param>
        public void DeleteEcholinkChannel(Guid channelId)
        {
            var echolinkChannel = echolinkChannels.FirstOrDefault(x => x.Id == channelId) ?? throw new Exception("Canal Echolink introuvable");

            echolinkChannels.Remove(echolinkChannel);
        }

        /// <summary>
        /// Obtient la liste des réflecteurs.
        /// </summary>
        public IReadOnlyCollection<Reflector> Reflectors
        {
            get => reflectors.AsReadOnly();
            private set => reflectors = value.ToList();
        }

        /// <summary>
        /// Ajoute un réflecteur.
        /// </summary>
        /// <param name="reflector">Le réflecteur à ajouter.</param>
        public void AddReflector(Reflector reflector)
        {
            if(reflectors.Any(r => r.Name == reflector.Name))
                throw new DomainException("Un réflecteur avec le même nom existe déjà.");

            reflectors.Add(reflector);
        }

        /// <summary>
        /// Supprime un réflecteur.
        /// </summary>
        /// <param name="reflectorId">L'identifiant du réflecteur à supprimer.</param>
        public void DeleteReflector(Guid reflectorId)
        {
            var reflector = reflectors.FirstOrDefault(x => x.Id == reflectorId) ?? throw new Exception("Réflecteur introuvable");

            reflectors.Remove(reflector);
        }

        /// <summary>
        /// Obtient la liste des profils radio.
        /// </summary>
        public IReadOnlyCollection<RadioProfil> RadioProfils
        {
            get => radioProfils.AsReadOnly();
            private set => radioProfils = value.ToList();
        }

        /// <summary>
        /// Ajoute un profil radio.
        /// </summary>
        /// <param name="radioProfil">Le profil radio à ajouter.</param>
        public void AddRadioProfil(RadioProfil radioProfil)
        {
            if(radioProfils.Any(rp => rp.Name == radioProfil.Name))
                throw new DomainException("Un profil radio avec le même nom existe déjà.");

            radioProfils.Add(radioProfil);
        }

        /// <summary>
        /// Supprime un profil radio.
        /// </summary>
        /// <param name="radioProfilId">L'identifiant du profil radio à supprimer.</param>
        public void DeleteRadioProfil(Guid radioProfilId)
        {
            var radioProfil = radioProfils.FirstOrDefault(x => x.Id == radioProfilId) ?? throw new Exception("Profil radio introuvable");

            radioProfils.Remove(radioProfil);
        }

        /// <summary>
        /// Supprime tous les profils radio.
        /// </summary>
        public void DeleteAllRadioProfils()
        {
            radioProfils.Clear();
        }

        /// <summary>
        /// Obtient la liste des canaux Svxlink avancés.
        /// </summary>
        public IReadOnlyCollection<AdvanceSvxlinkChannel> AdvanceSvxlinkChannels
        {
            get => advanceSvxlinkChannels.AsReadOnly();
            private set => advanceSvxlinkChannels = value.ToList();
        }

        /// <summary>
        /// Ajoute un canal Svxlink avancé.
        /// </summary>
        /// <param name="advanceSvxlinkChannel">Le canal Svxlink avancé à ajouter.</param>
        public void AddAdvanceSvxlinkChannel(AdvanceSvxlinkChannel advanceSvxlinkChannel)
        {
            if (advanceSvxlinkChannels.Any(c => c.Name == advanceSvxlinkChannel.Name))
                throw new DomainException("Un canal Svxlink avancé avec le même nom existe déjà.");

            advanceSvxlinkChannels.Add(advanceSvxlinkChannel);
        }

        /// <summary>
        /// Supprime un canal Svxlink avancé.
        /// </summary>
        /// <param name="channelId">L'identifiant du canal à supprimer.</param>
        public void DeleteAdvanceSvxlinkChannel(Guid channelId)
        {
            var avanceSvxlinkChannel = advanceSvxlinkChannels.FirstOrDefault(x => x.Id == channelId) ?? throw new Exception("Canal Svxlink avancé introuvable");

            advanceSvxlinkChannels.Remove(avanceSvxlinkChannel);
        }

        /// <summary>
        /// Supprime un canal géré.
        /// </summary>
        /// <param name="channelId">L'identifiant du canal à supprimer.</param>
        public void DeleteManagedChannel(Guid channelId)
        {
            var managedChannel = GetManagedChannel(channelId) ?? throw new Exception("Canal géré introuvable");

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

        /// <summary>
        /// Obtient la liste des canaux gérés.
        /// </summary>
        /// <returns>La liste des canaux gérés.</returns>
        public IEnumerable<ManagedChannel> GetManagedChannels()
        {
            var managedChannels = new List<ManagedChannel>();

            managedChannels.AddRange(svxlinkChannels);
            managedChannels.AddRange(echolinkChannels);
            managedChannels.AddRange(advanceSvxlinkChannels);

            return managedChannels;
        }

        /// <summary>
        /// Obtient la liste des canaux Svxlink et Echolink.
        /// </summary>
        /// <returns>La liste des canaux Svxlink et Echolink.</returns>
        public IEnumerable<ManagedChannel> GetSvxlinkAndEcholinkChannels()
        {
            var managedChannels = new List<ManagedChannel>();

            managedChannels.AddRange(svxlinkChannels);
            managedChannels.AddRange(echolinkChannels);

            return managedChannels;
        }

        /// <summary>
        /// Obtient le canal géré avec l'identifiant spécifié.
        /// </summary>
        /// <param name="channelId">L'identifiant du canal géré.</param>
        /// <returns>Le canal géré correspondant à l'identifiant spécifié, ou null si aucun canal n'est trouvé.</returns>
        public ManagedChannel? GetManagedChannel(Guid channelId)
        {
            return GetManagedChannels().FirstOrDefault(x => x.Id == channelId);
        }

        /// <summary>
        /// Obtient les paramètres par défaut.
        /// </summary>
        /// <returns>Un dictionnaire contenant les paramètres par défaut.</returns>
        public static IReadOnlyDictionary<string, string> GetDefaultParameters()
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

        /// <summary>
        /// Définit le canal actif.
        /// </summary>
        /// <param name="id">L'identifiant du canal à définir comme actif.</param>
        /// <exception cref="Exception">Le canal spécifié est introuvable.</exception>
        public void SetActiveChannel(Guid id)
        {
            try
            {
                foreach (var channel in GetManagedChannels())
                    channel.IsActive = false;

                var activeChannel = GetManagedChannel(id) ?? throw new Exception("Canal introuvable");
                activeChannel.IsActive = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Impossible de définir le canal actif.", ex);
            }
        }

        public ManagedChannel GetActiveChannel()
        {
            return GetManagedChannels().Single(x => x.IsActive);
        }

        public void SetActiveRadioProfile(Guid id)
        {
            try
            {
                foreach (var radioProfil in radioProfils)
                    radioProfil.IsActive = false;

                var activeRadioProfil = radioProfils.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Profil radio introuvable");
                activeRadioProfil.IsActive = true;
            }
            catch (Exception ex)
            {

                throw new Exception("Impossible de setter le radio profil par defaut.", ex);
            }
        }

        public RadioProfil GetActiveRadioProfile()
        {
            try
            {
                var profil = radioProfils.FirstOrDefault(x => x.IsActive) ?? throw new Exception("Profil radio non trouvé.");
                return profil;
            }
            catch (Exception ex)
            {
                throw new Exception("Impossible de trouver le profil radio actif.", ex);
            }
        }
    }
}
