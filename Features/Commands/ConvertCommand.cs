using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    class ConvertCommand : ICommand
    {
        CharacterConverter charConverter = new();

        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            ICommandArgument arg = arguments.Skip(1).FirstOrDefault();

            if (arg == null || arg is IntArgument)
                return new CommandResult(false);

            string converted = charConverter.Convert(arg.ToString());
            converted = charConverter.ConvertIds(converted);
            context.stack.Add(converted);

            context.stack.Print();

            return new CommandResult(true);
        }
    }
}