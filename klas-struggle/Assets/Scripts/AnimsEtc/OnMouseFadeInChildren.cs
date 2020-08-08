using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.AnimsEtc
{
    public class OnMouseFadeInChildren : MonoBehaviour
    {
        //MouseOver animation initialization and check
        public void OnMouseEnter() => ShowMouseOver();
        public void OnMouseExit() => HideMouseOver();

        public void ShowMouseOver()
        {
            SpriteRenderer MouseOverAnimation = GetComponentInChildren<SpriteRenderer>();
            MouseOverAnimation.DOFade(1f, 0.5f);

            // there's only one TextMeshPro in an answer
            //var answerTextBounds = GetComponent<TextMeshPro>().bounds.center;
            //GetComponent<TextMeshPro>().ForceMeshUpdate();
            //Vector3 MouseOverAnimationPosition = MOAnimationTransform.position;
            //MOAnimationTransform.position = answerTextBounds;
            //Debug.Log(answerTextBounds);
        }

        public void HideMouseOver()
        {
            SpriteRenderer MouseOverAnimation = GetComponentInChildren<SpriteRenderer>();
            MouseOverAnimation.DOFade(0f, 0.5f);
        }
    }
}