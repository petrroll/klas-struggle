using Assets.Scripts;
using SimpleFirebaseUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBaseConnector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Firebase firebase = Firebase.CreateNew("https://klas-struggle.firebaseio.com", Secrets.FirebaseAPIKey);
        var database = firebase.Child("asdf", true); // get's a child that can have OnSuccess / OnFail callbacks set
        //database.SetValue("asdr", false);          // always operates with the whole sub-graph get/set/push
        database.Push("asdf");

        database.OnGetSuccess += onSuccess;
        database.GetValue();
    }

    private void onSuccess(Firebase arg1, DataSnapshot arg2)
    {
        Debug.Log(arg2.RawValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
