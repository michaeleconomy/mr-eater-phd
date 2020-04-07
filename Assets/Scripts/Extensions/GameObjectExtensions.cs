using UnityEngine;
using System.Collections;

public static class GameObjectExtensions {
    public static void SetLayerRecursive(this GameObject go, int layer) {
        go.layer = layer;
        for (var i = 0; i < go.transform.childCount; i++) {
            var child = go.transform.GetChild(i);
            child.gameObject.SetLayerRecursive(layer);
        }
    }
}
