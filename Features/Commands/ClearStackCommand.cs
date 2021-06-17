using System.Collections.Generic;

namespace BonetIDE
{
    class ClearStackCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            context.stack.Clear();
            context.stack.Print();

            return new CommandResult(true);
        }
    }
}