using System.Collections.Generic;

using Leger;

namespace BonetIDE
{
    class Context : IContext
    {
        internal Context(ICharacterStore characterReadingStore,
        IGraph idsGraph,
        IBonetDictionary bonetDictionary,
        IEnumerable<object> resultList,
        List<string> stack)
        {
            this.characterReadingStore = characterReadingStore;
            this.idsGraph = idsGraph;
            this.bonetDictionary = bonetDictionary;
            this.resultList = resultList;
            this.stack = stack;
        }

        public ICharacterStore characterReadingStore { get; private set; }
        public IGraph idsGraph { get; }
        public IBonetDictionary bonetDictionary { get; }
        public IEnumerable<object> resultList { get; }
        public List<string> stack { get; }
    }
}