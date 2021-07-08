using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Data.Configurations
{
  public class SvxlinkManagerParametersConfiguration : IEntityTypeConfiguration<SvxlinkManagerParameter>
  {
    public void Configure(EntityTypeBuilder<SvxlinkManagerParameter> builder)
    {
      var sb = new StringBuilder();
      sb.AppendLine("[GLOBAL]");
      sb.AppendLine("LOGICS = LOGICS");
      sb.AppendLine("CFG_DIR = svxlink.d");
      sb.AppendLine("TIMESTAMP_FORMAT =% c");
      sb.AppendLine("CARD_SAMPLE_RATE = 16000");
      sb.AppendLine("CARD_CHANNELS = 1");
      sb.AppendLine("LINKS = ALLlink");
      sb.AppendLine("[SimplexLogic]");
      sb.AppendLine("TYPE = Simplex");
      sb.AppendLine("RX = Rx1");
      sb.AppendLine("TX = Tx1");
      sb.AppendLine("MODULES = MODULES");
      sb.AppendLine("CALLSIGN = REPORTCALLSIGN");
      sb.AppendLine("SHORT_IDENT_INTERVAL = 15");
      sb.AppendLine("LONG_IDENT_INTERVAL = 60");
      sb.AppendLine("IDENT_ONLY_AFTER_TX = 10");
      sb.AppendLine("EXEC_CMD_ON_SQL_CLOSE = 500");
      sb.AppendLine("EVENT_HANDLER =/ usr / share / svxlink / events.tcl");
      sb.AppendLine("DEFAULT_LANG = fr_FR");
      sb.AppendLine("RGR_SOUND_ALWAYS = 1");
      sb.AppendLine("RGR_SOUND_DELAY = 0");
      sb.AppendLine("REPORT_CTCSS = REPORT_CTCSS");
      sb.AppendLine("TX_CTCSS = ALWAYS");
      sb.AppendLine("MACROS = Macros");
      sb.AppendLine("FX_GAIN_NORMAL = 0");
      sb.AppendLine("FX_GAIN_LOW = -12");
      sb.AppendLine("ACTIVATE_MODULE_ON_LONG_CMD = 10:PropagationMonitor");
      sb.AppendLine("MUTE_RX_ON_TX = 1");
      sb.AppendLine("DTMF_CTRL_PTY =/ tmp / dtmf_uhf");
      sb.AppendLine("[ALLlink]");
      sb.AppendLine("CONNECT_LOGICS = SimplexLogic:434MHZ:945, ReflectorLogic");
      sb.AppendLine("DEFAULT_ACTIVE = 1");
      sb.AppendLine("TIMEOUT = 0");
      sb.AppendLine("[Rx1]");
      sb.AppendLine("TYPE = Local");
      sb.AppendLine("AUDIO_DEV = udp:127.0.0.1:10000");
      sb.AppendLine("AUDIO_CHANNEL = 0");
      sb.AppendLine("SQL_DET = CTCSS");
      sb.AppendLine("SQL_START_DELAY = 500");
      sb.AppendLine("SQL_DELAY = 100");
      sb.AppendLine("SQL_HANGTIME = 20");
      sb.AppendLine("SQL_EXTENDED_HANGTIME = 1000");
      sb.AppendLine("SQL_EXTENDED_HANGTIME_THRESH = 13");
      sb.AppendLine("SQL_TIMEOUT = 600");
      sb.AppendLine("VOX_FILTER_DEPTH = 300");
      sb.AppendLine("VOX_THRESH = 1000");
      sb.AppendLine("CTCSS_MODE = 2");
      sb.AppendLine("CTCSS_FQ = 71.9");
      sb.AppendLine("CTCSS_SNR_OFFSET = 0");
      sb.AppendLine("CTCSS_OPEN_THRESH = 15");
      sb.AppendLine("CTCSS_CLOSE_THRESH = 9");
      sb.AppendLine("CTCSS_BPF_LOW = 60");
      sb.AppendLine("CTCSS_BPF_HIGH = 260");
      sb.AppendLine("#GPIO_PATH=/sys/class/gpio");
      sb.AppendLine("#GPIO_SQL_PIN=gpio10");
      sb.AppendLine("DEEMPHASIS = 0");
      sb.AppendLine("SQL_TAIL_ELIM = 0");
      sb.AppendLine("PREAMP = -4");
      sb.AppendLine("PEAK_METER = 1");
      sb.AppendLine("DTMF_DEC_TYPE = INTERNAL");
      sb.AppendLine("DTMF_MUTING = 1");
      sb.AppendLine("DTMF_HANGTIME = 40");
      sb.AppendLine("1750_MUTING = 1");
      sb.AppendLine("[Tx1]");
      sb.AppendLine("TYPE = Local");
      sb.AppendLine("AUDIO_DEV = udp:127.0.0.1:10000");
      sb.AppendLine("AUDIO_CHANNEL = 0");
      sb.AppendLine("PTT_TYPE = Dummy");
      sb.AppendLine("#GPIO_PATH=/sys/class/gpio");
      sb.AppendLine("#PTT_PIN=gpio7");
      sb.AppendLine("TIMEOUT = 300");
      sb.AppendLine("TX_DELAY = 900");
      sb.AppendLine("PREAMP = 0");
      sb.AppendLine("CTCSS_FQ = 71.9");
      sb.AppendLine("CTCSS_LEVEL = 9");
      sb.AppendLine("PREEMPHASIS = 0");
      sb.AppendLine("DTMF_TONE_LENGTH = 100");
      sb.AppendLine("DTMF_TONE_SPACING = 50");
      sb.AppendLine("DTMF_DIGIT_PWR = -15");
      sb.AppendLine("[ReflectorLogic]");
      sb.AppendLine("TYPE = Reflector");
      sb.AppendLine("AUDIO_CODEC = OPUS");
      sb.AppendLine("JITTER_BUFFER_DELAY = 2");
      sb.AppendLine("CALLSIGN = CALLSIGN");
      sb.AppendLine("HOST = HOST");
      sb.AppendLine("AUTH_KEY = AUTH_KEY");
      sb.AppendLine("PORT = PORT");

      builder.HasData(new SvxlinkManagerParameter
      {
        Id = 1,
        Key = "default.svxlink.conf",
        Value = sb.ToString()
      });

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

      builder.HasData(new SvxlinkManagerParameter
      {
        Id = 2,
        Key = "default.echolink.conf",
        Value = sb.ToString()
      });
    }
  }
}