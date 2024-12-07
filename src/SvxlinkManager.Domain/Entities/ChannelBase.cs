using LanguageExt;
using LanguageExt.Common;

namespace SvxlinkManager.Domain.Entities
{
    public abstract class ChannelBase : Entity<Guid>
    {
        private string name;

        protected ChannelBase()
        {
            
        }
        public ChannelBase(Guid id, string name) : base(id)
        {
            this.name = name;
        }

        public string Name => name;

        protected static Validation<Error, string> ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Error.New("Le nom du channel est obligatoire.");

            return name;
        }

        public Validation<Error, string> SetName(string name)
        {
            return ValidateName(name).Map(v => this.name = v);
        }
    }
}
