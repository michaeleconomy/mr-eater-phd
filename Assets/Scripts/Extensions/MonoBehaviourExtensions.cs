using System;
using UnityEngine;

public static class MonoBehaviourExtensions {
    public static T FindChildByName<T>(this MonoBehaviour monoBehaviour, string name) where T : Component {
        var ts = monoBehaviour.GetComponentsInChildren<T>(true);
        foreach (var t in ts) {
            if (t.name == name) {
                return t;
            }
        }
        return null;
    }
}
