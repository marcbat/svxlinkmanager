using LanguageExt;
using LanguageExt.Common;

namespace SvxlinkManager.Domain.Entities
{
    public abstract class Channel : ManagedChannel
    {
        private string host;
        private string callSign;

        protected Channel()
        {
            
        }

        protected Channel(Guid id, string name, string host, string callSign) : base(id, name)
        {
            this.callSign = callSign;
            this.host = host;
        }

        public string Host
        {
            get => host;
            protected set => host = value;
        }

        public static Validation<Error, string> ValidateHost(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                return Error.New("Le nom du host ne peut pas être vide.");

            return host;
        }

        public Validation<Error, string> SetHost(string host)
        {
            return ValidateHost(host).Map(v => this.host = v);
        }

        public string CallSign {
            get => callSign;
            protected set => callSign = value;
        }

        public static Validation<Error, string> ValidateCallSign(string callSign)
        {
            if (string.IsNullOrWhiteSpace(callSign))
                return Error.New("Le nom du callSign ne peut pas être vide.");

            return callSign;
        }

        public Validation<Error, string> SetCallSign(string callSign)
        {
            return ValidateCallSign(callSign).Map(v => this.callSign = v);
        }
    }
}
