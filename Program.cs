using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Leger;
using Leger.IO;

namespace BonetIDE
{
    class Program
    {
        public static void Main(string[] args)
        {
            UTF8Encoding utf8 = new();
            Console.OutputEncoding = utf8;

            Program p = new();
            p.Run();
        }

        Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>()
        {
            ["a"] = new AddCommand(),
            ["c"] = new ConvertCommand(),
            ["cs"] = new ClearStackCommand(),
            ["d"] = new DeleteCommand(),
            ["dc"] = new DecomposeCharacterCommand(),
            ["df"] = new DeleteFromCommand(),
            ["h"] = new HelpCommand(),
            ["ht"] = new HelpToneCommand(),
            ["hi"] = new HelpIdsCommand(),
            ["m"] = new MergeCommand(),
            ["me"] = new MacroExecuteCommand(),
            ["ml"] = new MacroListCommand(),
            ["mm"] = new MergeMagicCommand(),
            ["ms"] = new MergeSpaceCommand(),
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
            Queue<CommandComponents> commandList = new();

            IContext context = new Context(
                new CombinedCharacterStore(LoadWikiData(), LoadBonetData()),
                LoadIdsData(),
                OpenBonetDictionary(),
                new List<object>(),
                new List<string>(),
                new MacroStore(),
                commandList,
                LoadRawIdsData(),
                commands.Values.ToList()
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

        private ICharacterStore LoadWikiData()
        {
            Console.WriteLine("Loading Wiktionary character reading data.");

            WiktionaryDataLoader wikiData = new();
            ICharacterStore result = wikiData.LoadFile(Path.Combine(Environment.CurrentDirectory, "data/wiki.txt"));

            Console.WriteLine("Done.");

            return result;
        }

        private ICharacterStore LoadBonetData()
        {
            Console.WriteLine("Loading Bonet character reading data.");

            BonetDictionaryLoader bonetData = new();
            ICharacterStore result = bonetData.LoadFile(Path.Combine(Environment.CurrentDirectory, "data/bonet.txt"));

            Console.WriteLine("Done.");

            return result;
        }

        private IGraph LoadIdsData()
        {
            Console.WriteLine("Loading IDS character composition data (XML).");

            XmlDeserializer deser = new();
            IGraph idsGraph = deser.Deserialize(Path.Combine(Environment.CurrentDirectory, "data/ids.xml"));

            Console.WriteLine("Done.");

            return idsGraph;
        }

        private BonetDictionary OpenBonetDictionary()
        {
            return new BonetDictionary(Path.Combine(Environment.CurrentDirectory, "data/bonet.txt"));
        }

        private IdsDataStore LoadRawIdsData()
        {
            Console.WriteLine("Loading IDS character composition data (text).");

            IdsFileLoader loader = new();
            IdsDataStore result = new(loader.LoadFile(Path.Combine(Environment.CurrentDirectory, "data/ids.txt")));

            Console.WriteLine("Done.");

            return result;
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
