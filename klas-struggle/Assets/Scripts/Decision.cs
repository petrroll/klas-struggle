using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum SelectedAnswer { A, B}
    abstract class Decision : MonoBehaviour
    {
        public Decision NextDecision;
        public WheatController Wheat;

        public abstract void Decide(SelectedAnswer answer);
    }
}
