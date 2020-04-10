using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tiler : MonoBehaviour {
    public static Tiler instance;

    public Tile tilePrefab;
    public Enemy enemyPrefab;
    public SpriteRenderer squarePrefab;
    public int blockSize;
    public Transform tilesParent;

    private readonly Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    private readonly Dictionary<Vector2Int, int> blocks = new Dictionary<Vector2Int, int>();
    private int block = 100;
    private readonly List<bool[,]> shapes = new List<bool[,]>(),
        baseShapes = new List<bool[,]> {
        new bool[,] {
            {true, true}
        },
        // new bool[,] {
        //     {true, true, true}
        // },
        // new bool[,] {
        //     {true, true, true, true}
        // },
        // new bool[,] {
        //     {true, true, true, true, true}
        // },
        new bool[,] {
            {true, true},
            {true, true}
        },
        new bool[,] {
            {false, true},
            {true, true}
        },
        new bool[,] {
            {false, true, false},
            {true, true, true}
        },
        new bool[,] {
            {false, true, false, false},
            {true, true, true, true}
        },
        new bool[,] {
            {true, false, false},
            {true, true, true}
        },
        // new bool[,] {
        //     {true, false, false, false},
        //     {true, true, true, true}
        // },
        // new bool[,] {
        //     {true, true, false},
        //     {false, true, true}
        // },
    };

    private void Awake() {
        instance = this;
        FlipShapes();
    }

    public void Initialize() {
        Clear();

        DoNearby(Vector2Int.zero);
    }

    private void FlipShapes() {
        foreach (var shape in baseShapes) {
            shapes.Add(shape);
            var flipped = shape.Flip();
            if (flipped != shape) {
                shapes.Add(flipped);
            }
        }
    }

    public Tile GetTile(Vector2Int position) {
        if (!tiles.TryGetValue(position, out var tile)) {
            throw new Exception("no tile at position: " + position);
        }
        return tile;
    }

    public void DoNearby(Vector2Int currentPos) {
        EachNearby(currentPos, FitShape);
        var pos = currentPos - Vector2Int.one * blockSize;
        var s = "";
        for(pos.y = currentPos.y - blockSize; pos.y < currentPos.y + blockSize; pos.y++) {
            for(pos.x = currentPos.x - blockSize; pos.x < currentPos.x + blockSize; pos.x++) {
                s += blocks.GetWithDefault(pos, 999) + " ";
            }
            s += "\n";
        }
        Debug.Log(s);
        EachNearby(currentPos, SetTile);

        Debug.Log("TODO add tacos");
        Debug.Log("TODO add burgers");
        Debug.Log("TODO add other foods");
        Debug.Log("TODO add enemies");
    }

    private void FitShape(Vector2Int pos) {
        if (blocks.ContainsKey(pos)) {
            return;
        }
        block++;
        foreach (var shape in shapes.Shuffle()) {
            var rotated = shape;
            for (var i = 0; i < 4; i++) {
                if (ShapeFits(rotated, pos)) {
                    return;
                }
                rotated = rotated.Rotate();
            }
        }
        Debug.Log("No shape fit, inserting single.");
        blocks[pos] = block;

        var square = Instantiate(squarePrefab, new Vector3(pos.x + .5f, pos.y + .5f), Quaternion.identity);
        square.color = UnityEngine.Random.ColorHSV();
    }

    private bool ShapeFits(bool[,] shape, Vector2Int pos) {
        var offset = new Vector2Int(0, 0);
        for (; offset.x < shape.GetLength(0); offset.x++) {
            for (offset.y = 0; offset.y < shape.GetLength(1); offset.y++) {
                if (!shape[offset.x, offset.y]) {
                    continue;
                }
                if (ShapeFits(shape, offset, pos)) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool ShapeFits(bool[,] shape, Vector2Int offset, Vector2Int pos) {
        for (var x = 0; x < shape.GetLength(0); x++) {
            for (var y = 0; y < shape.GetLength(1); y++) {
                var space = new Vector2Int(x + pos.x - offset.x, y + pos.y - offset.y);
                if (blocks.ContainsKey(space) && shape[x, y]) {
                    return false;
                }
            }
        }
        var color = UnityEngine.Random.ColorHSV();
        for (var x = 0; x < shape.GetLength(0); x++) {
            for (var y = 0; y < shape.GetLength(1); y++) {
                var space = new Vector2Int(x + pos.x - offset.x, y + pos.y - offset.y);
                if (shape[x, y]) {
                    blocks[space] = block;
                    var square = Instantiate(squarePrefab, new Vector3(space.x + .5f, space.y + .5f), Quaternion.identity);
                    square.color = color;
                }
            }
        }
        return true;
    }

    private void SetTile(Vector2Int pos) {
        if (!tiles.TryGetValue(pos, out var tile)) {
            tile = Instantiate(tilePrefab,
                new Vector3(pos.x, pos.y),
                Quaternion.identity,
                tilesParent);
            tiles.Add(pos, tile);
        }
        var currentBlock = blocks[pos];
        tile.wallUp = currentBlock == blocks.GetWithDefault(pos + Vector2Int.left);
        tile.wallRight = currentBlock == blocks.GetWithDefault(pos + Vector2Int.down);
        tile.wallDown =  blocks.GetWithDefault(pos - Vector2Int.one) == blocks.GetWithDefault(pos + Vector2Int.down);
        tile.wallLeft = blocks.GetWithDefault(pos - Vector2Int.one) == blocks.GetWithDefault(pos + Vector2Int.left);
        tile.Refresh();
    }

    private void EachNearby(Vector2Int currentPos, Action<Vector2Int> action) {
        action(currentPos);
        for (var distance = 1; distance < blockSize; distance++) {
            var pos = new Vector2Int(currentPos.x - distance, currentPos.y - distance);
            for (; pos.y < currentPos.y + distance; pos.y++) {
                action(pos);
            }
            for (; pos.x < currentPos.x + distance; pos.x++) {
                action(pos);
            }
            for (; pos.y > currentPos.y - distance; pos.y--) {
                action(pos);
            }
            for (; pos.x > currentPos.x - distance; pos.x--) {
                action(pos);
            }
        }
    }

    private void Clear() {
        foreach (var tile in tiles.Values) {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
        blocks.Clear();
        block = 100;
    }

}