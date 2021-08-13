using System.Collections.Generic;

namespace BonetIDE
{
    interface ICharacterStore
    {
        void AddCharacter(string hannom, string reading);
        List<CharacterReading> SearchByReading(string reading);
        List<CharacterReading> SearchByInitial(string initial);
    }
}