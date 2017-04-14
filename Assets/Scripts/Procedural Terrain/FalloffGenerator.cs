using UnityEngine;
using System.Collections;

public static class FalloffGenerator {

    public static float[,] generateFalloffMap(int size, float a, float b) {

        float[,] falloffMap = new float[size, size];

        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {

                float x = (i / (float) size) * 2 - 1;
                float y = (j / (float) size) * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

                falloffMap[i, j] = falloffCurve(value, a, b);

            }
        }

        return falloffMap;

    }

    private static float falloffCurve(float value, float a, float b) {

        //Uses function:
        //f(x) = x^a / (x^a + (b - bx)^a)

        float valuePowA = Mathf.Pow(value, a);

        return valuePowA / (valuePowA + Mathf.Pow(b - b * value, a));

    }

}
