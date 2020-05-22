using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AnimsEtc
{
    public class StopMotionController : MonoBehaviour
    {
        public List<StopMotionScript> HandledObjects = new List<StopMotionScript>();

        public void RunStopMotion()
        {
            foreach (var obj in HandledObjects)
            {
                StartCoroutine(obj.MakeMotion());
            }
        }
    }
}
