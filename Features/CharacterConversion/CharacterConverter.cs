using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonetIDE
{
    class CharacterConverter
    {
        Dictionary<string, string> conversions = new()
        {
            ["oo"] = "ơ",
            ["uu"] = "ư",
            ["aa"] = "ă",
        };

        internal string Convert(string str)
        {
            return ConvertTone(ConvertLetters(str));
        }

        private string ConvertLetters(string str)
        {
            string result = str;

            foreach (KeyValuePair<string, string> pair in conversions)
            {
                result = result.Replace(pair.Key, pair.Value);
            }

            return result;
        }

        private string ConvertTone(string str)
        {
            string last = str.Last().ToString();

            char[] vowels = new char[]
            {
                'i', 'y', 'ê', 'e', 'ư', 'u',
                'ô', 'ơ', 'o', 'ă', 'â', 'a'
            };

            char[] toneSymbols = new char[] {
                '\u0300', // COMBINING GRAVE ACCENT
                '\u0301', // COMBINING ACUTE ACCENT
                '\u0323', // COMBINING DOT BELOW
                '\u0309', // COMBINING HOOK ABOVE
                '\u0303', // COMBINING TILDE
            };

            if (int.TryParse(last, out int toneNumber))
            {
                if (toneNumber >= 2 && toneNumber <= 6)
                {
                    toneNumber -= 2;
                    char diacritic = toneSymbols[toneNumber];
                    str = str.Substring(0, str.Length - 1);
                    str = str.Normalize(NormalizationForm.FormC);

                    VietnameseSyllableParser parser = new();
                    var syllable = parser.Parse(str);
                    syllable.Nucleus = syllable.Nucleus + diacritic;

                    return $"{syllable.Initial}{syllable.Medial}{syllable.Nucleus}{syllable.Coda}";
                }
            }

            return str;
        }
    }
}