using System;
using System.Collections.Generic;

namespace BonetIDE
{
    class QuitCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            Environment.Exit(0);

            return new CommandResult(true);
        }
    }
}