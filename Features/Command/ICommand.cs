using System.Collections.Generic;

namespace BonetIDE
{
    interface ICommand
    {
        ICommandResult Execute(IContext context, List<ICommandArgument> arguments);
    }
}