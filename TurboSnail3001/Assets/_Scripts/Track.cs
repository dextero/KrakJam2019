using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField]
    private GameObject TilePrefab;
    private Vector3 TileSize;

    private List<List<GameObject>> tiles;

    GameObject MakeTile(char c, Vector2 pos) {
        switch (c) {
            case ' ':
                return null;
            case 'x':
                return GameObject.Instantiate(TilePrefab, new Vector3(pos.x * TileSize.x, 0, pos.y * TileSize.z), Quaternion.identity);
            default:
                throw new ArgumentException("unsupported tile char: " + c);
        }
    }

    List<GameObject> MakeTilesRow(string line, Vector2 startPos) {
        return line.Select((c, idx) => MakeTile(c, new Vector2(startPos.x + idx, startPos.y))).ToList();
    }
    List<List<GameObject>> MakeTiles(string[] text, Vector2 startPos) {
        return text.Select((line, idx) => MakeTilesRow(line, new Vector2(startPos.x, startPos.y + idx))).ToList();;
    }

    // Start is called before the first frame update
    void Start()
    {
        TileSize = TilePrefab.GetComponent<MeshRenderer>().bounds.size;

        string[] map = new string[] {
            " xxx ",
            " xxx ",
            "xxx  ",
            "xxx  ",
            " xxx ",
            " xxx ",
            "  xxx",
            "  xxx",
            " xxx ",
            " xxx ",
        }.Reverse().ToArray();
        Vector2 startPos = new Vector2(-2.0f, 0.0f);

        tiles = MakeTiles(map, startPos);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
