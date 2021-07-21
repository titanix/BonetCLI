using System.Linq;
using System.Collections.Generic;

namespace BonetIDE
{
    class DeleteBelowCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            ICommandArgument arg = arguments.Skip(1).FirstOrDefault();

            if (arg == null || arg is StringArgument)
                return new CommandResult(false);

            int index = (arg as IntArgument).Value;
            index--;

            if (index > context.stack.Count)
                return new CommandResult(false);

            if (!context.stack.IsValidStackReference(index))
                return new CommandResult(false);

            context.stack.RemoveRange(0, index);

            context.stack.Print();

            return new CommandResult(true);
        }
    }
}