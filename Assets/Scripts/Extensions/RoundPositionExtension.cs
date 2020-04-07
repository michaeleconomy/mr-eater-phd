using UnityEngine;

public static class RoundPositionExtension {

    public static Vector2 RoundPosition(this Vector2 v) {
        return new Vector2(Round(v.x), Round(v.y));
    }

    public static Vector3 RoundPosition(this Vector3 v) {
        return new Vector3(Round(v.x), Round(v.y), 0);
    }

    public static Vector2 RoundPosition(this Vector2Int v) {
        return new Vector2(Round(v.x), Round(v.y));
    }

    public static Vector3 RoundPosition(this Vector3Int v) {
        return new Vector3(Round(v.x), Round(v.y), 0);
    }

    private static float Round(float f) {
        return Mathf.Floor(f) + 0.5f;
    }


}
