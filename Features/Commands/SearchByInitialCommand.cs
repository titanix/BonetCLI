using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class SearchByInitialCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            arguments = arguments.Skip(1).ToList();

            if (arguments is null || arguments.Count != 1)
                return new CommandResult(false);

            if (arguments[0] is StringArgument strArg)
            {
                List<CharacterReading> results = context.characterReadingStore.SearchByInitial(strArg.Value);

                context.resultList.Clear();
                context.resultList.AddRange(results);

                context.resultList.PrintResultList();

                return new CommandResult(true);
            }

            return new CommandResult(false);
        }
    }
}