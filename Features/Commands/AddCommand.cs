using System;
using System.Linq;
using System.Collections.Generic;

namespace BonetIDE
{
    class AddCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            List<ICommandArgument> args = arguments.Skip(1).ToList();
            string content = "";

            if (args.Count == 0)
                return new CommandResult(false);

            if (args[0] is IntArgument)
            {
                int index = (args[0] as IntArgument).Value - 1;
                if (context.stack.IsValidStackReference(index))
                {
                    content = context.stack[index];
                }
                else
                {
                    return new CommandResult(false);
                }
            }
            else
            {
                content = string.Join(" ", args.Select(a => (a as StringArgument).Value));
            }

            string message = context.bonetDictionary.AddContent(content);

            foreach (CharacterReading cr in BonetDictionaryLoader.GetCharacterReadings(content))
            {
                context.characterReadingStore.AddCharacter(cr.Character, cr.Reading);
            }

            Console.WriteLine(message);

            return new CommandResult(true);
        }
    }
}