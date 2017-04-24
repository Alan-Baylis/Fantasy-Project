using SimplexNoise;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralNoise {

    //The normalize modes are used for scaling of all heights in the mesh:
    public enum NormalizeMode {
        //LOCAL normalize mode scales all heights based off of the highest and lowest values within each seperate mesh, where each esh has it's 
        //own values
        LOCAL,
        //GLOBAL normalize mode scales all height values across all meshes with an estimate for the average highest and lowest mesh heights
        GLOBAL
    }

    //This function generates the heightMap that represents terrain in each mesh, the mapWidth and mapHeight should be equal for a square mesh,
    //The scale is how much terrain fits into each mesh, the bigger the value, the larger the terrain is and the less that fits in one mesh,
    //The seed guratess that the same terrain is generated every time using the same seed,
    //The octaves control how many times we overlay heights onto the same coordintes, like octaves on a musical note,
    //Persistance affects the strength/influence of the amplitude on each successive octave,
    //lacunarity affects the scaling down of the frequency across each octave,
    //The offset shifts the entire map by the offset value,
    //The normalizemode is descibed above and scales the heights based off of highest and lowest values
    public static float[,] generateNoiseMap(int mapWidth, int mapHeight, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode) {

        //Create a new pseudo random number generator using the seed, so that the same numbers will always generated in the same order
        System.Random random = new System.Random(seed);
        //Create an array of offsets, one for each octave
        Vector2[] octaveOffsets = new Vector2[octaves];

        //Initialise the maxPossible height to 0
        float maxPossibleHeight = 0;
        //And the highest possible amplitude is 1
        float amplitude = 1;

        //iterate over all the octaves
        for(int i = 0; i < octaves; i++) {
            //The terrain generation across successive octaves looks more realistic if each octave is centered around a random spot, 
            //between (-100,000, 100,000), it is the offset by the supplied offset aswell
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetY = random.Next(-100000, 100000) - offset.y;
            //Assign the offset coordinate into the octaveOffsets array
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            //Increase the maxPossibleHeight by the amplitude
            maxPossibleHeight += amplitude;
            //Scale down the amplitude by the persistance
            amplitude *= persistance;

        }

        //Create a new empty noise map of the given dimensions
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //Scale can't be zero/negative so set it to a small value is it is
        if(scale <= 0) {
            scale = 0.001f;
        }

        //Initialise the local max and min heights as the min and max values respectively, so that there will be a value generated that
        //is guaranteed to be bigger/smaller than them
        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        //Calculate half the width and height once to save reaclculating them
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        //Iterate over all coordinates in the mesh
        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {

                //reset the amplitude to 1
                amplitude = 1;
                //initialise the frequency at 1
                float frequency = 1;
                //The noiseHeight starts at 0
                float noiseHeight = 0;
                //Iterate over all octaves for this coordinate
                for(int i = 0; i < octaves; i++) {
                    //Generate the sample noiseValues for the x & y coordinates
                    //we take away the halfWidth/height, so that the map scales to the centre of the map, 
                    //add the offset for that octave and then scale up the value by scale, and change it's frequency
                    //by multipling bby frequency
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                    //float noiseValue = (Noise.Generate(sampleX, sampleY) + 1f) * (1 / 2f);
                    //float noiseValue = Noise.Generate(sampleX, sampleY);
                    
                    //Generate a noise value for this coordinate, the default values range is (0, 1), so we multiply by 2 and -1
                    //to get it in the range (-1, 1)
                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    //Add this nois Value to the existing noiseHeight but scale it by the octaves amplitude
                    noiseHeight += noiseValue * amplitude;

                    //Scale both the amplitude and frequency of this octave by the persistance and lacunarity respectively
                    amplitude *= persistance;
                    frequency *= lacunarity;

                }

                //If this noiseHeight is a new max vale, replace the current max with this value
                if(noiseHeight > maxLocalNoiseHeight) {
                    maxLocalNoiseHeight = noiseHeight;
                //Same idea again for the min value
                } else if(noiseHeight < minLocalNoiseHeight) {
                    minLocalNoiseHeight = noiseHeight;
                }

                //Assign the noiseHeight at this coordinate
                noiseMap[x, y] = noiseHeight;
            }
        }

        //Now scale all the noise values based off of the normalize mode

        //Iterate over all coordinates
        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {

                //If the scaling is based off of max and mins for this mesh only:
                if(normalizeMode == NormalizeMode.LOCAL) {
                    //Inverse lerp finds the what fraction a value is between two anchors, ie:
                    //The value of noiseMap[x, y] between 0 & 1 if min was 0 & max was 1
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                //If the scaling is based off of a gloabl estimated maximum and minimum
                } else {
                    //The normalizedHeight is the generated value in the range (0, 1) => + 1,
                    //then scale it over 2*maxHeight divided by an estimated scale factor to give each value scaled between (0, 1) as a fraction of
                    //the max possible height
                    float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight / 1.66f);
                    //Clamp the value to be between 0 & the maximum possible number
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }

            }
        }
        //return the noiseMap
        return noiseMap;
    }

}
