using System;
using System.IO;
using UnityEngine;

public static class DirectoryHelper {

    public static void CreateForPath(string path) {
        var lastSlash = path.LastIndexOf('/');
        var relativeDirectory = path.Substring(0, lastSlash);
        var directory = Application.dataPath.Replace("Assets", relativeDirectory);
        Directory.CreateDirectory(directory);
    }
}
