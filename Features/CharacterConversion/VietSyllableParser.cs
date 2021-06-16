using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using SiniticDidactics.Common;
using Lecailliez.Language.Regex;

namespace BonetIDE
{
    public class VietnameseSyllableParser
    {
        List<string> initials = new()
        {
            "b",
            "c",
            "d",
            "đ",
            "g",
            "h",
            "k",
            "l",
            "m",
            "n",
            "p",
            "qu",
            "r",
            "s",
            "t",
            "v",
            "x",
            "ch",
            "gh",
            "kh",
            "ng",
            "ngh",
            "nh",
            "th",
            "tr",
            "ph",
            "gi",
        };

        List<string> medials = new List<string>() { "u", "o", "i" };

        // la valeure associée est la première tirée du tableau :
        // https://vi.wikipedia.org/wiki/Ch%E1%BB%AF_Qu%E1%BB%91c_ng%E1%BB%AF#Nguy%C3%AAn_%C3%A2m
        // tones are in order Ngang (1), Huyền (2), Sắc (3), Hỏi (4), Ngã (5), Nặng (6)
        List<string> vowels = new()
        {
            "a",
            "à",
            "á",
            "ả",
            "ã",
            "ạ",

            "ă",
            "ằ",
            "ắ",
            "ẳ",
            "ẵ",
            "ặ",

            "â",
            "ầ",
            "ấ",
            "ẩ",
            "ẫ",
            "ậ",

            "e",
            "è",
            "é",
            "ẻ",
            "ẽ",
            "ẹ",

            "ê",
            "ề",
            "ế",
            "ể",
            "ễ",
            "ệ",

            "i",
            "ì",
            "í",
            "ỉ",
            "ĩ",
            "ị",

            "o",
            "ò",
            "ó",
            "ỏ",
            "õ",
            "ọ",

            "ô",
            "ồ",
            "ố",
            "ổ",
            "ỗ",
            "ộ",

            "ơ",
            "ờ",
            "ớ",
            "ở",
            "ỡ",
            "ợ",

            "u",
            "ù",
            "ú",
            "ủ",
            "ũ",
            "ụ",

            "ư",
            "ừ",
            "ứ",
            "ử",
            "ữ",
            "ự",

            "y",
            "ỳ",
            "ý",
            "ỷ",
            "ỹ",
            "ỵ",

            // diphtongs
            "iê",
            "iề",
            "iế",
            "iể",
            "iễ",
            "iệ",

            "ia",
            "ìa",
            "ía",
            "ỉa",
            "ĩa",
            "ịa",

            "yê",
            "yề",
            "yế",
            "yể",
            "yễ",
            "yệ",

            "ya",
            "ỳa",
            "ýa",
            "ỷa",
            "ỹa",
            "ỵa",

            "ia",
            "ià",
            "iá",
            "iả",
            "iã",
            "iạ",

            "ươ",
            "ườ",
            "ướ",
            "ưở",
            "ưỡ",
            "ượ",

            "ưa",
            "ừa",
            "ứa",
            "ửa",
            "ữa",
            "ựa",

            "ua",
            "ùa",
            "úa",
            "ủa",
            "ũa",
            "ụa",
        };

        List<string> finals_consonants = new()
        {
            "p",
            "t",
            "ch",
            "c",
            "m",
            "n",
            "nh",
            "ng",
        };

        List<string> finals_vowels = new()
        {
            "o",
            "u",
            "y",
            "i",
        };

        private static string InitialConsonants = "initial_consonants";
        private static string PrefrontalVowels = "prefrontal_vowels";
        private static string MainVowels = "main_vowels";
        private static string Finals = "finals";

        private string SyllableRegex()
        {
            IRegexBuilder regex = new RegexBuilder()
            .Raw(
                new RegexBuilder()
                    .StartGroup(InitialConsonants)
                        .Disjunction(initials.ToArray())
                    .CloseGroup()
                    .AtMost(1)
                    .Regex())
            .Raw(
                new RegexBuilder()
                    .StartGroup(PrefrontalVowels)
                        .Disjunction(medials.ToArray())
                    .CloseGroup()
                    .AtMost(1)
                    .Regex())
            .Raw(
                new RegexBuilder()
                    .StartGroup(MainVowels)
                        .Disjunction(vowels.ToArray())
                    .CloseGroup()
                .Exactly(1)
                .Regex())
            .Raw(
                new RegexBuilder()
                    .StartGroup(Finals)
                        .Disjunction(finals_consonants.ToArray().Concat(finals_vowels.ToArray()).ToArray())
                    .CloseGroup()
                    .AtMost(1)
                    .Regex());

            return regex.Regex();
        }

        public SinoSyllable Parse(string str)
        {
            string regexString = $"^{SyllableRegex()}$";

            Regex regex = new Regex(regexString);
            Match matchResult = regex.Match(str);

            SinoSyllable result = new SinoSyllable();

            result.Initial = "";
            result.Medial = "";
            result.Coda = "";
            result.Language = "vie";

            if (matchResult.Groups[InitialConsonants].Captures.Count > 0)
            {
                result.Initial = matchResult.Groups[InitialConsonants].Captures[0].Value;
            }

            if (matchResult.Groups[PrefrontalVowels].Captures.Count > 0)
            {
                result.Medial = matchResult.Groups[PrefrontalVowels].Captures[0].Value;
            }

            if (matchResult.Groups[MainVowels].Captures.Count > 0)
            {
                string v = matchResult.Groups[MainVowels].Captures[0].Value;
                result.Nucleus = v.Last().ToString();
            }

            if (matchResult.Groups[Finals].Captures.Count > 0)
            {
                result.Coda = matchResult.Groups[Finals].Captures[0].Value;
            }

            if (result.Nucleus == "ươ")
            {
                result.Medial = "ư";
                result.Nucleus = "ơ";
            }

            if (result.Nucleus == "ưa")
            {
                result.Medial = "ư";
                result.Nucleus = "a";
            }

            return result;
        }
    }
}