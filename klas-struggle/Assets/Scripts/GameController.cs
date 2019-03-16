using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class GameController
{
    static GameController _instance;
    public static GameController Get { get { if (_instance == null) { _instance = new GameController(); } return _instance; } }

    public FireBaseConnector FireBaseConnector { get; }
    public DataStorage DataStorage { get; }

    bool _inited = false;

    public GameController()
    {
        FireBaseConnector = new FireBaseConnector();
        DataStorage = new DataStorage();
    }

    public void Init(bool lazyDownloadWheatsOnInit)
    {
        if (_inited) { return; }

        if (lazyDownloadWheatsOnInit) { this.DownloadOtherWheatStates(); }

        _inited = true;

    }


    public async Task DownloadOtherWheatStates()
    {
        var states = await FireBaseConnector.GetStatesAsync();

        Debug.Log($"Retrieved & instantiated {states.Count} states.");
        DataStorage.OtherWheatStatesOnline = states;
    }

}
