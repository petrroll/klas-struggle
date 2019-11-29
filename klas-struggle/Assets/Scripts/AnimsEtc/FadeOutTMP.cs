using TMPro;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.AnimsEtc
{
    public class FadeOutTMP : MonoBehaviour
    {
        bool alreadyFired = false;
        void Update()
        {
            if (Input.anyKeyDown && !alreadyFired)
            {
                alreadyFired = true;
                GetComponent<TextMeshProUGUI>().DOFade(0f, 3f);
            }
        }
    }
}
