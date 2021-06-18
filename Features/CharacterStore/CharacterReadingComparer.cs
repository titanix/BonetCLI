using System;
using System.Collections.Generic;

namespace BonetIDE
{
class CharacterReadingComparer : IEqualityComparer<CharacterReading>
{
    public bool Equals(CharacterReading x, CharacterReading y)
    {
        if (Object.ReferenceEquals(x, y)) return true;

        if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            return false;

        return x.Character == y.Character && x.Reading == y.Reading;
    }

    // If Equals() returns true for a pair of objects
    // then GetHashCode() must return the same value for these objects.

    public int GetHashCode(CharacterReading charReading)
    {
        if (Object.ReferenceEquals(charReading, null)) return 0;

        int hashProductCharacter = charReading.Character.GetHashCode();

        int hashProductReading = charReading.Reading.GetHashCode();

        return hashProductCharacter ^ hashProductReading;
    }
}
}