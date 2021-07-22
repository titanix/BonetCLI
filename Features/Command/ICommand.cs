using System;
using System.Collections.Generic;

namespace BonetIDE
{
    interface ICommand
    {
        ICommandResult Execute(IContext context, List<ICommandArgument> arguments);

        void PrintHelp() => Console.WriteLine("No help details for this command.");
    }
}