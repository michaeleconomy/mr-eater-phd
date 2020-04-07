using System;
using System.IO;
using UnityEngine;

[Serializable]
public class ColorRange {
    public Color color1, color2;
    public Color Random() {
        return ColorExtension.Random(color1, color2);
    }
}
