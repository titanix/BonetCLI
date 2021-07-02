using System;
using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class DecomposeCharacterCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            arguments = arguments.Skip(1).ToList();

            ComplexCharacterSearch searcher = new();
            context.resultList.Clear();
            List<string> searchChars = new();

            foreach (ICommandArgument arg in arguments)
            {
                if (arg is StringArgument stringArg)
                {
                    searchChars.Add(arg.ToString());
                }

                if (arg is IntArgument intArg)
                {
                    int stackRef = intArg.Value - 1;
                    if (context.stack.IsValidStackReference(stackRef))
                    {
                        searchChars.Add(context.stack.ElementAt(stackRef));
                    }
                }
            }

            context.resultList.AddRange(searcher.Decompose(context.idsGraph, searchChars.ToArray()));

            context.resultList.PrintResultList();

            return new CommandResult(true);
        }
    }
}