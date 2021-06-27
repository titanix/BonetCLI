using System;
using System.Collections.Generic;

namespace BonetIDE
{
    class HelpIdsCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            Console.WriteLine("V ⿰   H ⿱   L ⿺");
            Console.WriteLine("T ⿹   N ⿵   O ⿴");
            Console.WriteLine("M ⿲   E ⿳   W ⿻");

            return new CommandResult(true);
        }
    }
}