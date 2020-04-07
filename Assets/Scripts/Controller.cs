using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Mover))]
public abstract class Controller : MonoBehaviour  {
    private Vector2Int direction, nextDirection;
    protected Mover mover;

    private void Awake() {
        mover = GetComponent<Mover>();
    }

    public Vector2Int Direction {
        get { return direction; }
    }

    public Vector2Int NextDirection {
        get { return nextDirection; }
    }

    public void MoveNextDirection() {
        direction = nextDirection;
    }

    protected virtual void SetDirection(Vector2Int newDirection) {
        nextDirection = newDirection;
        if (newDirection == direction) {
            return;
        }
        if (newDirection == direction * -1) {
            direction = newDirection;
        }
    }
}