using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorRanges {
    public List<ColorRange> ranges;

    public Color Random() {
        return ranges.RandomElement().Random();
    }
}
