using System;
using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class HelpCommand : ICommand
    {
        CharacterConverter charConverter = new();

        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            Console.WriteLine("** Available commands **");

            IEnumerable<ICommand> sorted = context.commands.OrderBy(a => a.GetType().Name);
            
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