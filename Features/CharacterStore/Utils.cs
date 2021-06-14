using System.Text;
using System.Linq;

namespace BonetIDE
{
    public static class Utils
    {
        public static string StripAccents(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormD);

            char[] accents = new char[] {
                '\u0300', // COMBINING GRAVE ACCENT
                '\u0301', // COMBINING ACUTE ACCENT
                '\u0302', // COMBINING CIRCUMFLEX ACCENT 
                '\u0303',
                '\u0306',
                '\u0309',
                '\u031B',
                '\u0323',

                '\u0341', // COMBINING ACUTE TONE MARK
                '\u0340', // COMBINING GRAVE TONE
            };

            string result = "";
            foreach (char c in normalized)
            {
                if (!accents.Contains(c))
                {
                    result += c;
                }
            }

            return result;
        }
    }
}