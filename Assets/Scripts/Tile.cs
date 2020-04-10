using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    public GameObject[] specialPickupPrefabs;
    public Pickup defaultPickupPrefab;
    public GameObject[] enemyPrefabs;

    public float enemyChance, specialChance;

    public bool wallUp, wallDown, wallLeft, wallRight;

    public Sprite cornerTR, cornerTL, cornerBL, cornerBR,
        tL, tR, tT, tB,
        straightH, straightV,
        cross, full;

    private SpriteRenderer sr;

    private SpriteRenderer SR {
        get {
            if (sr == null) {
                sr = GetComponent<SpriteRenderer>();
            }
            return sr;
        }
    }

    private void Start() {
        if (wallUp && wallDown && wallLeft && wallRight) {
            return;
        }
        var specialPrefab = SpecialType();
        if (specialPrefab != null) {
            Instantiate(specialPrefab, transform.position, Quaternion.identity);
        }

        if (specialPrefab == null || specialPrefab.GetComponent<Mover>() != null) {
            Instantiate(defaultPickupPrefab, transform.position, Quaternion.identity);
        }
        if (!wallRight) {
            var rightSide = transform.position;
            rightSide.x += .5f;
            Instantiate(defaultPickupPrefab, rightSide, Quaternion.identity);
        }
        if (!wallDown) {
            var rightSide = transform.position;
            rightSide.y -= .5f;
            Instantiate(defaultPickupPrefab, rightSide, Quaternion.identity);
        }
    }

    private GameObject SpecialType() {
        if (Rand.P(enemyChance)) {
            return enemyPrefabs.RandomElement();
        }
        else if (Rand.P(specialChance)) {
            return specialPickupPrefabs.RandomElement();
        }
        return null;
    }

    public void Refresh() {
        if (!wallUp && !wallDown && !wallLeft && !wallRight) {
            SR.sprite = cross;
            return;
        }
        if (wallUp && wallDown && wallLeft && wallRight) {
            SR.sprite = full;
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