using System;
using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class HelpCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            arguments = arguments.Skip(1).ToList();

            // syntax: h <command_name>
            if (arguments.Count > 0 && arguments[0] is StringArgument strArg)
            {
                ICommand cmd = context.commands.Where(c => CommandNameHelper.GetCommandCode(c.GetType()) == strArg.Value).FirstOrDefault();
                if (cmd != null)
                    cmd.PrintHelp();

                return new CommandResult(true);
            }

            // syntax: h
            Console.WriteLine("** Available commands **");

            IEnumerable<ICommand> sorted = context.commands.OrderBy(a => CommandNameHelper.GetCommandCode(a.GetType()));

            foreach (ICommand command in sorted)
            {
                string fullName = command.GetType().Name;
                string shortName = fullName.Substring(0, fullName.IndexOf("Command"));
                string cmd = string.Join("", shortName.Where(c => char.IsUpper(c)));

                Console.WriteLine($"{cmd.ToLower()}\t{shortName}");
            }

            return new CommandResult(true);
        }
    }
}