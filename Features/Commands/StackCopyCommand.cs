using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class StackCopyCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            arguments = arguments.Skip(1).ToList();

            ICommandArgument firstArg = arguments.FirstOrDefault();

            if (firstArg is IntArgument intArg)
            {
                int value = intArg.Value - 1;
                if (context.stack.IsValidStackReference(value))
                {
                    context.stack.Add(context.stack.ElementAt(value));
                }
                else
                {
                    return new CommandResult(false);
                }
            }
            else
            {
                return new CommandResult(false);
            }

            context.stack.Print();

            return new CommandResult(true);
        }
    }
}