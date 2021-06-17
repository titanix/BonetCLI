using System;
using System.Collections.Generic;

namespace BonetIDE
{
    class PrintStackCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            context.stack.Print();

            return new CommandResult(true);
        }
    }
}