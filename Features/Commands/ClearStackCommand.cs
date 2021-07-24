using System.Collections.Generic;

namespace BonetIDE
{
    class ClearStackCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            context.previousStack.Clear();
            context.previousStack.AddRange(context.stack);

            context.stack.Clear();
            context.stack.Print();

            return new CommandResult(true);
        }
    }
}