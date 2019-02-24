using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum SelectedAnswer { A, B}
    abstract class Decision : MonoBehaviour
    {
        public Decision NextDecision;
        public WheatController Controller;

        public bool ActiveOnStart = false;

        public virtual void Decide(SelectedAnswer answer)
        {
            gameObject.SetActive(false);
            NextDecision?.gameObject.SetActive(true);
        }

        public void Start()
        {
            gameObject.SetActive(ActiveOnStart);
        }
    }
}
