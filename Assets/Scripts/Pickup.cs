using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour {
    public int points;

    public void Grab() {
        Destroy(gameObject);
        GameManager.instance.Score(points);
    }
}