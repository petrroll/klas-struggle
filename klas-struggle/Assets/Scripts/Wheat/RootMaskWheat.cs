using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Wheat
{
    public class RootMaskWheat : MonoBehaviour
    {
        public bool MaskedByDefault = true;
        private SpriteMask spriteMask;

        // Start is called before the first frame update
        void Start()
        {
            spriteMask = gameObject.GetComponent<SpriteMask>();
            spriteMask.gameObject.SetActive(spriteMask);
        }

        public void SetActive(bool active = true) => spriteMask.gameObject.SetActive(active);
    }

}
