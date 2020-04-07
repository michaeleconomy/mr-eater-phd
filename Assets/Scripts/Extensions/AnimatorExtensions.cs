using System;
using System.Collections;
using UnityEngine;

public static class AnimatorExtensions {

    public static IEnumerator WaitForFinished(this Animator animator,
                                                string stateName,
                                                int layerIndex = 0) {
        while (!animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName)) {
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName)) {
            yield return null;
        }
    }
}
