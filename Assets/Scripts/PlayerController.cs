using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Controller {
    public float minDragDistance, inputDecaySpeed;
    private Vector2 aggregateTravel, lastMousePosition;
    private bool dragging;
    
    private void Update() {
        if (!Input.GetMouseButton(0)) {
            dragging = false;
            return;
        }
        if (!dragging) {
            aggregateTravel = Vector2.zero;
            dragging = true;
            lastMousePosition = Input.mousePosition;
            return;
        }
        var newMousePosition = (Vector2)Input.mousePosition;
        var decay = inputDecaySpeed * Time.deltaTime;
        aggregateTravel = aggregateTravel * decay + newMousePosition - lastMousePosition;
        lastMousePosition = newMousePosition;

        if (Mathf.Abs(aggregateTravel.x) > Mathf.Abs(aggregateTravel.y)) {
            if (aggregateTravel.x >= minDragDistance) {
                SetDirection(Vector2Int.right);
                return;
            }

            if (aggregateTravel.x <= -minDragDistance) {
                SetDirection(Vector2Int.left);
                return;
            }
            return;
        }
        if (aggregateTravel.y >= minDragDistance) {
            SetDirection(Vector2Int.up);
            return;
        }

        if (aggregateTravel.y <= -minDragDistance) {
            SetDirection(Vector2Int.down);
            return;
        }
    }

    protected override void SetDirection(Vector2Int newDirection) {
        base.SetDirection(newDirection);
        aggregateTravel = Vector2Int.zero;
    }

}