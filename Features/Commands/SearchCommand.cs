using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class SearchCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            arguments = arguments.Skip(1).ToList();

            try
            {
                if (arguments.Count == 1)
                {
                    context.resultList.Clear();
                    context.resultList.AddRange(context.characterReadingStore.SearchByReading(arguments[0].ToString()));
                }
                else
                {
                    ComplexCharacterSearch searcher = new();
                    context.resultList.Clear();
                    string[] chars = arguments.Select(a => a.ToString()).ToArray();
                    context.resultList.AddRange(searcher.Search(context.idsGraph, true, chars));
                }

                context.resultList.PrintResultList();
            }
            catch
            { }

            return new CommandResult(true);
        }
    }
}