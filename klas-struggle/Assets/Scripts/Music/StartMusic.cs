using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Persistent
{
    public class StartMusic : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameObject.FindGameObjectWithTag("Music")?.GetComponent<MusicClass>()?.PlayMusic();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
