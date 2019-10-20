using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Persistent
{
    /// <summary>
    /// Wrapper for <see cref="GameController"/> to put into GameObjects with some configuration.
    /// </summary>
    public class GameControllerHolder : MonoBehaviour
    {
        public GameController Controller;
        public bool LazyDownloadWheatsOnInit = true;

        // Start is called before the first frame update
        void Start()
        {
            Controller = GameController.Get;
            _ = Controller.InitAsync(LazyDownloadWheatsOnInit);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
