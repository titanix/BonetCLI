using System;
using System.Collections.Generic;
using System.Linq;

using Language.General;

namespace BonetIDE
{
    class StackSplitStringCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            arguments = arguments.Skip(1).ToList();

            ICommandArgument firstArg = arguments.FirstOrDefault();

            if (firstArg is IntArgument intArg)
            {
                int value = intArg.Value - 1;
                if (context.stack.IsValidStackReference(value))
                {
                    string source = context.stack.ElementAt(value);

                    CodePointIndexedString cpi = new(source);

                    bool isFullNom = true;
                    foreach (string str in cpi.Iterator)
                    {
                        if (str.Length == 1 && str[0] < '\u4E00')
                        {
                            isFullNom = false;
                            break;
                        }
                    }

                    if (isFullNom)
                    {
                        foreach (string str in cpi.Iterator)
                            context.stack.Add(str);
                    }
                    else
                    {
                        string[] parts = source.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        context.stack.AddRange(parts);
                    }
                }
                else
                {
                    return new CommandResult(false);
                }
            }
            else
            {
                return new CommandResult(false);
            }

            context.stack.Print();

            return new CommandResult(true);
        }

        public void PrintHelp()
        {
            Console.WriteLine("Split a string indexed by a stack reference over (Western) spaces and push the resulting strings on the stack.");
            Console.WriteLine("If the string is totally composed of Han/Nôm character, decompose the string into individual characters instead.");
        }
    }
}