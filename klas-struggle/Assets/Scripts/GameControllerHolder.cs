using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerHolder : MonoBehaviour
{

    public GameController Controller;
    private bool _inited = false;

    public bool LazyDownloadWheatsOnInit = true;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GameController.Get;
        Controller.Init(LazyDownloadWheatsOnInit);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
