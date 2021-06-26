using System.IO;
using System.Linq;
using System.Collections.Generic;

using Language.General;

namespace BonetIDE
{
    class BonetDictionaryLoader : ICharacterStoreLoader
    {
        public ICharacterStore LoadFile(string path)
        {
            CharacterStore result = new();

            foreach (string line in File.ReadAllLines(path))
            {
                foreach (CharacterReading cr in GetCharacterReadings(line))
                {
                    result.AddCharacter(hannom: cr.Character, reading: cr.Reading);
                }
            }

            return result;
        }

        public static List<CharacterReading> GetCharacterReadings(string line)
        {
            List<CharacterReading> result = new();

            string[] parts = line.Split(' ');
            CodePointIndexedString hannom = new(parts.Last());

            for (int i = 0; i < hannom.Length; i++)
            {
                try
                {
                    string @char = hannom.AtIndex(i);
                    result.Add(new CharacterReading(@char, parts[i]));
                }
                catch { }
            }

            return result;
        }
    }
}