using System;
using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class PushSplitCommand : ICommand
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
                    string content = context.resultList.ToList().ElementAt(value).ToString();
                    string[] parts = content.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    foreach (string p in parts)
                    {
                        context.stack.Add(p);
                    }
                }
                else
                {
                    return new CommandResult(false);
                }
            }
            else
            {
                foreach (ICommandArgument arg in arguments)
                {
                    context.stack.Add(arg.ToString());
                }
            }

            context.stack.Print();

            return new CommandResult(true);
        }
    }
}