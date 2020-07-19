using TMPro;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.AnimsEtc
{
    public class FadeOutFieldTutorial : MonoBehaviour
    {
        public float FadeOutTime = 3f;

        bool alreadyFired = false;
        void Update()
        {

            //We will need to chain this to unlocking players movement so you cant erase it during fadein.
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0) && !alreadyFired)
            {
                alreadyFired = true;
                GetComponent<TextMeshPro>().DOFade(0f, FadeOutTime);
            }
        }
    }
}
