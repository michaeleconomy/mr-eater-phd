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
        if (lastPosition == position) {
            return;
        }
        lastPosition = position;
        SetDirection(RandomDirection());
    }

    private Vector2Int RandomDirection() {
        switch (Rand.R(0, 4)) {
            case 0:
                return Vector2Int.left;
            case 1:
                return Vector2Int.right;
            case 2:
                return Vector2Int.up;
            case 3:
                return Vector2Int.down;
        }
        Debug.LogWarning("invalid rand value");
        return Vector2Int.down;
    }

}