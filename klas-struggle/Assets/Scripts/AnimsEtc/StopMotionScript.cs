using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AnimsEtc
{
    public class StopMotionScript : MonoBehaviour
    {
        public Vector3 Step1;
        public Vector3 Step2;
        public Vector3 Step3;
        public Vector3 Step4;

        public bool ContinueCoroutine = true;
        public float StepLength = 1f;

        private int CoroutineStep = 0;

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
                if (CoroutineStep == 1) { MotionStepMake(Step1); }
                else if (CoroutineStep == 2) { MotionStepMake(Step2); }
                else if (CoroutineStep == 3) { MotionStepMake(Step3); }
                else if (CoroutineStep == 4) { MotionStepMake(Step4); }
                else if (CoroutineStep == 5) { ContinueCoroutine = false; }

                yield return new WaitForSeconds(StepLength);
            }
        }
    }
}
