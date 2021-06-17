using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonetIDE
{
    class MergeCommand : ICommand
    {
        private  string separator="";
        CharacterConverter charConverter = new();

        public MergeCommand(string separator = "")
        {
            this.separator = separator;
        }

        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            arguments = arguments.Skip(1).ToList();
            StringBuilder result = new();

            foreach (ICommandArgument arg in arguments)
            {
                if (arg is StringArgument)
                {
                    result.Append(arg.ToString() + separator);
                }
                if (arg is IntArgument)
                {
                    int intArg = (arg as IntArgument).Value - 1;
                    if(context.stack.IsValidStackReference(intArg))
                    {
                        result.Append(context.stack.ElementAt(intArg) + separator);
                    }
                }
            }

            context.stack.Add(result.ToString().Trim());
            context.stack.Print();

            return new CommandResult(true);
        }
    }
}