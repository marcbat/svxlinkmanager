using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Interfaces
{
    public interface IIniService
    {
        string? FindConfigValueInFile(string filePath, string key);

        string? FindConfigValueInString(string ini, string key);

        void ReplaceConfig(string filePath, Dictionary<string, Dictionary<string, string>> parameters);
    }
}
