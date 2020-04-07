public static class Modulus {
    public static float Mod(float a, float b) {
        float c = a % b;
        if ((c < 0 && b > 0) || (c > 0 && b < 0)) {
            c += b;
        }
        return c;
    }
}