using UnityEngine;
using System.Collections;

//This static class generates a falloff map of given size, fall off maps can be used to generate individual "continents" on each mesh
public static class FalloffGenerator {

    //A FallOff map is a height map with values close to 0 at the centre of the map and the values get closer and closer to 1 as 
    //you near the edge of the map

    //The function takes in the dimensions of the fallOffMap, has to be a square, aswell as two variables for the function that 
    //generates height values in the map
    public static float[,] generateFalloffMap(int size, float a, float b) {

        //Create a new empty heigh map of the given dimensions
        float[,] falloffMap = new float[size, size];

        //Iterate over all coords in the map
        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {

                //Get the x value as a fraction of it's position all the length of the map in the range (-1, 1)
                float x = (i / (float) size) * 2 - 1;
                //same idea for y value
                float y = (j / (float) size) * 2 - 1;

                //The value to use in the function is the most dominant value between x and y
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                //Set the value at this coord to be the calcuated value from the function using the parameters gven by the input
                falloffMap[i, j] = falloffCurve(value, a, b);

            }
        }
        //return the generated fall off map
        return falloffMap;

    }

    //The function that generates a value between (0, 1) from the inputed value and two variables a & b
    private static float falloffCurve(float value, float a, float b) {

        //Uses function:
        //f(x) = x^a / (x^a + (b - bx)^a)

        //Calculate the value x^a, as it is repeated in the function and it is more efficient to calculate the value once
        float valuePowA = Mathf.Pow(value, a);
        //Return the value from the function as it is described above
        return valuePowA / (valuePowA + Mathf.Pow(b - b * value, a));

    }

}
