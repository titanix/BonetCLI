using System;
using System.Linq;
using System.Collections.Generic;

namespace BonetIDE
{
    class DeleteCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            List<ICommandArgument> args = arguments.Skip(1).ToList();
            List<int> references = new();

            foreach (ICommandArgument arg in args)
            {
                if (arg is IntArgument)
                {
                    int index = (arg as IntArgument).Value - 1;
                    if (context.stack.IsValidStackReference(index))
                    {
                        references.Add(index);
                    }
                }
            }

            references.Sort();
            int offset = 0;

            foreach (int i in references)
            {
                context.stack.RemoveAt(i - offset);
                offset++;
            }

            context.stack.Print();

            return new CommandResult(true);
        }
    }
}