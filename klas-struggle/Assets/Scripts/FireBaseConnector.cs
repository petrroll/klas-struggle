using Assets.Scripts;
using SimpleFirebaseUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FireBaseConnector : MonoBehaviour
{
    public WheatController Controller;

    // Start is called before the first frame update
    void Start()
    {
        Controller.State = DataStorage.DS.State ?? new WheatState();
        Controller.State.Size *= 0.5f;
        Controller.ApplyState();

        var state = Controller.State;
        var result = JsonUtility.ToJson(state);

        Firebase firebase = Firebase.CreateNew("https://klas-struggle.firebaseio.com", Secrets.FirebaseAPIKey);
        var textNodeV1 = firebase.Child("checkTestV1", true); // get's a child that can have OnSuccess / OnFail callbacks set

        textNodeV1.Push(result);

        textNodeV1.OnGetSuccess += onSuccess;
        textNodeV1.GetValue();
    }

    void onSuccess(Firebase node, DataSnapshot data)
    {
        var rawDictOfJsonStates = data.RawValue;
        List<WheatState> states = ParseDictOfRetrievedJsonStates<WheatState>(rawDictOfJsonStates);

        foreach(var state in states)
        {
            var newInstace = Instantiate(Controller, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-3, 3), 0), Quaternion.identity);
            newInstace.State = state;
            newInstace.State.Size *= 0.5f;
            newInstace.ApplyState();
        }
    }

    static List<T> ParseDictOfRetrievedJsonStates<T>(object rawDictOfJsonStates)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
