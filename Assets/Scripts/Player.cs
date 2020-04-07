using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        var enemy = other.GetComponent<Enemy>();
        if (enemy) {
            Debug.Log("Enemy Collision");
            return;
        }
        var pickup = other.GetComponent<Pickup>();
        if (pickup) {
            pickup.Grab();
            return;
        }
        Debug.Log("unknown collision" + other.name);
    }
}