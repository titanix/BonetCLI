using System;
using System.Collections.Generic;

namespace BonetIDE
{
    class RestoreStackCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            if (context.previousStack.Count > 0)
            {
                context.stack.Clear();
                context.stack.AddRange(context.previousStack);
                context.stack.Print();
                context.previousStack.Clear();
            }

            return new CommandResult(true);
        }

        public void PrintHelp()
        {
            Console.WriteLine("Allow one restauration of the stack cleared by [c]lear [s]tack command.");
        }
    }
}