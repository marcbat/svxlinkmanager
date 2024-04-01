using System;
using System.ComponentModel.DataAnnotations;

namespace SvxlinkManager.Models
{
    public class Reflector : IModelEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Config { get; set; }

        public bool Enable { get; set; }

        public static implicit operator Reflector(Domain.Entities.Reflector v)
        {
            return new Reflector
            {
                Id = v.Id,
                Name = v.Name,
                Config = v.Config,
                Enable = v.Enable
            };
        }
    }
}