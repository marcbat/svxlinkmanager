using Microsoft.AspNetCore.Components.Forms;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace SvxlinkManager.Models
{
    public abstract class ManagedChannel : ChannelBase
    {
        public int Dtmf { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        public bool IsTemporized { get; set; }

        [Required]
        public int TimerDelay { get; set; } = 180;

        public string TrackerUrl { get; set; }

        [NotMapped]
        [FileValidation(new[] { ".wav" })]
        public IBrowserFile SoundBrowserFile { get; set; }

        public Sound Sound { get; set; }

        private class FileValidationAttribute : ValidationAttribute
        {
            public FileValidationAttribute(string[] allowedExtensions)
            {
                AllowedExtensions = allowedExtensions;
            }

            private string[] AllowedExtensions { get; }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                    return ValidationResult.Success;

                var file = (IBrowserFile)value;

                var extension = Path.GetExtension(file.Name);

                if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                {
                    return new ValidationResult($"File must have one of the following extensions: {string.Join(", ", AllowedExtensions)}.", new[] { validationContext.MemberName });
                }

                return ValidationResult.Success;
            }
        }

        public static implicit operator ManagedChannel(Domain.Entities.ManagedChannel managedChannel)
        {
            switch (managedChannel)
            {
                case Domain.Entities.SvxlinkChannel derivedChannel1:
                    return derivedChannel1;

               
                default:
                    throw new NotSupportedException();
            }

            
        }   
    }
}