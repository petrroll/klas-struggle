using Assets.Scripts.KlasStruggle.Wheat;
using System.Collections.Generic;

namespace Assets.Scripts.KlasStruggle.Persistent
{
    /// <summary>
    /// Class for storing information between scenes.
    /// </summary>
    public class DataStorage
    {
        public WheatState GeneratedWheatState;
        public List<WheatState> OtherWheatStatesOnline;
    }
}
