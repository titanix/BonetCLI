using System;
using System.IO;

namespace BonetIDE
{
    class BonetDictionary : IBonetDictionary, IDisposable
    {
        StreamWriter file;

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