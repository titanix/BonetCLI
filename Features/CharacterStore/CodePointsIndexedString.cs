using System;
using System.Collections.Generic;
using System.Text;

namespace Language.General
{
    public class CodePointIndexedString
    {
        List<string> codePoints = new List<string>();

        public CodePointIndexedString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (str[i] > 0xD800 && str[i] < 0xE000)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(str[i]);
                    sb.Append(str[i + 1]);
                    string newString = sb.ToString();
                    codePoints.Add(newString);
                    i++;
                }
                else
                {
                    codePoints.Add(str[i].ToString());
                }
            }
        }

        public int Length
        {
            get
            {
                return codePoints.Count;
            }
        }

        public string AtIndex(int i)
        {
            if (i < 0 || i > codePoints.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return codePoints[i];
        }

        public IEnumerable<string> Iterator { get { return codePoints.AsReadOnly(); } }

        public string Substring(int start, int length)
        {
            if (start < 0 || start + length > codePoints.Count)
            {
                throw new ArgumentException();
            }

            StringBuilder result = new StringBuilder();
            for (int i = start; i < start + length; i++)
            {
                result.Append(codePoints[i]);
            }
            return result.ToString();
        }

        public override string ToString()
        {
            return string.Join("", codePoints);
        }
    }
}
