using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils
{
    public class RadioProfilService
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISa818Service sa818Service;

        public RadioProfilService(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISa818Service sa818Service)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.sa818Service = sa818Service;
        }

        internal Validation<Error, Guid> AddRadioProfil(Guid configId,
                                   string name,
                                   string rxFrequency,
                                   string txFrequency,
                                   string squelch,
                                   string txCtcss,
                                   string rxCtCss,
                                   string volume,
                                   string preEmph,
                                   string highPass,
                                   string lowPass,
                                   string squelchDetection)
        {
            return from config in svxlinkManagerConfigRepository.GetConfig(configId)
                   from radioProfil in RadioProfil.Create(Guid.NewGuid(), name, rxFrequency, txFrequency, squelch, txCtcss, rxCtCss, volume, preEmph, highPass, lowPass, squelchDetection)
                   from _ in config.AddRadioProfil(radioProfil)
                   from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                   select radioProfil.Id;
        }

        internal Validation<Error, Guid> UpdateRadioProfil(Guid configId,
                                   Guid profilId,
                                   string name,
                                   string rxFrequency,
                                   string txFrequency,
                                   string squelch,
                                   string txCtcss,
                                   string rxCtCss,
                                   string volume,
                                   string preEmph,
                                   string highPass,
                                   string lowPass,
                                   string squelchDetection)
        {
            return from config in svxlinkManagerConfigRepository.GetConfig(configId)
                   from _ in config.DeleteRadioProfil(profilId)
                   from radioProfil in RadioProfil.Create(profilId, name, rxFrequency, txFrequency, squelch, txCtcss, rxCtCss, volume, preEmph, highPass, lowPass, squelchDetection)
                   from __ in config.AddRadioProfil(radioProfil)
                   from ___ in svxlinkManagerConfigRepository.UpdateAsync(config)
                   select radioProfil.Id;
        } 

        internal Validation<Error, Guid> DeleteRadioProfil(Guid configId, Guid profilId)
        {
            return from config in svxlinkManagerConfigRepository.GetConfig(configId)
                   from _ in config.DeleteRadioProfil(profilId)
                   from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                   select profilId;
        }

        internal Validation<Error, Guid> ApplyRadioProfil(Guid configId, Guid profilId)
        {
            return from config in svxlinkManagerConfigRepository.GetConfig(configId)
                   from radioProfile in config.GetRadioProfil(profilId)
                   from _ in WriteRadioProfil(radioProfile)
                   from __ in config.SetActiveRadioProfile(radioProfile.Id)
                   from ___ in svxlinkManagerConfigRepository.UpdateAsync(config)
                   select config.Id;
        }

        private Validation<Error, Unit> WriteRadioProfil(RadioProfil radioProfil)
        {
            if (radioProfil.HasSa818)
                return sa818Service.WriteRadioProfile(radioProfil);

            return Unit.Default;
        }
    }
}
