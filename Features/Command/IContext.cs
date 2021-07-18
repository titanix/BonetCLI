using System.Collections.Generic;

using Leger;

namespace BonetIDE
{
    interface IContext
    {
        ICharacterStore characterReadingStore { get; }
        IGraph idsGraph { get; }
        IBonetDictionary bonetDictionary { get; }
        List<object> resultList { get; }
        List<string> stack { get; }
        IMacroStore macros { get; }
        Queue<CommandComponents> commandList { get; }
        IdsDataStore rawIdsStore { get; }
        List<ICommand> commands { get; }
    }
}