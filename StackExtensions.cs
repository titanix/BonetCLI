using System;
using System.Collections.Generic;

namespace BonetIDE
{
    public static class StackExtensions
    {
        public static void Print(this List<string> stack)
        {
            Console.WriteLine("Stack content:");
            int i = 1;
            foreach (string str in stack)
            {
                Console.WriteLine($"{i++}. {str}");
            }
        }

        public static bool IsValidStackReference(this List<string> stack, int value)
        {
            return value >= 0 && value < stack.Count;
        }
    }
}