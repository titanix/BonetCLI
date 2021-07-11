using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BonetIDE
{
    class NormalizeComposedCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            return Execute(context, arguments, NormalizationForm.FormC);
        }

        internal ICommandResult Execute(IContext context, List<ICommandArgument> arguments, NormalizationForm form)
        {
            arguments = arguments.Skip(1).ToList();
            
            List<string> toNormalize = new();

            foreach (ICommandArgument arg in arguments)
            {
                if (arg is StringArgument stringArg)
                {
                    toNormalize.Add(stringArg.Value);
                }

                if (arg is IntArgument intArg)
                {
                    if (context.stack.IsValidStackReference(intArg.Value - 1))
                    {
                        toNormalize.Add(context.stack.ElementAt(intArg.Value - 1));
                    }
                }
            }

            context.resultList.Clear();

            foreach (string str in toNormalize)
            {
                context.resultList.Add(str.Normalize(form));
            }

            context.resultList.PrintResultList();

            return new CommandResult(true);
        }
    }
}