using LanguageExt;
using LanguageExt.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class EcholinkChannel : Channel
    {
        public EcholinkChannel() 
        {
            
        }

        internal EcholinkChannel(Guid id, string name, string host, string callSign, string password, string sysopName, string location, int maxQso, string description) : base(id, name, host, callSign)
        {
            Password = password;
            SysopName = sysopName;
            Location = location;
            MaxQso = maxQso;
            Description = description;
        }

        public static Validation<Error, EcholinkChannel> Create(Guid id, string name, string host, string callSign, string password, string sysopName, string location, int maxQso, string description)
        {
            return (ValidateName(name), ValidateHost(host), ValidatePassword(password), ValidateSysopName(sysopName), ValidateLocation(location), ValidateMaxQso(maxQso))
                .Apply((vname, vhost, vpassword, vsysopName, vlocation, vmaxQso) => new EcholinkChannel(id, vname, vhost, callSign, vpassword, vsysopName, vlocation, vmaxQso, description));
        }

        public string Password { get; protected set; }

        public static Validation<Error, string> ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Error.New("Le mot de passe ne peut pas être vide.");

            return password;
        }

        public Validation<Error, string> SetPassword(string password)
        {
            return ValidatePassword(password).Map(v => Password = v);
        }

        public string SysopName { get; protected set; }

        public static Validation<Error, string> ValidateSysopName(string sysopName)
        {
            if (string.IsNullOrWhiteSpace(sysopName))
                return Error.New("Le nom du sysop ne peut pas être vide.");

            return sysopName;
        }

        public Validation<Error, string> SetSysopName(string sysopName)
        {
            return ValidateSysopName(sysopName).Map(v => SysopName = v);
        }

        public string Location { get; protected set; }

        public static Validation<Error, string> ValidateLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return Error.New("La location ne peut pas être vide.");

            return location;
        }

        public Validation<Error, string> SetLocation(string location)
        {
            return ValidateLocation(location).Map(v => Location = v);
        }

        public int MaxQso { get; protected set; }

        public static Validation<Error, int> ValidateMaxQso(int maxQso)
        {
            if (maxQso < 0)
                return Error.New("Le nombre maximum de QSO ne peut pas être inférieur à 0.");

            return maxQso;
        }

        public Validation<Error, int> SetMaxQso(int maxQso)
        {
            return ValidateMaxQso(maxQso).Map(v => MaxQso = v);
        }

        public string? Description { get; set; }
    }
}
