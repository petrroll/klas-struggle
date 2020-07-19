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

        public Vector2 StepDiff;

        public int StepFadeOutStart = -1;
        public float FadeOutTime = 2.0f;

        public IEnumerator MakeMotion()
        {
            for (int coroutineStep = 0; coroutineStep <= StepCount; coroutineStep++)
            {
                if (coroutineStep == StepFadeOutStart)
                {
                    gameObject.DOFadeChildrenSprites(0.0f, FadeOutTime, false);
                }

                var currentPosition = transform.position;
                currentPosition.y += StepDiff.y;
                currentPosition.x += StepDiff.x;
                transform.position = currentPosition;

                yield return new WaitForSeconds(StepTime);
            }
        }
    }
}
