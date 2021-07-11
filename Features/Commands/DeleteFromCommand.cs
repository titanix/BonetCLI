using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class DeleteFromCommand : ICommand
    {
        CharacterConverter charConverter = new();

        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            ICommandArgument arg = arguments.Skip(1).FirstOrDefault();

            if (arg == null || arg is StringArgument)
                return new CommandResult(false);

            int index = (arg as IntArgument).Value;
            index--;

            if (index > context.stack.Count)
                return new CommandResult(false);

            context.stack.RemoveRange(index, context.stack.Count - index);

            context.stack.Print();

            return new CommandResult(true);
        }
    }
}