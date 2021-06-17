using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Leger;
using Leger.IO;

namespace BonetIDE
{
    class Program
    {
        public static void Main(string[] args)
        {
            Program p = new();
            p.Run();
        }

        public void Run()
        {
            LoadWikiData();
            LoadIdsData();
            OpenBonetDictionary();
            //PrintHelp();

            CharacterConverter charConverter = new();
            IContext context = new Context(
                characterReadingStore,
                idsGraph,
                bonetDictionary,
                resultList,
                stack
            );

        loop:
            Console.Write("> ");
            string line = Console.ReadLine();
            line = NormalizeSpaces(line);
            List<ICommandArgument> commandArgs = ParseCommand(line);

            if (commandArgs.Count > 0 && commandArgs[0] is StringArgument)
            {
                StringArgument command = commandArgs[0] as StringArgument;

                switch (command.Value)
                {
                    case "a":
                        AddCommand ac = new();
                        ac.Execute(context, commandArgs);
                        break;
                    case "c":
                        string converted = charConverter.Convert(line.Substring(2));
                        Push(converted);
                        break;
                    case "cs":
                        ClearStackCommand csc = new();
                        csc.Execute(context, commandArgs);
                        break;
                    case "d":
                        DeleteCommand dc = new();
                        dc.Execute(context, commandArgs);
                        break;
                    case "h":
                        PrintHelp();
                        break;
                    case "ht":
                        PrintToneHelp();
                        break;
                    case "m":
                        Merge(line);
                        break;
                    case "ms":
                        MergeWithSpace(line);
                        break;
                    case "p":
                        // TODO: push _ r & push _ n
                        Push(line.Substring(2));
                        break;
                    case "ps":
                        stack.Print();
                        break;
                    case "s":
                        Search(line);
                        break;
                    case "sn":
                        SearchNet(line);
                        break;
                    case "q":
                        goto end;
                    default:
                        break;
                }
            }
            goto loop;

        end:
            ;
        }

        ICharacterStore characterReadingStore;
        IGraph idsGraph;
        IBonetDictionary bonetDictionary;
        IEnumerable<object> resultList = new List<object>();
        List<string> stack = new();

        private bool SecondCharEqual(string str, char c)
        {
            return str.Length > 1 && str[1] == c;
        }

        private void LoadWikiData()
        {
            Console.WriteLine("Loading Wiktionary character reading data.");

            WiktionaryDataLoader wikiData = new();
            characterReadingStore = wikiData.LoadWiktionaryFile(Path.Combine(Environment.CurrentDirectory, "data/wiki.txt"));

            Console.WriteLine("Done.");
        }

        private void LoadIdsData()
        {
            Console.WriteLine("Loading IDS character composition data.");

            XmlDeserializer deser = new();
            idsGraph = deser.Deserialize(Path.Combine(Environment.CurrentDirectory, "data/ids.xml"));

            Console.WriteLine("Done.");
        }

        private void OpenBonetDictionary()
        {
            bonetDictionary = new BonetDictionary(Path.Combine(Environment.CurrentDirectory, "data/bonet.txt"));
        }

        private void PrintHelp()
        {
            Console.WriteLine("** Available commands **");
            Console.WriteLine();

            Console.WriteLine("Search");
            Console.WriteLine("s quoc_ngu_without_diacritics");
            Console.WriteLine("-> s boi");
            Console.WriteLine("s sinogram_1 ... sinogram_N");
            Console.WriteLine("-> s 不 皿");
            Console.WriteLine();

            Console.WriteLine("Add Content");
            Console.WriteLine("a nom reading");
            Console.WriteLine("-> a 盃 bôi");
            Console.WriteLine();

            Console.WriteLine("Help");
            Console.WriteLine("h");
        }

        private void Search(string line)
        {
            try
            {
                string[] parts = SplitOnSpaces(line.Substring(1));
                if (parts.Length == 1)
                {
                    resultList = characterReadingStore.SearchByReading(parts[0]);
                }
                else
                {
                    ComplexCharacterSearch searcher = new();
                    resultList = searcher.Search(idsGraph, true, parts);
                }
                PrintResultList();
            }
            catch
            { }
        }

        private void PrintResultList()
        {
            Console.WriteLine("Results list:");
            int i = 1;
            foreach (object obj in resultList)
            {
                Console.WriteLine($"{i++}. {obj}");
            }
        }

        private void Push(string content)
        {
            if (int.TryParse(content, out int value))
            {
                value--;
                if (value < resultList.Count() && value >= 0)
                {
                    stack.Add(resultList.ToList().ElementAt(value).ToString());
                }
                stack.Print();
            }
            else
            {
                stack.Add(content.Trim());
                stack.Print();
            }
        }

        private bool Merge(string line, string separator = "")
        {
            line = line.Substring(2);
            string[] parts = SplitOnSpaces(line);
            StringBuilder result = new();

            foreach (string str in parts)
            {
                if (int.TryParse(str, out int value))
                {
                    value--;
                    if (ValidStackReference(value))
                    {
                        result.Append(stack.ElementAt(value) + separator);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    result.Append(str + separator);
                }
            }

            stack.Add(result.ToString().Trim());
            stack.Print();

            return true;
        }

        private bool MergeWithSpace(string line)
        {
            line = line.Substring(1);
            return Merge(line, " ");
        }

        private string[] SplitOnSpaces(string str)
        {
            return str.Split(new char[] { ' ', '　', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private bool ValidStackReference(int value)
        {
            return value >= 0 && value < stack.Count;
        }

        private void PrintToneHelp()
        {
            Console.WriteLine("1 o   2 ò   3 ó");
            Console.WriteLine("4 ọ   5 ỏ   6 õ");
        }

        private void SearchNet(string line)
        {
            line = line.Substring(3);
            string url = "https://chunom.org/pages/?search=";
            string search = line;

            if (int.TryParse(line, out int value))
            {
                if (ValidStackReference(value))
                {
                    search = stack[value];
                }
                else
                {
                    return;
                }
            }

            System.Diagnostics.Process.Start(@"/Applications/Brave\ Browser.app/Contents/MacOS/Brave\ Browser", url + search);
        }

        private string NormalizeSpaces(string str)
        {
            return str.Replace("　", " ") // Chinese space
                .Replace("  ", " ") // double space by a single space
                .Replace("  ", " ") // multiple times to remove potential multiple spaces (up to three)
                .Replace("  ", " ");
        }

        private List<ICommandArgument> ParseCommand(string line)
        {
            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            List<ICommandArgument> result = new();

            foreach (string part in parts)
            {
                if (int.TryParse(part, out int value))
                {
                    result.Add(new IntArgument(value));
                }
                else
                {
                    result.Add(new StringArgument(part));
                }
            }

            return result;
        }
    }
}
