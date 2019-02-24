using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatController : MonoBehaviour
{
    private int _size;
    public int Size
    {
        get { return _size; }
        set { _size = value; transform.localScale = new Vector3(_size, _size); }
    }

    private Vector3 _position;
    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; transform.localPosition = _position; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
