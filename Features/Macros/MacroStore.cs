using System;
using System.Collections.Generic;

namespace BonetIDE
{
    class MacroStore : IMacroStore
    {
        List<string> macros = new();

        internal MacroStore()
        {
            macros.Add("mm 2 1 ; a 3 ; d 3");
            macros.Add("mm 2 4 1 3 ; a 5 ; d 3 4 5");
            macros.Add("mm 4 2 3 1 ; a 5 ; d 3 4 5");
            macros.Add("mm 2 3 1 4 ; a 5 ; d 3 4 5");
        }
        
        public void AddMacro(string macro)
        {
            macros.Add(macro);
        }

        public List<string> GetMacros()
        {
            return macros;
        }

        public void PersistMacros(string path)
        {
            throw new NotImplementedException();
        }
    }
}