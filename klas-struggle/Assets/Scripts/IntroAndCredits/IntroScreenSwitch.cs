using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.KlasStruggle.Intro
{
    public class IntroScreenSwitch : MonoBehaviour
    {
        public int IntroBeforeNextSceneDelay = 3;

        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown)
            {
                Invoke(nameof(SwitchScene), IntroBeforeNextSceneDelay);
            }
        }

        void SwitchScene()
        {
            SceneManager.LoadScene("wheat-gen");
        }
    }
}
