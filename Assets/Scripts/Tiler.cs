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

    private readonly Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    public static readonly Dictionary<Vector2Int, bool> wallsH = new Dictionary<Vector2Int, bool>(),
        wallsV = new Dictionary<Vector2Int, bool>();

    private void Awake() {
        instance = this;
    }

    public void Initialize() {
        Clear();

        DoNearby(Vector2Int.zero);
    }

    public Tile GetTile(Vector2Int position) {
        if (!tiles.TryGetValue(position, out var tile)) {
            throw new Exception("no tile at position: " + position);
        }
        return tile;
    }

    public void DoNearby(Vector2Int currentPos) {
        EachNearby(currentPos, true, pos => {
            if (!wallsV.ContainsKey(pos)) {
                wallsV.Add(pos, Rand.B());
            }
        });

        EachNearby(currentPos, true, pos => {
            if (!wallsH.ContainsKey(pos)) {
                wallsH.Add(pos, CannotHaveHorizontalWall(pos) ? false : Rand.B());
            }
        });

        EachNearby(currentPos, false, pos => {
            if (!tiles.ContainsKey(pos)) {
                var tile = Instantiate(tilePrefab,
                    new Vector3(pos.x, pos.y),
                    Quaternion.identity,
                    tilesParent);
                tile.wallUp = wallsV[pos];
                tile.wallDown = wallsV[pos + Vector2Int.down];
                tile.wallRight = wallsH[pos];
                tile.wallLeft = wallsH[pos + Vector2Int.left];
                tile.Refresh();
                tiles.Add(pos, tile);
            }
        });

        Debug.Log("TODO add tacos");
        Debug.Log("TODO add burgers");
        Debug.Log("TODO add other foods");
        Debug.Log("TODO add enemies");
    }

    private bool CannotHaveHorizontalWall(Vector2Int pos) {
        var lefts = 0;
        if (wallsH.GetWithDefault(pos + Vector2Int.left, false)) {
            lefts++;
        }
        if (wallsV.GetWithDefault(pos, false)) {
            lefts++;
        }
        if (wallsV.GetWithDefault(pos + Vector2Int.down, false)) {
            lefts++;
        }
        if (lefts >= 2) {
            return true;
        }
        var rights = 0;
        if (wallsH.GetWithDefault(pos + Vector2Int.right, false)) {
            rights++;
        }
        if (wallsV.GetWithDefault(pos + Vector2Int.right, false)) {
            rights++;
        }
        if (wallsV.GetWithDefault(pos +  Vector2Int.right + Vector2Int.down, false)) {
            rights++;
        }
        return rights >= 2;
    }

    private void EachNearby(Vector2Int currentPos, bool includeExtras, Action<Vector2Int> action) {
        var pos = new Vector2Int();
        var startX = currentPos.x - blockSize;
        var startY = currentPos.y - blockSize;
        if (includeExtras) {
            startX--;
            startY--;
        }
        for (pos.x = startX; pos.x < currentPos.x + blockSize; pos.x++) {
            for (pos.y = startY; pos.y < currentPos.y + blockSize; pos.y++) {
                action(pos);
            }
        }
    }

    private void Clear() {
        foreach (var tile in tiles.Values) {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
        wallsH.Clear();
        wallsV.Clear();
    }

}