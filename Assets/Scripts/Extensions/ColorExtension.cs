using System;
using System.IO;
using UnityEngine;

public static class ColorExtension {

    public static Color Random(Color min, Color max) {
        Color.RGBToHSV(min, out var hMin, out var sMin, out var vMin);
        Color.RGBToHSV(max, out var hMax, out var sMax, out var vMax);

        return UnityEngine.Random.ColorHSV(hMin, hMax, sMin, sMax, vMin, vMax);
    }


    public static string Serialize(this Color color) {
        return color.r + "," + color.g + "," + color.b + "," + color.a;
    }

    public static Color Deserialize(string s) {
        var parts = s.Split(",");
        if (parts.Length != 4) {
            Debug.LogWarning("Color Deserialize Error: " + s);
            return Color.white;
        }

        if (!float.TryParse(parts[0], out var r)) {
            Debug.LogWarning("Color Deserialize Error: " + s);
        }
        if (!float.TryParse(parts[1], out var g)) {
            Debug.LogWarning("Color Deserialize Error: " + s);
        }
        if (!float.TryParse(parts[2], out var b)) {
            Debug.LogWarning("Color Deserialize Error: " + s);
        }
        if (!float.TryParse(parts[3], out var a)) {
            Debug.LogWarning("Color Deserialize Error: " + s);
        }
        return new Color(r, g, b, a);
    }
}
