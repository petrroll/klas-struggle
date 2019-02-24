using Assets.Scripts;
using SimpleFirebaseUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBaseConnector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Firebase firebase = Firebase.CreateNew("klas-struggle.firebaseapp.com", Secrets.FirebaseAPIKey);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
