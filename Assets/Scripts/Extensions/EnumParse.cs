using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public static class EnumParse {
    public static T ParseIgnoreCase<T>(string s) where T : Enum {
        var lower = s.ToLower();
        foreach (var val in Enum.GetValues(typeof(T)).Cast<T>()) {
            if (val.ToString().ToLower() == lower) {
                return val;
            }
        }
        throw new FormatException();
    }

    public static bool TryParseIgnoreCase<T>(string s, out T result) where T : Enum {
        try {
            result = ParseIgnoreCase<T>(s);
            return true;
        }
        catch (FormatException) { }
        result = default(T);
        return false;
    }
}
