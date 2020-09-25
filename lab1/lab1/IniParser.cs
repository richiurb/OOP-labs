using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace lab1
{
    internal static class IniParser
    {
        private static readonly Regex _sectionRegex = new Regex(@"^\[([A-Za-z0-9_]+)\]$", RegexOptions.Compiled);
        private static readonly Regex _parameterRegex = new Regex(@"^([A-Za-z0-9_]+)\s*=\s*([^\s=;]+)\s*(?>;.*)?$", RegexOptions.Compiled);

        public static Dictionary<string, Dictionary<string, string>> Parse(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("File not found", filepath);
            }

            return ParseFile(filepath);
        }

        private static Dictionary<string, Dictionary<string, string>> ParseFile(string filepath)
        {
            var sectionsDictionary = new Dictionary<string, Dictionary<string, string>>();

            (string name, Dictionary<string, string> parameters) currentSection = (null, null);
            string line = null;

            using (var sr = new StreamReader(new FileStream(filepath, FileMode.Open, FileAccess.Read)))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith(';'))
                    {
                        continue;
                    }

                    if (line.StartsWith('['))
                    {
                        var sectionName = ParseSection(line);
                        var parametersDictionary = new Dictionary<string, string>();

                        if (!sectionsDictionary.TryAdd(sectionName, parametersDictionary))
                        {
                            throw new InvalidOperationException($"Section \"{sectionName}\" already exists");
                        }

                        currentSection = (sectionName, parametersDictionary);
                    }
                    else
                    {
                        if (currentSection.name == null)
                        {
                            throw new InvalidOperationException("Section must be defined before any parameters");
                        }

                        var parameter = ParseParameter(line);

                        if (!currentSection.parameters.TryAdd(parameter.key, parameter.val))
                        {
                            throw new InvalidOperationException($"Parameter \"{parameter.key}\" in section \"{currentSection}\" already exists");
                        }
                    }
                }
            }

            return sectionsDictionary;
        }

        private static string ParseSection(string line)
        {
            var match = _sectionRegex.Match(line);

            if (!match.Success)
            {
                throw new FormatException($"Section \"{line}\" has invalid format");
            }

            return match.Groups[1].Value;     
        }

        private static (string key, string val) ParseParameter(string line)
        {
            var match = _parameterRegex.Match(line);

            if (!match.Success)
            {
                throw new FormatException($"Line \"{line}\" has invalid format");
            }

            var key = match.Groups[1].Value;
            var val = match.Groups[2].Value;

            return (key, val);
        }
    }
}
