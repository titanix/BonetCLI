using System;
using System.IO;
using System.Text.RegularExpressions;

namespace BonetIDE
{
    class BonetDictionary : IBonetDictionary, IDisposable
    {
        StreamWriter file;
        Regex regex = new("");
        VietnameseSyllableParser parser = new();

        internal BonetDictionary(string path)
        {
            FileStream stream = File.Open(path, FileMode.Append);
            file = new StreamWriter(stream);
            file.AutoFlush = true;
        }

        public void AddCompound(string word, string reading)
        {
            file.WriteLine($"{word} {reading}");
        }

        public string AddContent(string content)
        {
            file.WriteLine(content);
            return "Content added.";
            /*
            var result = Validate(content);
            if (result.Success)
            {
                file.WriteLine(content);
                return "Content added to the dictionary.";
            }
            else
            {
                return result.ErrorMessage;
            }
            */
        }

        /*
         * Input must be of the form:
         * quốc ngữ 漢字
         */
        private (bool Success, string ErrorMessage) Validate(string input)
        {
            input = input.Trim();

            if (string.IsNullOrEmpty(input))
                return (false, "Empty string.");

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if(parts.Length <= 1)
            return (false, "Missing data.");

            foreach (string part in parts.DropLast(1))
            {
                if (parser.Parse(part) == null)
                    return (false, "Non conforming quốc ngữ syllable.");
            }

            // TODO : validate han tữ part

            return (true, "");
        }

        public void AddHeadword(string character, string reading)
        {
            file.WriteLine($"{character} {reading}");
        }

        public void Dispose()
        {
            file.Flush();
            file.Close();
            file.Dispose();
        }
    }
}