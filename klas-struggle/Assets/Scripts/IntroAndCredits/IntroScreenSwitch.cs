using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.KlasStruggle.Intro
{
    public class IntroScreenSwitch : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown)
            {
                Invoke("SwitchScene", 3);
            }
        }

        void SwitchScene()
        {
            SceneManager.LoadScene("wheat-gen");
        }
    }
}
