using System;
using UnityEngine;

public static class RangeExtensions {
    public static bool Contains(this RangeInt range, int number) {
        return number >= range.start && number < range.end;
    }
}
