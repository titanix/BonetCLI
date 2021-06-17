using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class PushCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            arguments = arguments.Skip(1).ToList();

            ICommandArgument firstArg = arguments.FirstOrDefault();

            if (firstArg is IntArgument intArg)
            {
                int value = intArg.Value - 1;
                if (value < context.resultList.Count() && value >= 0)
                {
                    context.stack.Add(context.resultList.ToList().ElementAt(value).ToString());
                }
                else
                {
                    return new CommandResult(false);
                }
            }
            else
            {
                string content = string.Join(" ", arguments);
                context.stack.Add(content);
            }

            context.stack.Print();

            return new CommandResult(true);
        }
    }
}