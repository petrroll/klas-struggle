using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Wheat
{
    [System.Serializable]
    public class WheatState
    {
        public int Stage1Answer = -1;
        public int Stage2Answer = -1;
        public int Stage3Answer = -1;
        public int Stage4Answer = -1;
        public int Stage5Answer = -1;

        public float Size = 1;
        public Vector3 Loc;

        public WheatState() : this(false) { }
        public WheatState(bool initDebugState)
        {
            if (initDebugState) { InitDebugState(); }
        }

        internal void InitDebugState()
        {
            Stage1Answer = 5;
            Stage2Answer = 0;
            Stage3Answer = 2;
            Stage4Answer = 1;
            Stage5Answer = 1;
        }
    }
}