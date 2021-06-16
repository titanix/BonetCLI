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
                '\u0303', // COMBINING TILDE
                '\u0306', // COMBINING BREVE
                '\u0309', // COMBINING HOOK ABOVE
                '\u031B', // COMBINING HORN
                '\u0323', // COMBINING DOT BELOW

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