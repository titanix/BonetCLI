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
            LoadIdsData();
            OpenBonetDictionary();
        //PrintHelp();

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
                        if (line[1] == ' ')
                            Push(line);
                        if (line[1] == 's')
                            PrintStack();
                        break;
                    case "m":
                        if (line[1] == ' ')
                            Merge(line);
                        if (line[1] == 's')
                            MergeWithSpace(line);
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
            string[] parts = SplitOnSpaces(line);

            string nom = parts[0];
            string reading = line.Substring(nom.Length).Trim();

            CodePointIndexedString cpi = new(nom);
            if (cpi.Length == 1)
            {
                bonetDictionary.AddHeadword(nom, reading);
            }
            else
            {
                bonetDictionary.AddCompound(nom, reading);
            }
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

        private void Push(string line)
        {
            line = line.Substring(2);
            if (int.TryParse(line, out int value))
            {
                value--;
                if (value < resultList.Count() && value >= 0)
                {
                    stack.Add(resultList.ToList().ElementAt(value).ToString());
                }
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
                    if (value >= 0 && value < stack.Count)
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
    }
}
