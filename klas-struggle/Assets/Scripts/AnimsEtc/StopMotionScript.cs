using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts.AnimsEtc
{
    public class StopMotionScript : MonoBehaviour
    {
        private Vector3 TempPosition;
        public float yDifference;
        public float xDifference;

        public bool ContinueCoroutine = true;
        public float StepLength = 1f;

        public int StepCount = 1;
        private int CoroutineStep = 0;

        void Start()
        {
            TempPosition = transform.position;
        }
        // Update is called once per frame
        void MotionStepMake(Vector3 Step)
        {
            transform.position = Step;
        }

        public IEnumerator MakeMotion()
        {
            Debug.LogWarning("MakeMotion");
            while (ContinueCoroutine)
            {
                Debug.LogWarning($"MakeMotion: {CoroutineStep}");

                CoroutineStep++;
                if (CoroutineStep <= StepCount) { TempPosition.y += yDifference; TempPosition.x += xDifference; transform.position = TempPosition; Debug.LogWarning($"MakeMotion: {transform.position}"); }
                else { ContinueCoroutine = false; }

                yield return new WaitForSeconds(StepLength);
            }
        }
    }
}
