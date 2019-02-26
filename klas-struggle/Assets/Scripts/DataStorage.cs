using System.Collections.Generic;

namespace Assets.Scripts
{
    class DataStorage
    {
        static DataStorage _ds;
        public static DataStorage DS { get { if (_ds == null) { _ds = new DataStorage(); } return _ds; } }

        public WheatState State;
    }
}
