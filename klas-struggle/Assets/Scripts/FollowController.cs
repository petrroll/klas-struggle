using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{

    public class FollowController : MonoBehaviour
    {
        public GameObject Target;
        private Vector3 offset;

        void Start()
        {
            offset = transform.position - Target.transform.position;
        }

        void LateUpdate()
        {
            Vector3 newLocation = Target.transform.position + offset;
            transform.position = newLocation;
        }
    }
}