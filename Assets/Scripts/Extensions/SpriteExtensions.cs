using System;
using UnityEngine;

public static class SpriteTextureExtensions {

    public static Sprite TrimAlpha(this Sprite sprite) {
        var trimmedTexture = sprite.texture.TrimAlpha();
        return Sprite.Create(
            trimmedTexture,
            new Rect(0,0, trimmedTexture.width, trimmedTexture.height),
            Vector2.zero);
    }

    public static Texture2D TrimAlpha(this Texture2D texture) {
        var padding = texture.GetPadding();

        var croppedTexture = new Texture2D(padding.width, padding.height);

        var pixels = texture.GetPixels(
            padding.x, padding.y,
            padding.width, padding.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

    private static RectInt GetPadding(this Texture2D image) {
        var xPadding = 0;
        for (var x = 0; x < image.width - 1; x++) {
            var colorSeen = false;
            for (var y = 0; y < image.height; y++) {
                var pixel = image.GetPixel(x, y);
                if (pixel.a > 0f) {
                    colorSeen = true;
                    break;
                }
            }
            if (colorSeen) {
                break;
            }
            xPadding = x;
        }


        var yPadding = 0;
        for (var y = 0; y < image.height - 1; y++) {
            var colorSeen = false;
            for (var x = 0; x < image.width; x++) {
                var pixel = image.GetPixel(x, y);
                if (pixel.a > 0f) {
                    colorSeen = true;
                    break;
                }
            }
            if (colorSeen) {
                break;
            }
            yPadding = y;
        }


        var zPadding = image.width - 1;
        for (var x = image.width - 1; x > xPadding; x--) {
            var colorSeen = false;
            for (var y = 0; y < image.height; y++) {
                var pixel = image.GetPixel(x, y);
                if (pixel.a > 0f) {
                    colorSeen = true;
                    break;
                }
            }
            if (colorSeen) {
                break;
            }
            zPadding = x;
        }

        var wPadding = image.height - 1;
        for (var y = image.height-1; y > yPadding; y--) {
            var colorSeen = false;
            for (var x = 0; x < image.width; x++) {
                var pixel = image.GetPixel(x, y);
                if (pixel.a > 0f) {
                    colorSeen = true;
                    break;
                }
            }
            if (colorSeen) {
                break;
            }
            wPadding = y;
        }


        return new RectInt(xPadding, yPadding,
            zPadding - xPadding,
            wPadding - yPadding);
    }
}
