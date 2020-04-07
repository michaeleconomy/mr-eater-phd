using UnityEngine;
using System.Collections;
using System.Globalization;
using System;

public static class StringExtensions {
    public static string ToTitleCase(this string s) {
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
    }
    public static string[] Split(this string s, string by) {
        return s.Split(new string[] { by }, StringSplitOptions.None);
    }
}
