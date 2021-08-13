using System;
using System.Linq;
using System.Collections.Generic;

namespace BonetIDE
{
    class PermuteStackElementsCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            List<ICommandArgument> args = arguments.Skip(1).ToList();

            if (args == null || args.Count != 2 ||
                !(args[0] is IntArgument && args[1] is IntArgument))
                return new CommandResult(false);

            int arg_1 = (args[0] as IntArgument).Value - 1;
            int arg_2 = (args[1] as IntArgument).Value - 1;


            bool valid_1 = context.stack.IsValidStackReference(arg_1);
            bool valid_2 = context.stack.IsValidStackReference(arg_2);

            if (!(valid_1 && valid_2))
                return new CommandResult(false);

            string temp = context.stack[arg_1];
            context.stack[arg_1] = context.stack[arg_2];
            context.stack[arg_2] = temp;

            context.stack.Print();

            return new CommandResult(true);
        }

        public void PrintHelp()
        {
            Console.WriteLine("Permute two elements present on the stack.");
        }
    }
}