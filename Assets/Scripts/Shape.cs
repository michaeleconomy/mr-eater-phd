
using UnityEngine;

public static class Shape {
    public static bool[,] Flip(this bool[,] shape) {
        var width = shape.GetLength(0);
        var height = shape.GetLength(1);
        var flipped = new bool[width, height];
        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                flipped[x, y] = shape[width - (x + 1), y];
            }
        }
        return flipped;
    }

    public static bool[,] Rotate(this bool[,] shape) {
        var width = shape.GetLength(0);
        var height = shape.GetLength(1);
        var rotated = new bool[height, width];

        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                rotated[y, x] = shape[x, height - (y + 1)];
            }
        }
        return rotated;
    }
}