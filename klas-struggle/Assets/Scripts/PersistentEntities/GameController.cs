using Assets.Scripts.KlasStruggle.Wheat;
using System.Collections.Generic;
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

        internal bool StatesFromOnline { get; private set; }

        public GameController()
        {
            FireBaseConnector = new FireBaseConnector();
            DataStorage = new DataStorage();
        }

        public async Task InitAsync(bool downloadWheatsOnInit, bool forceStatesFromOffline, bool stableRngSeed)
        {
            if (_inited) { return; }
            _inited = true;

            if (stableRngSeed) { Random.InitState(42); }
            if (downloadWheatsOnInit) { await this.GetOtherWheatStatesAsync(forceStatesFromOffline); }
        }

        public async Task GetOtherWheatStatesAsync(bool forceStatesFromOffline)
        {
            // Tries to download wheat-states from firebase:
            // - doesn't work -> gets states from disk
            // - when getting from firebase finishes -> commits the data only when disk-data haven't already been used
            // NOTE: No need to be worried about `DataStorage.OtherWheatStatesOnline` race conditions -> this method
            // will always be called on UI thread -> two calls can't interleave.
            if (!forceStatesFromOffline)
            {
                var onlineStates = await FireBaseConnector.GetStatesFromFirebaseAsync();
                Debug.Log($"Downloaded {onlineStates?.Count} states.");

                if (DataStorage.OtherWheatStatesOnline == null && onlineStates != null)
                {
                    DataStorage.OtherWheatStatesOnline = onlineStates;
                    StatesFromOnline = true;

                    return;
                }
            }

            var offlineStates = await FireBaseConnector.GetStatesFromDiskAsync("OfflineFields/klas-struggle-3d476-export");
            Debug.Log($"Read from disk {offlineStates?.Count} states.");

            if (offlineStates == null)
            {
                offlineStates = new List<WheatState>();
                Debug.LogWarning("Couldn't get other wheat states from online/disk.");
            }

            DataStorage.OtherWheatStatesOnline = offlineStates;
            StatesFromOnline = true;

            return;
        }
    }
}
