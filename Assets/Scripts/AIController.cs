using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class AIController : Controller {
    private Vector2Int lastPosition;

    private void Update() {
        var position = mover.Position;
        if (lastPosition == position && !mover.stopped) {
            return;
        }
        lastPosition = position;
        SetDirection(GetNextDirection());
    }

    private Vector2Int GetNextDirection() {
        if (Direction == Vector2Int.up) {

            switch (Rand.R(0, 3)) {
                case 0:
                    return Vector2Int.left;
                case 1:
                    return Vector2Int.right;
                case 2:
                    return Vector2Int.up;
            }
            Debug.LogWarning("invalid rand value");
            return Vector2Int.up;
        }

        if (Direction == Vector2Int.down) {

            switch (Rand.R(0, 3)) {
                case 0:
                    return Vector2Int.left;
                case 1:
                    return Vector2Int.right;
                case 2:
                    return Vector2Int.down;
            }
            Debug.LogWarning("invalid rand value");
            return Vector2Int.down;
        }

        if (Direction == Vector2Int.left) {

            switch (Rand.R(0, 3)) {
                case 0:
                    return Vector2Int.left;
                case 1:
                    return Vector2Int.down;
                case 2:
                    return Vector2Int.up;
            }
            Debug.LogWarning("invalid rand value");
            return Vector2Int.left;
        }

        if (Direction == Vector2Int.right) {
            switch (Rand.R(0, 3)) {
                case 0:
                    return Vector2Int.down;
                case 1:
                    return Vector2Int.right;
                case 2:
                    return Vector2Int.up;
            }
            Debug.LogWarning("invalid rand value");
            return Vector2Int.right;
        }

        switch (Rand.R(0, 4)) {
            case 0:
                return Vector2Int.down;
            case 1:
                return Vector2Int.right;
            case 2:
                return Vector2Int.left;
            case 3:
                return Vector2Int.up;
        }
        Debug.LogWarning("invalid rand value");
        return Vector2Int.right;
    }

}