using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace BonetIDE
{
    class SaveStackCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            ICommandArgument arg = arguments.Skip(1).FirstOrDefault();

            if (arg == null || arg is not StringArgument)
                return new CommandResult(false);

            StringArgument strArg = arg as StringArgument;

            using StreamWriter tw = new(strArg.Value);
            context.stack.Print(tw);

            return new CommandResult(true);
        }
    }
}