using System.ComponentModel.DataAnnotations;

namespace SvxlinkManager.Models
{
    public class EcholinkChannel : Channel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string SysopName { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int MaxQso { get; set; } = 1;

        [Required]
        public string Description { get; set; }

        public static implicit operator EcholinkChannel(Domain.Entities.EcholinkChannel v)
        {
            return new EcholinkChannel
            {
                Id = v.Id,
                Name = v.Name,
                Host = v.Host,
                CallSign = v.CallSign,
                Password = v.Password,
                SysopName = v.SysopName,
                Location = v.Location,
                MaxQso = v.MaxQso,
                Description = v.Description
            };
        }
    }
}