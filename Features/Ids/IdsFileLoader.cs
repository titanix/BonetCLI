using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BonetIDE
{
    class IdsFileLoader
    {
        internal Dictionary<string, string> LoadFile(string path)
        {
            Dictionary<string, string> result = new();

            foreach (string line in File.ReadLines(path))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line[0] == '#')
                    continue;

                string[] parts = line.Split('\t');
                string character = parts[1];

                foreach (string decomposition in parts.Skip(2))
                {
                    string cleaned = RemoveBracketInfo(decomposition);

                    if (result.ContainsKey(cleaned))
                        continue;

                    result.Add(cleaned, character);
                }
            }

            return result;

            string RemoveBracketInfo(string str)
            {
                int index = str.IndexOf('[');

                if (index > 0)
                {
                    return str.Substring(0, index);
                }

                return str;
            }
        }
    }
}