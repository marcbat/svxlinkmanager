using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class AdvanceSvxlinkChannel : ManagedChannel
    {
        public AdvanceSvxlinkChannel(Guid id,
                                    string name,
                                    string svxlinkConf,
                                    string moduleDtmfRepeater,
                                    string moduleEchoLink,
                                    string moduleFrn,
                                    string moduleHelp,
                                    string moduleMetarInfo,
                                    string moduleParrot,
                                    string modulePropagationMonitor,
                                    string moduleSelCallEnc,
                                    string moduleTclVoiceMail,
                                    string moduleTrx) : base(id, name)
        {

            SvxlinkConf = svxlinkConf;
            ModuleDtmfRepeater = moduleDtmfRepeater;
            ModuleEchoLink = moduleEchoLink;
            ModuleFrn = moduleFrn;
            ModuleHelp = moduleHelp;
            ModuleMetarInfo = moduleMetarInfo;
            ModuleParrot = moduleParrot;
            ModulePropagationMonitor = modulePropagationMonitor;
            ModuleSelCallEnc = moduleSelCallEnc;
            ModuleTclVoiceMail = moduleTclVoiceMail;
            ModuleTrx = moduleTrx;
        }


        public string SvxlinkConf { get; }
        public string ModuleDtmfRepeater { get; }
        public string ModuleEchoLink { get; }
        public string ModuleFrn { get; }
        public string ModuleHelp { get; }
        public string ModuleMetarInfo { get; }
        public string ModuleParrot { get; }
        public string ModulePropagationMonitor { get; }
        public string ModuleSelCallEnc { get; }
        public string ModuleTclVoiceMail { get; }
        public string ModuleTrx { get; }
    }
}
