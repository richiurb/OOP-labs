using System;
using System.Collections.Generic;

namespace lab1
{
    public class IniFile
    {
        private readonly Dictionary<string, Dictionary<string, string>> _iniFile;

        public IniFile(string filepath)
        {
            _iniFile = IniParser.Parse(filepath);
        }

        public string GetParameter(string sectionName, string parameterName)
        {
            if (!_iniFile.TryGetValue(sectionName, out var parametersDictionary))
            {
                throw new InvalidOperationException($"Section \"{sectionName}\" not found");
            }

            if (!parametersDictionary.TryGetValue(parameterName, out var parameterValue))
            {
                throw new InvalidOperationException($"Parameter \"{parameterName}\" in section \"{sectionName}\" not found");
            }

            return parameterValue;
        }
    }
}
