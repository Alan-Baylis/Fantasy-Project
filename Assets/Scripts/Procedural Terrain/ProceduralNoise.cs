using SimplexNoise;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralNoise {

    public enum NormalizeMode {
        LOCAL,
        GLOBAL
    }

    public static float[,] generateNoiseMap(int mapWidth, int mapHeight, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode) {

        System.Random random = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;

        for(int i = 0; i < octaves; i++) {
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetY = random.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;

        } 


        float[,] noiseMap = new float[mapWidth, mapHeight];

        if(scale <= 0) {
            scale = 0.001f;
        }

        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {

                amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++) {

                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                    //float noiseValue = (Noise.Generate(sampleX, sampleY) + 1f) * (1 / 2f);
                    //float noiseValue = Noise.Generate(sampleX, sampleY);
                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += noiseValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;

                }

                if(noiseHeight > maxLocalNoiseHeight) {
                    maxLocalNoiseHeight = noiseHeight;
                } else if(noiseHeight < minLocalNoiseHeight) {
                    minLocalNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {

                if(normalizeMode == NormalizeMode.LOCAL) {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                } else {
                    float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight / 1.5f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0,  int.MaxValue);
                }

            }
        }

        return noiseMap;
    }

}
