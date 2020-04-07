using UnityEngine;
using System.Collections;

public static class RectIntExtensions {
    public static RectInt SpacesInRange(this RectInt rect, int range) {
        return new RectInt(rect.xMin - range,
            rect.yMin - range,
            rect.width + 2 * range,
            rect.height + 2 * range);
    }
}
