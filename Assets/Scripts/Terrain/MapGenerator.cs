using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System;

public class MapGenerator : MonoBehaviour {

	//A reference to the prefab that is used for floor tiles
	public GameObject floorTile;
	//A reference to the prefab that is used for wall tiles
	public GameObject wallTile;

	//A multiplier for the offset of the coordinate tiles from each other: i.e = 0.14, means each yile is 0.14 units from each other
	public float offset = 0.14f;

	//The width and height of the map
	public int width = 0;
	public int height = 0;

	//The number of walls tthat must be neighbouring this tile, for it to also be a wall tile.
	public int numberOfNeighbouringWalls = 4;

	//A string that is used to generate the map, so if you use the same seed, the same map will be generated
	public string seed;
	//Whether or not to generate a new random seed every time
	public bool useRandomSeed = true;
	//The length of the random seed to generate
	public int randomSeedLength = 8;

	//The percent of the map that will be filled with walls
	//Since it's a percent it's confined to the range of 0 to 100
	[Range(0, 100)]
	public int randomFillPercent;
	public int numberOfSmoothings = 5;

	//Map defines a grid of integers
	int[,] map;
	//A new Random number generator
	private System.Random random = new System.Random();

	//Generate the map on load
	void Start() {
		GenerateMap();
	}

	void Update() {
		if(Input.GetKey(KeyCode.Space)) {

//			for(int i = 0; i < this.transform.childCount; i++) {
//				Transform child = this.transform.GetChild(i);
////				GameObject.Destroy(child);
//				child.de
//			}

			GenerateMap();
		}
	}

	//The function that generates the map
	void GenerateMap() {
		//Initialise the map array with the width and height
		map = new int[width, height];
		RandomFillMap();

		//Smooth the shapes in the map the specified number of times (default of 5)
		for (int i = 0; i < numberOfSmoothings; i++) {
			SmoothMap();
		}

		drawTiles();
	}

	void RandomFillMap() {

		if (useRandomSeed) {
			//Can use some other way to generate a seed later.
//			seed = Time.time.ToString();
			seed = generateSeed(randomSeedLength);
		}
		//Create a new pseudo random number generator using the unique identifier for the string seed (hash code)
		System.Random randomNumberGenerator = new System.Random(seed.GetHashCode());

		//Iterate over every coordinate in the map:
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				//If the coordinate is an edge or a corner it's always a wall:
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map[x, y] = 1;
				} else {
					//This means that we generate a random number between 0 & 100, if the number is less than the randomFillPercent,
					//we set the coordinate's value in the map to 1 (a wall), otherwise we set it to zero (the floor).
					map[x, y] = (randomNumberGenerator.Next(0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}
	}

	//Smooth map makes the shapes of the caves generated more natural looking
	void SmoothMap() {

		//Iterate over every coordinate in the map:
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				//Get the number of surrounding wall tiles to this tile (function below)
				int neighbourWallTiles = GetSurroundingWallCount(x, y);

				//If this tile is surrounded by 4 or more walls, set it to a wall.
				if(neighbourWallTiles > numberOfNeighbouringWalls) {
					map[x, y] = 1;
				} else if(neighbourWallTiles < numberOfNeighbouringWalls) {
					//If there are less than 4 wall tiles around this tile, make it a floor tile
					map[x, y] = 0;
				}

			}
		}

	}

	//A function that calculates the number of neighbouring walls to the given coordinate
	int GetSurroundingWallCount(int gridX, int gridY) {
		//Keep track of the number of neighbouring walls
		int wallCount = 0;

		//Iterate through all the surrounding coordinates: in a 3*3 grid centred on the given x,y
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
				//Make sure the coordinates are within the map:
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					//If one coordinate isn't equal to the otherone, we know we aren't examining the inputed coordinate
					if (neighbourX != gridX || neighbourY != gridY) {
						//Add the vlue of the coordinate to the count, a floor is 0 so adding floors does nothing
						wallCount += map[neighbourX, neighbourY];

					}
				} else {
					//The edge of the map is always a wall...
					wallCount++;
				}
			}
		}
		//Ya...
		return wallCount;

	}

	void drawTiles() {
		//If the map contains values
		if (map != null) {

			//Iterate over every coordinate in the map:
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {

					//This sets the gizmo colour to black if the value of map at this coordiante is 1 (a wall), otherwise it's
					//colour is white (the floor)
					GameObject tile = (map[x, y] == 1) ? wallTile : floorTile;

					//Create the coordinate of the new tile
					Vector2 position = new Vector2((-width / 2 + x + 0.5f) * offset, (-height / 2 + y + 0.5f) * offset);
					//Instantiate the object in the game
					GameObject instance = GameObject.Instantiate(tile);
					//Set it's coordinate to the tile coordinate
					instance.transform.position = position;
					//Set it's parent to be the Map Generator Object (the object the script is attached to) to declutter the hierarchy
					instance.transform.SetParent(this.transform);
//					Gizmos.DrawCube(position, Vector3.one);

				}
			}

		}

	}

	//A function that simply generates a random string of a given length
	public string generateSeed(int length) {
		//All of the characters that a seed can possibly contain
		string allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-";
		//An array of each character in the seed
		char[] stringCharacters = new char[length];

		for(int i = 0; i < length; i++) {
			//For every character in the seed, randomly select a character to be in it's position.
			stringCharacters[i] = allowedCharacters[random.Next(allowedCharacters.Length)];
		}
		//convert the array to a string
		string seed = new String(stringCharacters);
		return seed;
	}

//	void onDrawGizmos() {
//		//If the map contains values
//		if (map != null) {
//
//			//Iterate over every coordinate in the map:
//			for (int x = 0; x < width; x++) {
//				for (int y = 0; y < height; y++) {
//
//					//This sets the gizmo colour to black if the value of map at this coordiante is 1 (a wall), otherwise it's
//					//colour is white (the floor)
//					Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
//
//					Vector3 position = new Vector3((-width / 2 + x + 0.5f) * offset, 0f, (-height / 2 + y + 0.5f) * offset);
//
//					Gizmos.DrawCube(position, Vector3.one);
//
//				}
//			}
//
//		}
//
//	}



}
