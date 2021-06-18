using System.IO;

using Language.General;

namespace BonetIDE
{
    class WiktionaryDataLoader : ICharacterStoreLoader
    {
        public ICharacterStore LoadFile(string path)
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
                    result.AddCharacter(hannom: @char, reading: quocngu[i]);
                }
            }

            return result;
        }
    }
}