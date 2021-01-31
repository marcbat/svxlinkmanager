﻿using Microsoft.AspNetCore.Components.Forms;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SvxlinkManager.Models
{
  public abstract class Channel : IModelEntity
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Host { get; set; }

    [Required]
    public string CallSign { get; set; }

    [Required]
    public bool IsDefault { get; set; }

    [Required]
    public bool IsTemporized { get; set; }

    [Required]
    public int TimerDelay { get; set; } = 180;

    public string TrackerUrl { get; set; }

    public List<ScanProfile> ScanProfiles { get; set; }

    public int Dtmf { get; set; }

    public override bool Equals(object obj)
    {
      return Id == ((Channel)obj).Id;
    }

    [NotMapped]
    [FileValidation(new[] { ".wav" })]
    public IBrowserFile Sound { get; set; }

    public string SoundName { get; set; }

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

    [NotMapped]
    public abstract Dictionary<string, string> TrackProperties { get; }
  }
}