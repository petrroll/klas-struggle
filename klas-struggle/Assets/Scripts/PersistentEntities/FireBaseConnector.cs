using Assets.Scripts.KlasStruggle.Wheat;
using SimpleFirebaseUnity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Persistent
{
    /// <summary>
    /// Handles all communication to FireBase, keeps connection open.
    /// </summary>
    public class FireBaseConnector
    {
        private bool _inited = false;
        private Firebase fbRoot;

        public void Init()
        {
            if (_inited) { return; }
            _inited = true;

            fbRoot = Firebase.CreateNew(Secrets.FirebaseURL, Secrets.FirebaseAPIKey);
        }

        public Task<(Firebase, DataSnapshot)> PushStateAsync(WheatState state)
        {
            Init();
            var fbNode = fbRoot.Child("v1");
            var result = JsonUtility.ToJson(state);

            var taskCompletionSource = new TaskCompletionSource<(Firebase, DataSnapshot)>();

            fbNode.OnPushSuccess += (Firebase node, DataSnapshot pushedKeyInfo) => { taskCompletionSource.SetResult((node, pushedKeyInfo)); }; ;
            fbNode.OnPushFailed += (Firebase fireBase, FirebaseError error) => { Debug.Log($"Error pushing data to firebase: {error.Message}"); taskCompletionSource.SetResult((fireBase, null)); };
            fbNode.Push(result);

            return taskCompletionSource.Task;
        }

        public Task<List<WheatState>> GetStatesAsync()
        {
            Init();
            var fbNode = fbRoot.Child("v1");

            var taskCompletionSource = new TaskCompletionSource<List<WheatState>>();
            fbNode.OnGetFailed += (Firebase fireBase, FirebaseError error) => { Debug.Log($"Error getting data from firebase: {error.Message}"); taskCompletionSource.SetResult(new List<WheatState>()); };
            fbNode.OnGetSuccess += (Firebase node, DataSnapshot dta) =>
            {
                List<WheatState> states = ParseDictOfRetrievedJsonStates<WheatState>(dta.RawValue);
                taskCompletionSource.SetResult(states);
            };

            fbNode.GetValue();
            return taskCompletionSource.Task;


        }

        private List<T> ParseDictOfRetrievedJsonStates<T>(object rawDictOfJsonStates)
        {
            List<T> states = new List<T>();
            if (rawDictOfJsonStates is Dictionary<string, object> dictOfJsonStates)
            {
                foreach (var rawJsonState in dictOfJsonStates)
                {
                    if (rawJsonState.Value is string jsonState)
                    {
                        states.Add(JsonUtility.FromJson<T>(jsonState));
                    }
                    else { Debug.Assert(false, "Json deser error"); }

                }
            }
            else if (!(rawDictOfJsonStates is null)) { Debug.Assert(false, "Json deser error"); }

            return states;
        }
    }
}
