using System;
using System.Collections.Generic;

namespace BonetIDE
{
    class MacroListCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            Console.WriteLine("Macro list");

            int i = 1;
            foreach(string macro in context.macros.GetMacros())
            {
                Console.WriteLine($"{i++}. => {macro}");
            }

            return new CommandResult(true);
        }
    }
}