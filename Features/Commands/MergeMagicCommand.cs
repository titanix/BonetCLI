using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonetIDE
{
    class MergeMagicCommand : ICommand
    {
        private string separator = " ";
        CharacterConverter charConverter = new();

        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            arguments = arguments.Skip(1).ToList();
            StringBuilder result = new();

            foreach (ICommandArgument arg in arguments)
            {
                string content = "";
                if (arg is StringArgument)
                {
                    content = arg.ToString();
                }
                if (arg is IntArgument)
                {
                    int intArg = (arg as IntArgument).Value - 1;
                    if (context.stack.IsValidStackReference(intArg))
                    {
                        content = context.stack.ElementAt(intArg);
                    }
                }

                if (IsCjvk(content))
                {
                    result.Append(content);
                }
                else
                {
                    result.Append(content + separator);
                }
            }

            context.stack.Add(result.ToString().Trim());
            context.stack.Print();

            return new CommandResult(true);
        }

        private bool IsCjvk(string str)
        {
            foreach (char c in str)
            {
                if (c < '\u2E80')
                    return false;
            }

            return true;
        }
    }
}