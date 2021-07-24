using System.Collections.Generic;

using Leger;

namespace BonetIDE
{
    class Context : IContext
    {
        internal Context(ICharacterStore characterReadingStore,
        IGraph idsGraph,
        IBonetDictionary bonetDictionary,
        List<object> resultList,
        List<string> stack,
        IMacroStore macroStore,
        Queue<CommandComponents> commandList,
        IdsDataStore rawIdsStore,
        List<ICommand> commands)
        {
            this.characterReadingStore = characterReadingStore;
            this.idsGraph = idsGraph;
            this.bonetDictionary = bonetDictionary;
            this.resultList = resultList;
            this.stack = stack;
            this.macros = macroStore;
            this.commandList = commandList;
            this.rawIdsStore = rawIdsStore;
            this.commands = commands;
        }

        public ICharacterStore characterReadingStore { get; init; }
        public IGraph idsGraph { get; }
        public IBonetDictionary bonetDictionary { get; }
        public List<object> resultList { get; }
        public List<string> stack { get; }
        public IMacroStore macros { get; }
        public Queue<CommandComponents> commandList { get; }
        public IdsDataStore rawIdsStore { get; }
        public List<ICommand> commands { get; }
        public List<string> previousStack { get; } = new();
    }
}