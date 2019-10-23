using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Wheat
{
    public class HideableObject : MonoBehaviour
    {
        public float FadeInTime = 3;
        public float FadeOutTime = 3;

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void FadeOut()
        {
            Show();
            gameObject.DOFadeChildrenTextsAndSprites(0, FadeOutTime);
        }

        public void FadeIn()
        {
            Hide();
            gameObject.DOFadeChildrenTextsAndSprites(1, FadeInTime);
        }

        public void Show()
        {
            gameObject.SetFadeChildrenTextsAndSprites(1);
        }

        public void Hide()
        {
            gameObject.SetFadeChildrenTextsAndSprites(0);
        }
    }
}
