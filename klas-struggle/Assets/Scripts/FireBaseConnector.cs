using Assets.Scripts;
using SimpleFirebaseUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FireBaseConnector : MonoBehaviour
{
    public WheatController PlayerWheat;
    public WheatController Prefab;

    public bool LoadOtherInstances = true;
    public bool SaveCurrentInstance = true;

    // Start -> retrieveAndInstantiateOtherStates -> DataRetrieved -> PushCurrentState -> PushSuccess

    // Start is called before the first frame update
    void Start()
    {
        // init generated instance
        PlayerWheat.State = DataStorage.DS.State ?? new WheatState();
        PlayerWheat.State.Size *= 0.5f;
        PlayerWheat.ApplyState();

        // init firebase connection
        Firebase firebase = Firebase.CreateNew(Secrets.FirebaseURL, Secrets.FirebaseAPIKey);
        var textNodeV1 = firebase.Child("checkTestV1", true); // get's a child that can have OnSuccess / OnFail callbacks set

        RetrieveAndInstantiateOtherStates(textNodeV1);
    }

    private void RetrieveAndInstantiateOtherStates(Firebase textNodeV1)
    {
        // if !LoadOtherInstances skip loading and move to push immediately
        if (!LoadOtherInstances) { PushCurrentState(textNodeV1); return; }

        // retrieve instances from firebase
        textNodeV1.OnGetSuccess += DataRetrieved;
        textNodeV1.OnGetFailed += (Firebase fireBase, FirebaseError error) => Debug.Log($"Error retrieving data from firebase: {error.Message}");

        textNodeV1.GetValue();
    }

    private void PushCurrentState(Firebase textNodeV1)
    {
        // if !LoadOtherInstances skip pushing
        if (!SaveCurrentInstance) { return; }

        // push generated instance to firebase
        var state = PlayerWheat.State;
        var result = JsonUtility.ToJson(state);

        textNodeV1.OnPushSuccess += PushSuccess;
        textNodeV1.OnPushFailed += (Firebase fireBase, FirebaseError error) => Debug.Log($"Error pushing data to firebase: {error.Message}");
        textNodeV1.Push(result);
    }

    private void PushSuccess(Firebase node, DataSnapshot pushedKeyInfo)
    {
        Debug.Log($"Push successful.");
    }

    void DataRetrieved(Firebase node, DataSnapshot data)
    {
        // parse retrieved states and instantiate them as game objects
        List<WheatState> states = ParseDictOfRetrievedJsonStates<WheatState>(data.RawValue);
        Debug.Log($"Retrieved & instantiated {states.Count} states.");

        foreach(var state in states)
        {
            InstantiateNewElement(state);
        }

        PushCurrentState(node);
    }

    private void InstantiateNewElement(WheatState state)
    {
        // instantiate one object based on its state
        Vector3 location = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-3, 3), 0);

        var newInstace = Instantiate(Prefab, location, Quaternion.identity);
        newInstace.State = state;
        newInstace.State.Size *= 0.5f;

        // Set prefab copy so it doesn't disable itself automatically on Start & initialize it with state & enable
        newInstace.InitOnStart = true;
        newInstace.InitAndEnable();
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
        else { Debug.Assert(false, "Json deser error"); }

        return states;
    }

}
