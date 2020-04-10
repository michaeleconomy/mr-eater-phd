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
    public int blockSize;
    public Transform tilesParent;
    public SpriteRenderer squarePrefab;
    public bool drawSquares;

    private readonly Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    private readonly Dictionary<Vector2Int, int> blocks = new Dictionary<Vector2Int, int>();
    private int currentBlock = 100;
    private List<Vector2Int> directions = new List<Vector2Int>{
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
    };
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
        return tiles.GetWithDefault(position);
    }

    public void DoNearby(Vector2Int currentPos) {
        EachNearby(currentPos, FitShape);
        EachNearby(currentPos, SetTile);

        // Debug.Log("TODO add tacos");
        // Debug.Log("TODO add burgers");
        // Debug.Log("TODO add other foods");
        // Debug.Log("TODO add enemies");
    }

    private  int? BlockToMerge(int block, Vector2Int startPos) {
        var squaresToCheck = new List<Vector2Int>{
            startPos
        }; 
        var checkedSquares = new HashSet<Vector2Int>();
        var borders = new Dictionary<int, int>();
        while (!squaresToCheck.Empty()) {
            var pos = squaresToCheck.Pop();
            foreach (var direction in directions) {
                var otherPos = pos + direction;
                if (!blocks.TryGetValue(otherPos, out var otherBlock)) {
                    continue;
                }
                if (otherBlock != block) {
                    borders.Increment(otherBlock);
                    continue;
                }
                if (!checkedSquares.Contains(otherPos)) {
                    squaresToCheck.Add(otherPos);
                }
            }
            checkedSquares.Add(pos);
        }

        foreach (var pair in borders) {
            var borderCount = pair.Value;
            if (borderCount == 1) {
                return pair.Key;
            }
        }
        return null;
    }

    private void MergeBlocks(int block, Vector2Int startPos) {
        for (var mergeBlock = BlockToMerge(block, startPos);
                mergeBlock != null;
                mergeBlock = BlockToMerge(block, startPos)) {
            var squaresToCheck = new List<Vector2Int>{
                startPos
            }; 
            var checkedSquares = new HashSet<Vector2Int>();
            while (!squaresToCheck.Empty()) {
                var pos = squaresToCheck.Pop();
                foreach (var direction in directions) {
                    var otherPos = pos + direction;
                    if (!blocks.TryGetValue(otherPos, out var otherBlock)) {
                        continue;
                    }
                    if (otherBlock == mergeBlock.Value) {
                        blocks[otherPos] = block;
                        squaresToCheck.Add(otherPos);
                        continue;
                    }
                    if (otherBlock != block) {
                        continue;
                    }
                    if (!checkedSquares.Contains(otherPos)) {
                        squaresToCheck.Add(otherPos);
                    }
                }
                checkedSquares.Add(pos);
            }
        }
    }


    private void FitShape(Vector2Int pos) {
        if (blocks.ContainsKey(pos)) {
            return;
        }
        currentBlock++;
        foreach (var shape in shapes.Shuffle()) {
            var rotated = shape;
            for (var i = 0; i < 4; i++) {
                if (ShapeFits(rotated, pos)) {
                    return;
                }
                rotated = rotated.Rotate();
            }
        }
        // Debug.Log("No shape fit, inserting single.");
        blocks[pos] = currentBlock;
        MergeBlocks(currentBlock, pos);
        if (drawSquares) {
            var square = Instantiate(squarePrefab, new Vector3(pos.x + .5f, pos.y + .5f), Quaternion.identity);
            square.color = UnityEngine.Random.ColorHSV();
        }
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
                    blocks[space] = currentBlock;
                    if (drawSquares) {
                        var square = Instantiate(squarePrefab, new Vector3(space.x + .5f, space.y + .5f), Quaternion.identity);
                        square.color = color;
                    }
                }
            }
        }
        MergeBlocks(currentBlock, pos);
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
        currentBlock = 100;
    }

}