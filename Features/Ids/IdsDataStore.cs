using System.Collections.Generic;

namespace BonetIDE
{
    class IdsDataStore
    {
        Dictionary<string, string> data;

        internal IdsDataStore(Dictionary<string, string> data)
        {
            this.data = data;
        }

        internal string SearchCharacter(string ids)
        {
            if (data.ContainsKey(ids))
            {
                return data[ids];
            }

            return "";
        }
    }
}