using System.Collections.Generic;

namespace BonetIDE
{
    class ViewStackCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            context.stack.Print();

            return new CommandResult(true);
        }
    }
}