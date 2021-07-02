using System;
using System.Linq;
using System.Collections.Generic;

namespace BonetIDE
{
    class MacroExecuteCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            ICommandArgument arg = arguments.Skip(1).FirstOrDefault();

            if (arg == null)
                return new CommandResult(false);

            if (arg is IntArgument intArg)
            {
                int macroRef = intArg.Value - 1;
                if (context.macros.GetMacros().IsValidStackReference(macroRef))
                {
                    string macro = context.macros.GetMacros().ElementAt(macroRef);

                    foreach (CommandComponents cc in Program.ParseCommandLine(macro))
                    {
                        context.commandList.Enqueue(cc);
                    }

                    return new CommandResult(true);
                }
            }


            return new CommandResult(false);
        }
    }
}