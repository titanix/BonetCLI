using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Language.General;

namespace BonetIDE
{
    class WiktionaryDataLoader
    {
        internal ICharacterStore LoadWiktionaryFile(string path)
        {
            CharacterStore result = new();

            foreach (string line in File.ReadAllLines(path))
            {
                string[] parts = line.Split('/');
                CodePointIndexedString hannom = new(parts[0]);
                string[] quocngu = parts[1].Split(' ');

                for (int i = 0; i < hannom.Length; i++)
                {
                    string @char = hannom.AtIndex(i);
                    result.AddCharacter(hannom: @char, reading:quocngu[i]);
                }
            }

            return result;
        }
    }
}