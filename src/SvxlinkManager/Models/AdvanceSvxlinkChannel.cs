using System;

namespace SvxlinkManager.Models
{
    public class AdvanceSvxlinkChannel : ManagedChannel
    {
        public string SvxlinkConf { get; set; }

        public string ModuleDtmfRepeater { get; set; }

        public string ModuleEchoLink { get; set; }

        public string ModuleFrn { get; set; }

        public string ModuleHelp { get; set; }

        public string ModuleMetarInfo { get; set; }

        public string ModuleParrot { get; set; }

        public string ModulePropagationMonitor { get; set; }

        public string ModuleSelCallEnc { get; set; }

        public string ModuleTclVoiceMail { get; set; }

        public string ModuleTrx { get; set; }

        public static implicit operator AdvanceSvxlinkChannel(Domain.Entities.AdvanceSvxlinkChannel v)
        {
            return new AdvanceSvxlinkChannel
            {
                Id = v.Id,
                Name = v.Name,
                SvxlinkConf = v.SvxlinkConf,
                ModuleDtmfRepeater = v.ModuleDtmfRepeater,
                ModuleEchoLink = v.ModuleEchoLink,
                ModuleFrn = v.ModuleFrn,
                ModuleHelp = v.ModuleHelp,
                ModuleMetarInfo = v.ModuleMetarInfo,
                ModuleParrot = v.ModuleParrot,
                ModulePropagationMonitor = v.ModulePropagationMonitor,
                ModuleSelCallEnc = v.ModuleSelCallEnc,
                ModuleTclVoiceMail = v.ModuleTclVoiceMail,
                ModuleTrx = v.ModuleTrx
            };
           
        }
    }
}