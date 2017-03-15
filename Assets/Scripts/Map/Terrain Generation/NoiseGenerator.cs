using UnityEngine;
using SimplexNoise;

public class NoiseGenerator {

    private float scale;
    private int max;

    public NoiseGenerator(float scale, int max) {

        //Scale is 1 / the distance between two peaks
        this.scale = scale;
        this.max = max;

    }

    public float getScale() {
        return this.scale;
    }

    public int getMax() {
        return this.max;
    }

    public int generateNoise(int x, int y, int z) {

        float scale = getScale();

        float xScale = x * scale;
        float yScale = y * scale;
        float zScale = z * scale;

        int noise = Mathf.FloorToInt((Noise.Generate(xScale, yScale, zScale) + 1f) * (max / 2f));

        return noise;

    }

    public static float generateNoise(int x, int y, int z, float scale, int max) {

        NoiseGenerator noiseGenerator = new NoiseGenerator(scale, max);
        return noiseGenerator.generateNoise(x, y, z);

    }
   
}
