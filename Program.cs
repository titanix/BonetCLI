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
            OpenBonetDictionary();
            //PrintHelp();

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
                        ConvertCommand cc = new();
                        cc.Execute(context, commandArgs);
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
                        HelpCommand hc = new();
                        hc.Execute(context, commandArgs);
                        break;
                    case "ht":
                        HelpToneCommand htc = new();
                        htc.Execute(context, commandArgs);
                        break;
                    case "m":
                        MergeCommand mc = new();
                        mc.Execute(context, commandArgs);
                        break;
                    case "ms":
                        MergeCommand msc = new(" ");
                        msc.Execute(context, commandArgs);
                        break;
                    case "p":
                        // TODO: push _ r & push _ n
                        PushCommand pc = new();
                        pc.Execute(context, commandArgs);
                        break;
                    case "ps":
                        PrintStackCommand psc = new();
                        psc.Execute(context, commandArgs);
                        break;
                    case "s":
                        Search(line);
                        break;
                    case "sn":
                        SearchNet(line);
                        break;
                    case "q":
                        QuitCommand qc = new();
                        qc.Execute(context, commandArgs);
                        break;
                    default:
                        break;
                }
            }
            goto loop;
        }

        ICharacterStore characterReadingStore;
        IGraph idsGraph;
        IBonetDictionary bonetDictionary;
        List<object> resultList = new List<object>();
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

        private void Search(string line)
        {
            try
            {
                string[] parts = SplitOnSpaces(line.Substring(1));
                if (parts.Length == 1)
                {
                    resultList.Clear();
                    resultList.AddRange(characterReadingStore.SearchByReading(parts[0]));
                }
                else
                {
                    ComplexCharacterSearch searcher = new();
                    resultList.Clear();
                    resultList.AddRange(searcher.Search(idsGraph, true, parts));
                }

                Console.WriteLine("Results list:");
                int i = 1;
                foreach (object obj in resultList)
                {
                    Console.WriteLine($"{i++}. {obj}");
                }
            }
            catch
            { }
        }

        private string[] SplitOnSpaces(string str)
        {
            return str.Split(new char[] { ' ', '　', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private bool ValidStackReference(int value)
        {
            return value >= 0 && value < stack.Count;
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
