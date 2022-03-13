using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Infrastructure.Persistence
{
    public partial class CorrectParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 1,
                column: "Value",
                value: "[GLOBAL]\r\nLOGICS = LOGICS\r\nCFG_DIR = svxlink.d\r\nTIMESTAMP_FORMAT =% c\r\nCARD_SAMPLE_RATE = 16000\r\nCARD_CHANNELS = 1\r\nLINKS = ALLlink\r\n[SimplexLogic]\r\nTYPE = Simplex\r\nRX = Rx1\r\nTX = Tx1\r\nMODULES = MODULES\r\nCALLSIGN = REPORTCALLSIGN\r\nSHORT_IDENT_INTERVAL = 15\r\nLONG_IDENT_INTERVAL = 60\r\nIDENT_ONLY_AFTER_TX = 10\r\nEXEC_CMD_ON_SQL_CLOSE = 500\r\nEVENT_HANDLER =/usr/share/svxlink/events.tcl\r\nDEFAULT_LANG = fr_FR\r\nRGR_SOUND_ALWAYS = 1\r\nRGR_SOUND_DELAY = 0\r\nREPORT_CTCSS = REPORT_CTCSS\r\nTX_CTCSS = ALWAYS\r\nMACROS = Macros\r\nFX_GAIN_NORMAL = 0\r\nFX_GAIN_LOW = -12\r\nACTIVATE_MODULE_ON_LONG_CMD = 10:PropagationMonitor\r\nMUTE_RX_ON_TX = 1\r\nDTMF_CTRL_PTY =/tmp/dtmf_uhf\r\n[ALLlink]\r\nCONNECT_LOGICS = SimplexLogic:434MHZ:945, ReflectorLogic\r\nDEFAULT_ACTIVE = 1\r\nTIMEOUT = 0\r\n\r\n[Rx1]\r\nTYPE = Local\r\nAUDIO_DEV = alsa:plughw:0\r\nAUDIO_CHANNEL = 0\r\nSQL_DET = CTCSS\r\nSQL_START_DELAY = 500\r\nSQL_DELAY = 100\r\nSQL_HANGTIME = 20\r\nSQL_EXTENDED_HANGTIME = 1000\r\nSQL_EXTENDED_HANGTIME_THRESH = 13\r\nSQL_TIMEOUT = 600\r\nVOX_FILTER_DEPTH = 300\r\nVOX_THRESH = 1000\r\nCTCSS_MODE = 2\r\nCTCSS_FQ = 71.9\r\nCTCSS_SNR_OFFSET = 0\r\nCTCSS_OPEN_THRESH = 15\r\nCTCSS_CLOSE_THRESH = 9\r\nCTCSS_BPF_LOW = 60\r\nCTCSS_BPF_HIGH = 260\r\nGPIO_PATH=/sys/class/gpio\r\nGPIO_SQL_PIN=gpio10\r\nDEEMPHASIS = 0\r\nSQL_TAIL_ELIM = 0\r\nPREAMP = -4\r\nPEAK_METER = 1\r\nDTMF_DEC_TYPE = INTERNAL\r\nDTMF_MUTING = 1\r\nDTMF_HANGTIME = 40\r\n1750_MUTING = 1\r\n\r\n[Tx1]\r\nTYPE = Local\r\nAUDIO_DEV = alsa:plughw:0\r\nAUDIO_CHANNEL = 0\r\nPTT_TYPE = Dummy\r\nGPIO_PATH=/sys/class/gpio\r\nPTT_PIN=gpio7\r\nTIMEOUT = 300\r\nTX_DELAY = 900\r\nPREAMP = 0\r\nCTCSS_FQ = 71.9\r\nCTCSS_LEVEL = 9\r\nPREEMPHASIS = 0\r\nDTMF_TONE_LENGTH = 100\r\nDTMF_TONE_SPACING = 50\r\nDTMF_DIGIT_PWR = -15\r\n\r\n[ReflectorLogic]\r\nTYPE = Reflector\r\nAUDIO_CODEC = OPUS\r\nJITTER_BUFFER_DELAY = 2\r\nCALLSIGN = CALLSIGN\r\nHOST = HOST\r\nAUTH_KEY = AUTH_KEY\r\nPORT = PORT\r\n");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 1,
                column: "Value",
                value: "[GLOBAL]\r\nLOGICS = LOGICS\r\nCFG_DIR = svxlink.d\r\nTIMESTAMP_FORMAT =% c\r\nCARD_SAMPLE_RATE = 16000\r\nCARD_CHANNELS = 1\r\nLINKS = ALLlink\r\n[SimplexLogic]\r\nTYPE = Simplex\r\nRX = Rx1\r\nTX = Tx1\r\nMODULES = MODULES\r\nCALLSIGN = REPORTCALLSIGN\r\nSHORT_IDENT_INTERVAL = 15\r\nLONG_IDENT_INTERVAL = 60\r\nIDENT_ONLY_AFTER_TX = 10\r\nEXEC_CMD_ON_SQL_CLOSE = 500\r\nEVENT_HANDLER =/ usr / share / svxlink / events.tcl\r\nDEFAULT_LANG = fr_FR\r\nRGR_SOUND_ALWAYS = 1\r\nRGR_SOUND_DELAY = 0\r\nREPORT_CTCSS = REPORT_CTCSS\r\nTX_CTCSS = ALWAYS\r\nMACROS = Macros\r\nFX_GAIN_NORMAL = 0\r\nFX_GAIN_LOW = -12\r\nACTIVATE_MODULE_ON_LONG_CMD = 10:PropagationMonitor\r\nMUTE_RX_ON_TX = 1\r\nDTMF_CTRL_PTY =/ tmp / dtmf_uhf\r\n[ALLlink]\r\nCONNECT_LOGICS = SimplexLogic:434MHZ:945, ReflectorLogic\r\nDEFAULT_ACTIVE = 1\r\nTIMEOUT = 0\r\n[Rx1]\r\nTYPE = Local\r\nAUDIO_DEV = udp:127.0.0.1:10000\r\nAUDIO_CHANNEL = 0\r\nSQL_DET = CTCSS\r\nSQL_START_DELAY = 500\r\nSQL_DELAY = 100\r\nSQL_HANGTIME = 20\r\nSQL_EXTENDED_HANGTIME = 1000\r\nSQL_EXTENDED_HANGTIME_THRESH = 13\r\nSQL_TIMEOUT = 600\r\nVOX_FILTER_DEPTH = 300\r\nVOX_THRESH = 1000\r\nCTCSS_MODE = 2\r\nCTCSS_FQ = 71.9\r\nCTCSS_SNR_OFFSET = 0\r\nCTCSS_OPEN_THRESH = 15\r\nCTCSS_CLOSE_THRESH = 9\r\nCTCSS_BPF_LOW = 60\r\nCTCSS_BPF_HIGH = 260\r\n#GPIO_PATH=/sys/class/gpio\r\n#GPIO_SQL_PIN=gpio10\r\nDEEMPHASIS = 0\r\nSQL_TAIL_ELIM = 0\r\nPREAMP = -4\r\nPEAK_METER = 1\r\nDTMF_DEC_TYPE = INTERNAL\r\nDTMF_MUTING = 1\r\nDTMF_HANGTIME = 40\r\n1750_MUTING = 1\r\n[Tx1]\r\nTYPE = Local\r\nAUDIO_DEV = udp:127.0.0.1:10000\r\nAUDIO_CHANNEL = 0\r\nPTT_TYPE = Dummy\r\n#GPIO_PATH=/sys/class/gpio\r\n#PTT_PIN=gpio7\r\nTIMEOUT = 300\r\nTX_DELAY = 900\r\nPREAMP = 0\r\nCTCSS_FQ = 71.9\r\nCTCSS_LEVEL = 9\r\nPREEMPHASIS = 0\r\nDTMF_TONE_LENGTH = 100\r\nDTMF_TONE_SPACING = 50\r\nDTMF_DIGIT_PWR = -15\r\n[ReflectorLogic]\r\nTYPE = Reflector\r\nAUDIO_CODEC = OPUS\r\nJITTER_BUFFER_DELAY = 2\r\nCALLSIGN = CALLSIGN\r\nHOST = HOST\r\nAUTH_KEY = AUTH_KEY\r\nPORT = PORT\r\n");
        }
    }
}