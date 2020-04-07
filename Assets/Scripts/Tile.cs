using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    public bool wallUp, wallDown, wallLeft, wallRight;

    public Sprite cornerTR, cornerTL, cornerBL, cornerBR,
        tL, tR, tT, tB,
        straightH, straightV,
        cross;

    private SpriteRenderer sr;

    private SpriteRenderer SR {
        get {
            if (sr == null) {
                sr = GetComponent<SpriteRenderer>();
            }
            return sr;
        }
    }

    public void Refresh() {
        if (!wallUp && !wallDown && !wallLeft && !wallRight) {
            SR.sprite = cross;
            return;
        }
        //T
        if (wallUp && !wallDown && !wallLeft && !wallRight) {
            SR.sprite = tT;
            return;
        }
        if (!wallUp && wallDown && !wallLeft && !wallRight) {
            SR.sprite = tB;
            return;
        }
        if (!wallUp && !wallDown && wallLeft && !wallRight) {
            SR.sprite = tL;
            return;
        }
        if (!wallUp && !wallDown && !wallLeft && wallRight) {
            SR.sprite = tR;
            return;
        }
        //Corner
        if (wallUp && !wallDown && wallLeft && !wallRight) {
            SR.sprite = cornerTL;
            return;
        }
        if (wallUp && !wallDown && !wallLeft && wallRight) {
            SR.sprite = cornerTR;
            return;
        }
        if (!wallUp && wallDown && wallLeft && !wallRight) {
            SR.sprite = cornerBL;
            return;
        }
        if (!wallUp && wallDown && !wallLeft && wallRight) {
            SR.sprite = cornerBR;
            return;
        }

        //straight
        if (wallUp && wallDown && !wallLeft && !wallRight) {
            SR.sprite = straightH;
            return;
        }
        if (!wallUp && !wallDown && wallLeft && wallRight) {
            SR.sprite = straightV;
            return;
        }
        Debug.LogWarning("invalid walls: " + wallUp + wallDown + wallLeft + wallRight);
    }
}