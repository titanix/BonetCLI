using System.Collections.Generic;

namespace BonetIDE
{
    interface IMacroStore
    {
        void PersistMacros(string path);
        void AddMacro(string macro);
        List<string> GetMacros();
    }
}