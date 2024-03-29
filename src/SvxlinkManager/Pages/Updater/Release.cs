﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Updater
{
  public class Release
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }

    [JsonPropertyName("prerelease")]
    public bool Prerelease { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }

    [JsonIgnore]
    public string Created => DateTime.Parse(CreatedAt).ToString("dd MMMM yyyy HH:mm");

    [JsonPropertyName("assets_url")]
    public string AssetsUrl { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("assets")]
    public List<Asset> Assets { get; set; }

    public bool IsValid()
    {
      try
      {
        if (Package != null && PackageCheckSum != null && Updater != null && UpdaterCheckSum != null)
          return true;
      }
      catch (Exception e)
      {
      }

      return false;
    }

    [JsonIgnore]
    public Asset Package => Assets.SingleOrDefault(a => a.Name.StartsWith("svxlinkmanager-") && a.Name.EndsWith(".zip"));

    [JsonIgnore]
    public Asset Updater => Assets.SingleOrDefault(a => a.Name.StartsWith("updater-") && a.Name.EndsWith(".sh"));

    [JsonIgnore]
    public Asset PackageCheckSum => Assets.SingleOrDefault(a => a.Name.StartsWith("svxlinkmanager-") && a.Name.EndsWith(".zip.sha"));

    [JsonIgnore]
    public Asset UpdaterCheckSum => Assets.SingleOrDefault(a => a.Name.StartsWith("updater-") && a.Name.EndsWith(".sh.sha"));

    [JsonIgnore]
    public Asset Image => Assets.SingleOrDefault(a => a.Name.EndsWith(".img.7z"));

    [JsonIgnore]
    public int Major => int.Parse(TagName.Split('.').First());
  }

  public class Asset
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("browser_download_url")]
    public string DownloadUrl { get; set; }

    [JsonPropertyName("download_count")]
    public int DownloadCount { get; set; }
  }
}