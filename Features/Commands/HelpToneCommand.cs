using System;
using System.Collections.Generic;

namespace BonetIDE
{
    class HelpToneCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            Console.WriteLine("1 o   2 ò   3 ó");
            Console.WriteLine("4 ọ   5 ỏ   6 õ");

            return new CommandResult(true);
        }
    }
}