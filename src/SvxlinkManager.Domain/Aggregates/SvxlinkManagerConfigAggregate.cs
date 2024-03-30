using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Aggregates
{

    public class SvxlinkManagerConfigAggregate : AggregateRoot
    {
        private List<SvxlinkChannel> svxlinkChannels = [];
        private List<EcholinkChannel> echolinkChannels = [];
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

        public IReadOnlyCollection<SvxlinkChannel> SvxlinkChannels{
            get=>  svxlinkChannels.AsReadOnly(); 
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
            get=>  echolinkChannels.AsReadOnly(); 
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
            get=>  reflectors.AsReadOnly(); 
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
            get=>  radioProfils.AsReadOnly(); 
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
    }
}
