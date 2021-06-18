using System.Collections.Generic;
using System.Linq;

namespace BonetIDE
{
    /*
     * Combine multiple ICharacterStore to search from and contains a separate, non-persisted CharacterStore
     * for adding characters at runtime.
     */
    class CombinedCharacterStore : ICharacterStore
    {
        List<ICharacterStore> stores = new();
        ICharacterStore memoryStore = new CharacterStore();

        internal CombinedCharacterStore(params ICharacterStore[] stores)
        {
            this.stores.AddRange(stores);
            this.stores.Add(memoryStore);
        }

        public void AddCharacter(string hannom, string reading)
        {
            memoryStore.AddCharacter(hannom, reading);
        }

        public List<CharacterReading> SearchByReading(string reading)
        {
            List<CharacterReading> results = new();

            foreach (ICharacterStore store in stores)
            {
                results.AddRange(store.SearchByReading(reading));
            }

            return results.Distinct(new CharacterReadingComparer()).ToList();
        }
    }
}