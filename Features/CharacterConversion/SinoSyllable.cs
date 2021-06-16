using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SiniticDidactics.Common
{
    public class SinoSyllable : ICloneable
    {
        public string Initial { get; set; }
        public string Medial { get; set; }
        public string Nucleus { get; set; }
        public string Coda { get; set; }
        public string Tone { get; set; }

        public string Language { get; set; }

        public Dictionary<string, string> Metadata { get; } = new Dictionary<string, string>();

        public SinoSyllable() { }

        public SinoSyllable(string initial, string medial, string nucleus, string coda, string tone)
        {
            Initial = initial;
            Medial = medial;
            Nucleus = nucleus;
            Coda = coda;
            Tone = tone;
        }

        public object Clone()
        {
            SinoSyllable result = new SinoSyllable(Initial, Medial, Nucleus, Coda, Tone);
            result.Language = Language;

            foreach (KeyValuePair<string, string> pair in Metadata)
            {
                result.Metadata.Add(pair.Key, pair.Value);
            }

            return result;
        }

        public override string ToString()
        {
            string meta = String.Join(",", Metadata.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"[{Initial}/{Medial}/{Nucleus}/{Coda},{Tone},{Language},{{{meta}}}]";
        }

        public string ToStringV3()
        {
            if (Metadata.Count == 0)
                return $"[{Initial}/{Medial}/{Nucleus}/{Coda},{Tone}]";

            string meta = String.Join(",", Metadata.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"[{Initial}/{Medial}/{Nucleus}/{Coda},{Tone},{{{meta}}}]";
        }

        /// <summary>
        /// Returns a string representing the syllable. By default, return the short form, which looks like:
        /// <example>[x/u/e/i,4]</example>
        /// The short from does not contain language and metadata information.
        ///
        /// If the shortForm parameter is set to true, the result will contains all information in the syllable,
        /// allowing the full deserialization using <see cref="TryParse"/> static method. This is equivalent to
        /// calling <see cref="ToString"/> method directly.
        /// </summary>
        public string ToShortString()
        {
            return $"[{Initial}/{Medial}/{Nucleus}/{Coda},{Tone}]";
        }

        // for C# 8
        // using System.Diagnostics.CodeAnalysis;
        // public static bool TryParse(string str, [MaybeNullhen(returnValue: false)] out SinoSyllable value)
        /// <summary>
        /// Tries to parse and load data from a serialize SinoSyllable expressed in the ToString (long) format.
        /// Short format is also supported be will lead to data loss.
        /// </summary>
        public static bool TryParseSingle(string str, out SinoSyllable result)
        {
            Regex regex = new Regex(@"\[(?:([^/]*)\/){3}([^,]*),([0-8])(?:,(?:([a-z]{3}),)?\{(?:([^\}=]+=[^\,}=]+)(?:,?))*\})?\]");

            if (regex.IsMatch(str))
            {
                result = new SinoSyllable();

                Match m = regex.Match(str);

                result.Initial = m.Groups[1].Captures[0].Value;
                result.Medial = m.Groups[1].Captures[1].Value;
                result.Nucleus = m.Groups[1].Captures[2].Value;
                result.Coda = m.Groups[2].Captures[0].Value;
                result.Tone = m.Groups[3].Captures[0].Value;

                if (m.Groups[4].Captures.Count > 0) // handles short format
                {
                    result.Language = m.Groups[4].Captures[0].Value;
                }

                foreach (Capture capture in m.Groups[5].Captures)
                {
                    string[] parts = capture.Value.Split('=');
                    result.Metadata.Add(parts[0], parts[1]);
                }

                return true;
            }
            else
            {
                result = null;

                return false;
            }
        }

        /// <summary>
        /// Tries to parse a series of SinoSyllable serialised as a string.
        /// <returns>True if at least one syllable was extracted from the string passed as parameter.</returns>
        /// </summary>
        public static bool TryParseMultiple(string str, out List<SinoSyllable> result)
        {
            result = new List<SinoSyllable>();

            while (TryParseSingle(str, out var syllable))
            {
                result.Add(syllable);
                int index = str.IndexOf(']') + 1;
                str = str.Substring(index);
            }

            return result.Count > 0;
        }
    }
}
