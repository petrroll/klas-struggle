using Assets.Configs.Secrets;
using Assets.Scripts.KlasStruggle.Wheat;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
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

        public Task<(Firebase, DataSnapshot)> PushStateToFirebaseAsync(WheatState state)
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

        public Task<List<WheatState>> GetStatesFromFirebaseAsync()
        {
            Init();
            var fbNode = fbRoot.Child("v1");

            var taskCompletionSource = new TaskCompletionSource<List<WheatState>>();
            fbNode.OnGetFailed += (Firebase fireBase, FirebaseError error) => { Debug.Log($"Error getting data from firebase: {error.Message}"); taskCompletionSource.SetResult(null); };
            fbNode.OnGetSuccess += async (Firebase node, DataSnapshot dta) =>
            {
                await Task.Yield(); // Await for next frame after deserializing the root-level JSON
                List<WheatState> states = await ParseDictOfRetrievedJsonStatesAsync<WheatState>(dta.RawValue);
                taskCompletionSource.SetResult(states);
            };

            fbNode.GetValue();
            return taskCompletionSource.Task;
        }

        public async Task<List<WheatState>> GetStatesFromDiskAsync(string filePath)
        {
            Init();

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);
            if (targetFile == null) { Debug.LogWarning("Couldn't find file with offline field."); }

            var rootLevelJSON = Json.Deserialize(targetFile.text) as Dictionary<string, object>;
            await Task.Yield(); // Await for next frame after deserializing the root-level JSON
            List<WheatState> states = await ParseDictOfRetrievedJsonStatesAsync<WheatState>(rootLevelJSON);

            return states;
        }

        private async Task<List<T>> ParseDictOfRetrievedJsonStatesAsync<T>(object rawDictOfJsonStates)
        {
            List<T> states = new List<T>();
            if (rawDictOfJsonStates is Dictionary<string, object> dictOfJsonStates)
            {
                int i = 0;
                foreach (var rawJsonState in dictOfJsonStates)
                {
                    // don't deserialize everything in one go -> yield in between individual deserializations
                    if (i % 20 == 0) { await Task.Yield(); }

                    if (rawJsonState.Value is string jsonState)
                    {
                        states.Add(JsonUtility.FromJson<T>(jsonState));
                    }
                    else { Debug.Assert(false, "Json deser error"); }
                    i++;
                }
            }
            else if (!(rawDictOfJsonStates is null)) { Debug.Assert(false, "Json deser error"); return null; }

            return states;
        }
    }
}
