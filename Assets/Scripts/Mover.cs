using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller))]
public class Mover : MonoBehaviour {
    public float speed, turnArea;
    public bool stopped = true;
    private Controller controller;
    private Player player;
    private Vector2Int lastPlayerPosition;
    private Tile currentTile;

    private void Awake() {
        controller = GetComponent<Controller>();
        player = GetComponent<Player>();
    }

    public Vector2Int Position {
        get { return Vector2Int.RoundToInt(transform.position); }
    }

    private void Update() {
        if (GameManager.paused) {
            return;
        }
        var tile = Tiler.instance.GetTile(Position);
        if (tile == null) {
            stopped = true;
            return;
        }
        if (CanMove(controller.NextDirection, tile)) {
            controller.MoveNextDirection();
            var position = transform.position;
            if (controller.Direction == Vector2Int.up || controller.Direction == Vector2Int.down) {
                position.x = tile.transform.position.x;
            }
            else {
                position.y = tile.transform.position.y;
            }
            transform.position = position;
        }
        else if (!CanMove(controller.Direction, tile)) {
            stopped = true;
            return;
        }
        if (controller.Direction == Vector2Int.zero) {
            stopped = true;
            return;
        }
        stopped = false;
        var distance = Mathf.Min(1, speed * Time.deltaTime);
        var movement = new Vector3(controller.Direction.x * distance, controller.Direction.y * distance, 0);
        transform.position += movement;

        if (player != null) {
            var pos = Position;
            if (lastPlayerPosition != pos) {
                lastPlayerPosition = pos;
                Tiler.instance.DoNearby(pos);
            }
        }
    }

    public bool CanMove(Vector2Int direction, Tile tile) {
        if (direction == Vector2Int.left) {
            return transform.position.x > tile.transform.position.x || !tile.wallLeft;
        }
        if (direction == Vector2Int.right) {
            return transform.position.x < tile.transform.position.x || !tile.wallRight;
        }
        if (direction == Vector2Int.up) {
            return transform.position.y < tile.transform.position.y || !tile.wallUp;
        }
        if (direction == Vector2Int.down) {
            return transform.position.y > tile.transform.position.y || !tile.wallDown;
        }
        if (direction == Vector2Int.zero) {
            return true;
        }
        throw new Exception("direction not supported: " + direction);
    }

}