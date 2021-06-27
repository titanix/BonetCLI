using System;
using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class DecomposeCharacterCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            ICommandArgument arg = arguments.Skip(1).FirstOrDefault();

            ComplexCharacterSearch searcher = new();
            context.resultList.Clear();
            string[] chars = arguments.Select(a => a.ToString()).ToArray();
            context.resultList.AddRange(searcher.Decompose(context.idsGraph, chars));

            context.resultList.PrintResultList();

            return new CommandResult(true);
        }
    }
}