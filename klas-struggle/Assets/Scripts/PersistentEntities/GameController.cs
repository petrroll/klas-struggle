using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Persistent
{
    /// <summary>
    /// Game controller singleton, holds all persistent data structures & is proxy for non-scene-dependent events.
    /// </summary>
    public class GameController
    {
        bool _inited = false;
        static GameController _instance;

        public static GameController Get { get { if (_instance == null) { _instance = new GameController(); } return _instance; } }

        public FireBaseConnector FireBaseConnector { get; }
        public DataStorage DataStorage { get; }

        public GameController()
        {
            FireBaseConnector = new FireBaseConnector();
            DataStorage = new DataStorage();
        }

        public async Task Init(bool lazyDownloadWheatsOnInit)
        {
            if (_inited) { return; }
            _inited = true;

            if (lazyDownloadWheatsOnInit) { await this.DownloadOtherWheatStates(); }
        }

        public async Task DownloadOtherWheatStates()
        {
            var states = await FireBaseConnector.GetStatesAsync();

            Debug.Log($"Retrieved & instantiated {states.Count} states.");
            DataStorage.OtherWheatStatesOnline = states;
        }
    }
}
