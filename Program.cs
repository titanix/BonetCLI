using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Language.General;
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
            //LoadIdsData();
            OpenBonetDictionary();
            //PrintHelp();

            CharacterConverter charConverter = new();

        loop:
            Console.Write("> ");
            string line = Console.ReadLine();

            if (line.Length > 0)
            {
                switch (line[0].ToString())
                {
                    case "s":
                        Search(line);
                        break;
                    case "a":
                        AddWord(line);
                        break;
                    case "p":
                        // TODO: push _ r & push _ n
                        if (line[1] == ' ')
                            Push(line.Substring(2));
                        if (line[1] == 's')
                            PrintStack();
                        break;
                    case "m":
                        if (line[1] == ' ')
                            Merge(line);
                        if (line[1] == 's')
                            MergeWithSpace(line);
                        break;
                    case "c":
                        if (line[1] == ' ')
                        {
                            string converted = charConverter.Convert(line.Substring(2));
                            Push(converted);
                        }
                        if (line[1] == 's')
                        {
                            stack = new();
                            PrintStack();
                        }
                        break;
                    case "d":
                        //Delete(line);
                        break;
                    case "h":
                        PrintHelp();
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

        private void AddWord(string line)
        {
            line = line.Substring(2);
            string content = line;

            if (int.TryParse(line[0].ToString(), out int value))
            {
                if (ValidStackReference(value))
                {
                    content = stack[value];
                }
            }

            string message = bonetDictionary.AddContent(content);
            Console.WriteLine(message);
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
                PrintStack();
            }
            else
            {
                stack.Add(content.Trim());
                PrintStack();
            }
        }

        private void PrintStack()
        {
            Console.WriteLine("Stack content:");
            int i = 1;
            foreach (string str in stack)
            {
                Console.WriteLine($"{i++}. {str}");
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
            PrintStack();

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
    }
}
