using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BonetIDE
{
    class SearchNetCommand : ICommand
    {
        Dictionary<string, string> providers = new()
        {
            ["c"] = "https://chunom.org/pages/?search=",
            ["g"] = "https://www.google.com/search?q=",
            ["k"] = "https://www.kanjipedia.jp/search?kt=1&sk=leftHand&k=",
            ["w"] = "https://en.wiktionary.org/wiki/",
        };

        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("This command is only supported on Windows.");

                return new CommandResult(false);
            }

            arguments = arguments.Skip(1).ToList();

            if (arguments.Count != 2)
                return new CommandResult(false);

            string provider = arguments[0].ToString();

            string search = arguments[1] switch
            {
                StringArgument sa => sa.Value,
                IntArgument ia => context.stack.IsValidStackReference(ia.Value - 1) ? context.stack[ia.Value - 1] : "",
                _ => ""
            };

            if (providers.ContainsKey(provider))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = providers[provider] + search,
                    UseShellExecute = true
                });

                return new CommandResult(true);
            }

            return new CommandResult(false);
        }

        public void PrintHelp()
        {
            Console.WriteLine("* Available search providers:");
            foreach (KeyValuePair<string, string> pair in providers)
                Console.WriteLine($"{pair.Key}\t{pair.Value}");
        }
    }
}