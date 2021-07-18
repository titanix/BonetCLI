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

                    string search = arguments[0].ToString();
                    search = search.Replace("dd", "đ");

                    if (arguments[0] is IntArgument intArg)
                    {
                        if (context.stack.IsValidStackReference(intArg.Value - 1))
                        {
                            search = context.stack.ElementAt(intArg.Value - 1);
                        }
                    }

                    if (arguments[0] is StringArgument strArg && strArg.Value[0] >= '\u2FF0' && strArg.Value[0] <= '\u2FFB')
                    {
                        context.resultList.Add(context.rawIdsStore.SearchCharacter(strArg.Value));
                    }
                    else
                    {
                        context.resultList.AddRange(context.characterReadingStore.SearchByReading(search));
                    }
                }
                else
                {
                    ComplexCharacterSearch searcher = new();
                    context.resultList.Clear();

                    List<string> searchChars = new();
                    foreach (ICommandArgument arg in arguments)
                    {
                        if (arg is StringArgument stringArg)
                        {
                            searchChars.Add(stringArg.Value);
                        }

                        if (arg is IntArgument intArg)
                        {
                            if (context.stack.IsValidStackReference(intArg.Value - 1))
                            {
                                searchChars.Add(context.stack.ElementAt(intArg.Value - 1));
                            }
                        }
                    }

                    string[] chars = searchChars.Select(a => a.ToString()).ToArray();
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