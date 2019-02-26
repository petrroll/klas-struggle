using UnityEngine;

namespace Assets.Scripts.WheatFramework
{
    class Stages : MonoBehaviour
    {
        public void Start()
        {
            Init();
        }

        private void Init()
        {
            // assumes the order of retrieved components is the same as the order in Editor
            var stages = GetComponentsInChildren<Stage>(true);

            // Initialize questions stages subsequently their answers 
            for (int i = 0; i < stages.Length; i++)
            {
                stages[i].Init(i);
            }
        }

    }
}
