using System.Collections.Generic;

namespace BonetIDE
{
    class ViewResultsCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            context.resultList.PrintResultList();

            return new CommandResult(true);
        }
    }
}