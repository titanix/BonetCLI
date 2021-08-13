using System.Collections.Generic;
using System.Linq;

using Leger.Extra.Trie;

namespace BonetIDE
{
    /*
     * Store a pair (chunom, reading). Allow for retrieval from reading stripped of their diacritics.
     * Example:
     *  - store (育, dục)
     *  - retrieve (duc) -> [育]
     */
    class CharacterStore : ICharacterStore
    {
        Trie result = new();
        HashSet<string> existing = new();

        public void AddCharacter(string hannom, string reading)
        {
            string key = Utils.StripAccents(reading);
            string val = $"{hannom}/{reading}";

            if (!existing.Contains(val))
            {
                result.Insert(key, val);
                existing.Add(val);
            }
        }

        public List<CharacterReading> SearchByReading(string reading)
        {
            return result.MatchPrefix(reading)
                .Where(a => a.Match == reading)
                .Select(mr => new CharacterReading(mr.Value.Split('/')[0], mr.Value.Split('/')[1]))
                .ToList();
        }

        public List<CharacterReading> SearchByInitial(string initial)
        {
            return result.MatchPrefix(initial)
            .Select(mr => new CharacterReading(mr.Value.Split('/')[0], mr.Value.Split('/')[1]))
            .ToList();
        }
    }
}