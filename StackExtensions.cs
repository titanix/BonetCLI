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

        public static void PrintResultList(this List<object> list)
        {
            Console.WriteLine("Results list:");
            int i = 1;
            foreach (object obj in list)
            {
                Console.WriteLine($"{i++}. {obj}");
            }
        }
    }
}