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

            //TestParser.Test("/Users/Louis/Code/BonetIDE/data/bonet.txt");
        }

        Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>()
        {
            ["a"] = new AddCommand(),
            ["c"] = new ConvertCommand(),
            ["cs"] = new ClearStackCommand(),
            ["d"] = new DeleteCommand(),
            ["dc"] = new DecomposeCharacterCommand(),
            ["h"] = new HelpCommand(),
            ["ht"] = new HelpToneCommand(),
            ["hi"] = new HelpIdsCommand(),
            ["m"] = new MergeCommand(),
            ["me"] = new MacroExecuteCommand(),
            ["ml"] = new MacroListCommand(),
            ["mm"] = new MergeMagicCommand(),
            ["ms"] = new MergeCommand(" "),
            ["nc"] = new NormalizeComposedCommand(),
            ["nd"] = new NormalizeDecomposedCommand(),
            ["p"] = new PushCommand(),
            ["ps"] = new PushSplitCommand(),
            ["vr"] = new ViewResultsCommand(),
            ["vs"] = new ViewStackCommand(),
            ["s"] = new SearchCommand(),
            ["q"] = new QuitCommand(),
        };

        public void Run()
        {
            ICharacterStore wiki = LoadWikiData();
            ICharacterStore bonet = LoadBonetData();
            characterReadingStore = new CombinedCharacterStore(wiki, bonet);
            LoadIdsData();
            OpenBonetDictionary();
            Queue<CommandComponents> commandList = new();

            IContext context = new Context(
                characterReadingStore,
                idsGraph,
                bonetDictionary,
                resultList,
                stack,
                new MacroStore(),
                commandList
            );

        loop:
            if (commandList.Count == 0)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                line = NormalizeSpaces(line);

                foreach (CommandComponents cc in ParseCommandLine(line))
                {
                    commandList.Enqueue(cc);
                }
            }

            if (commandList.Count == 0)
                goto loop;
            CommandComponents commandArgs = commandList.Dequeue();

            if (commandArgs.Count > 0 && commandArgs[0] is StringArgument)
            {
                StringArgument command = commandArgs[0] as StringArgument;

                if (commands.ContainsKey(command.Value))
                {
                    commands[command.Value].Execute(context, commandArgs);
                }
                else
                {
                    if (command.Value[0] >= '\u2E80')
                    {
                        commandArgs.Insert(0, new StringArgument("p"));
                        commands["p"].Execute(context, commandArgs);
                    }
                }
            }
            goto loop;
        }

        ICharacterStore characterReadingStore;
        IGraph idsGraph;
        IBonetDictionary bonetDictionary;
        List<object> resultList = new List<object>();
        List<string> stack = new();

        private ICharacterStore LoadWikiData()
        {
            Console.WriteLine("Loading Wiktionary character reading data.");

            WiktionaryDataLoader wikiData = new();
            ICharacterStore result = characterReadingStore = wikiData.LoadFile(Path.Combine(Environment.CurrentDirectory, "data/wiki.txt"));

            Console.WriteLine("Done.");

            return result;
        }

        private ICharacterStore LoadBonetData()
        {
            Console.WriteLine("Loading Bonet character reading data.");

            BonetDictionaryLoader bonetData = new();
            ICharacterStore result = characterReadingStore = bonetData.LoadFile(Path.Combine(Environment.CurrentDirectory, "data/bonet.txt"));

            Console.WriteLine("Done.");

            return result;
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

        private void SearchNet(string line)
        {
            line = line.Substring(3);
            string url = "https://chunom.org/pages/?search=";
            string search = line;

            if (int.TryParse(line, out int value))
            {
                if (stack.IsValidStackReference(value))
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

        internal static List<CommandComponents> ParseCommandLine(string line)
        {
            List<CommandComponents> result = new();

            string[] parts = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (string subCommand in parts)
            {
                result.Add(ParseCommand(subCommand));
            }

            return result;
        }

        private static CommandComponents ParseCommand(string line)
        {
            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            CommandComponents result = new();

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
