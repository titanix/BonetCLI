using System;
using System.Collections.Generic;
using System.IO;

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
            PrintHelp();

        loop:
            Console.Write("> ");
            string line = Console.ReadLine();

            if (line.Length > 0)
            {
                switch (line[0].ToString())
                {
                    case "s":
                        string[] parts = line.Substring(1).Split(new char [] { ' ', '　', ' '}, StringSplitOptions.RemoveEmptyEntries);
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
                            foreach(string str in  searcher.Search(idsGraph, true, parts))
                            {
                                Console.WriteLine(str);
                            }
                        }
                        break;
                        case "h":
                            PrintHelp();
                            break;
                    default:
                        break;
                }
            }
            goto loop;
        }

        ICharacterStore characterReadingStore;
        IGraph idsGraph;

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
            Console.WriteLine("Help");
            Console.WriteLine("h");
        }


        /*
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
        */
    }
}
