using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTiles : MonoBehaviour
{
    private Transform[,] _tiles;
    public Transform CameraTransform;
    public Sprite Sprite;

    private float W;
    private float H;
    // Start is called before the first frame update
    void Start()
    {
        _tiles = new Transform[3,3];
        W = Sprite.bounds.size.x;
        H = Sprite.bounds.size.y;
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var obj = new GameObject("t" + i + "," + j);
                var spriteRendr = obj.AddComponent<SpriteRenderer>();
                spriteRendr.sprite = Sprite;
                _tiles[i,j] = obj.transform;
                _tiles[i,j].position = new Vector3((i-1)*W, (j-1)*H, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var centerTileX =  Mathf.FloorToInt(CameraTransform.position.x / W)-1;
        var centerTileY = Mathf.FloorToInt(CameraTransform.position.y / H)-1;
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                _tiles[i,j].position = new Vector3((i+centerTileX)*W, (j+centerTileY)*H, 0);
            }
        }
    }
}
