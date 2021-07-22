using System;
using System.Linq;

namespace BonetIDE
{
    static class CommandNameHelper
    {
        public static string GetCommandCode(Type type)
        {
            string fullName = type.Name;
            string shortName = fullName.Substring(0, fullName.IndexOf("Command"));
            string cmd = string.Join("", shortName.Where(c => char.IsUpper(c)));

            return cmd.ToLower();
        }
    }
}
