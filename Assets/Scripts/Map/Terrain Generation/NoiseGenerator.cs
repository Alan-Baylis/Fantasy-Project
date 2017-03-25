using UnityEngine;
using SimplexNoise;

//A simple handler class for using the noise generation provided by simplex noise
public class NoiseGenerator {

    //Scale is the reciprocal of the wavelength
    private float scale;
    //Max is the amplitude if it were a wave
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

    //Generates an integer value that represents the height of a terrain value at a given coordinate, by using noise, 
    //it ensures the same inputed values always get the same result
    public int generateNoise(int x, int y, int z) {

        float scale = getScale();

        //Scale all the x, y, z coordinates
        float xScale = x * scale;
        float yScale = y * scale;
        float zScale = z * scale;

        //Generate the noise for the given coordinate, this value is between [-1, 1] so add 1 for it to always be >=0, then 
        //Scale the value by 0.5*the max value achievable (ie the range is [0, 2] so the range of the final value is [0, max])
        int noise = Mathf.FloorToInt((Noise.Generate(xScale, yScale, zScale) + 1f) * (max / 2f));

        return noise;

    }

    //Performs the same functionality as a NoiseGenerator object without having to have an instance of NoiseGenerator, useful for
    //a single use noise generation
    public static int generateNoise(int x, int y, int z, float scale, int max) {

        NoiseGenerator noiseGenerator = new NoiseGenerator(scale, max);
        return noiseGenerator.generateNoise(x, y, z);

    }
   
}
