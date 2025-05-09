using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Noise {

	public static int[] GenerateNoiseMap(int mapWidth, int seed, float scale, int mapMaxHeight, int tempOffsetX, TBiome biome) {

		int[] noiseMap = new int[mapWidth];
		bool[] flatZone = new bool[mapWidth];
		
		// Marking flat zones
		foreach (Tstructures structure in biome.lstStructures)
		{
			// Compute the coordinates in the flatZone list and apply an offset
			int localStartPos = structure.spawnCord - tempOffsetX - structure.length / 2;
			int localEndPos = localStartPos + structure.length + structure.length / 2;

			// Make sure that start and end are in the bounds
			int start = Mathf.Max(0, localStartPos);
			int end = Mathf.Min(mapWidth - 1, localEndPos);

			for (int i = start; i <= end; i++)
			{
				flatZone[i] = true;
			}
		}


		System.Random prng = new System.Random(seed);
		float offsetX = prng.Next(-100000, 100000);
		if (scale <= 0) scale = 0.0001f;
		int previousHeight = 1;

		for (int x = 0; x < mapWidth; x++) {
			float sampleX = (x + tempOffsetX) / scale + offsetX;
			int perlinValue = Mathf.FloorToInt(Mathf.PerlinNoise(sampleX, 0) * mapMaxHeight);

			if (flatZone[x]) {
				perlinValue = biome.mapMaxHeight / 2; 
			} 


			previousHeight = perlinValue;
			noiseMap[x] = perlinValue;
		}


		return noiseMap;
	}

}