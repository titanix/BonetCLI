using System;
using System.Collections.Generic;
using System.IO;
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
            PrintHelp();

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
        List<object> resultList = new();

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
                string[] parts = line.Substring(1).Split(new char[] { ' ', '　', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                {
                    List<CharacterReading> result = characterReadingStore.SearchByReading(parts[0]);
                    foreach (var r in result)
                    {
                        Console.WriteLine($"{r.Character} {r.Reading}");
                    }
                }
                else
                {
                    ComplexCharacterSearch searcher = new();
                    foreach (string str in searcher.Search(idsGraph, true, parts))
                    {
                        Console.WriteLine(str);
                    }
                }
            }
            catch
            { }
        }

        private void AddWord(string line)
        {
            line = line.Substring(2);
            string[] parts = line.Split(new char[] { ' ', '　', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string nom = parts[0];
            string reading = line.Substring(nom.Length).Trim();

            //Console.WriteLine(parts[0]);
            //Console.WriteLine("debug: [" + reading + "]");

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
            int i = 1;
            foreach (object obj in resultList)
            {
                Console.WriteLine($"{i++}. {obj}");
            }
        }
    }
}
