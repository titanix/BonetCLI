using System;
using System.Linq;
using System.Collections.Generic;

namespace BonetIDE
{
    class AddCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            List<ICommandArgument> args = arguments.Skip(1).ToList();
            string content = "";

            if (args[0] is IntArgument)
            {
                int index = (args[0] as IntArgument).Value - 1;
                if (context.stack.IsValidStackReference(index))
                {
                    content = context.stack[index];
                }
                else
                {
                    return new CommandResult(false);
                }
            }
            else
            {
                content = string.Join(" ", args.Select(a => (a as StringArgument).Value));
            }

            string message = context.bonetDictionary.AddContent(content);
            Console.WriteLine(message);

            return new CommandResult(true);
        }
    }
}