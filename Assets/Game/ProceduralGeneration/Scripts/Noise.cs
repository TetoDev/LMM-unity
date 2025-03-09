using UnityEngine;
using System.Collections;

public static class Noise {

	public static int[] GenerateNoiseMap(int mapWidth, int seed, float scale, int mapMaxHeight, int tempOffsetX) {
		int[] noiseMap = new int[mapWidth];

		System.Random prng = new System.Random (seed);
		float offsetX = prng.Next (-100000, 100000) + tempOffsetX * 1 / scale;

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float halfWidth = mapWidth / 2f;

		for (int x = 0; x < mapWidth; x++) {

			float sampleX = (x-halfWidth) / scale + offsetX;
			float perlinValue = Mathf.PerlinNoise (sampleX, 0);

			noiseMap [x] = Mathf.RoundToInt(perlinValue * mapMaxHeight);
		}



		return noiseMap;
	}

}