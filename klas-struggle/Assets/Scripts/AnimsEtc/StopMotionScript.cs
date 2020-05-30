using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AnimsEtc
{
    public class StopMotionScript : MonoBehaviour
    {
        public bool ContinueStopMotion = true;
        public float StepTime = 1f;
        public int StepCount = 1;

        public int StepFadeOutStart = -1;
        public float FadeOutTime = 2.0f;

        public float StepYDiff;
        public float StepXDiff;

        private int coroutineStep = 0;
        private Vector3 currentPosition;


        void Start()
        {
            currentPosition = transform.position;
        }

        public IEnumerator MakeMotion()
        {

            Debug.Log("MakeMotion");
            while (ContinueStopMotion)
            {
                Debug.Log($"MakeMotion: {coroutineStep}");

                if (coroutineStep == StepFadeOutStart)
                {
                    gameObject.DOFadeChildrenSprites(0.0f, FadeOutTime, false);
                }

                if (coroutineStep <= StepCount) 
                { 
                    currentPosition.y += StepYDiff; 
                    currentPosition.x += StepXDiff; 
                    
                    transform.position = currentPosition; 
                    Debug.Log($"MakeMotion: {transform.position}"); 
                }
                else { ContinueStopMotion = false; }

                coroutineStep++;
                yield return new WaitForSeconds(StepTime);
            }
        }
    }
}
