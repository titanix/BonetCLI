using System;
using System.Collections.Generic;

namespace BonetIDE
{
    class HelpCommand : ICommand
    {
        CharacterConverter charConverter = new();

        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            Console.WriteLine("** Available commands **");
            Console.WriteLine();

            Console.WriteLine("Search");
            Console.WriteLine("s quoc_ngu_without_diacritics");
            Console.WriteLine("-> s boi");
            Console.WriteLine("s sinogram_1 ... sinogram_N");
            Console.WriteLine("-> s 不 皿");
            Console.WriteLine();

            Console.WriteLine("Add Content");
            Console.WriteLine("a nom reading");
            Console.WriteLine("-> a 盃 bôi");
            Console.WriteLine();

            Console.WriteLine("Help");
            Console.WriteLine("h");

            return new CommandResult(true);
        }
    }
}