using UnityEngine;
using System.Collections.Generic;

public static class Noise {

	public static int[] GenerateNoiseMap(int mapWidth, int seed, int tempOffsetX, TBiome biome, int flatHeight) {

		int[] noiseMap = new int[mapWidth];
		bool[] flatZone = new bool[mapWidth];
		float[] stairsWeight = new float[mapWidth];

		// tag flat zone and stairs / leveling off
		foreach (Tstructures structure in biome.lstStructures)
		{
			int localStartPos = structure.spawnCord - tempOffsetX - structure.length / 2;
			int localEndPos = localStartPos + structure.length + structure.length / 2;

			int start = Mathf.Clamp(localStartPos, 0, mapWidth - 1);
			int end = Mathf.Clamp(localEndPos, 0, mapWidth - 1);

			for (int i = start; i <= end; i++) {
				flatZone[i] = true;
			}

			// compute the weight for the lepr 
			for (int i = 1; i <= flatHeight; i++) {
				if (start - i >= 0)
					stairsWeight[start - i] = 1f - i / (float)flatHeight;
				if (end + i < mapWidth)
					stairsWeight[end + i] = i / (float)flatHeight;
			}
		}

		System.Random prng = new System.Random(seed);
		float offsetX = prng.Next(-100000, 100000);

		int ComputePerlinValue(int x) {
			float sampleX = (x + tempOffsetX) / (float)biome.noiseScale + offsetX;
			return Mathf.FloorToInt(Mathf.PerlinNoise(sampleX, 0) * biome.mapMaxHeight);
		}

		for (int x = 0; x < mapWidth; x++) {
			int rawPerlin = ComputePerlinValue(x);

			if (flatZone[x]) {
				noiseMap[x] = flatHeight;
			} else if (stairsWeight[x] > 0f) { // Leveling off
				// use of lepr to have the right height betwen (rawPerlin, flatHeight) with the weight 
				noiseMap[x] = Mathf.RoundToInt(Mathf.Lerp(rawPerlin, flatHeight, stairsWeight[x]));
			} else {
				noiseMap[x] = rawPerlin;
			}
		}

		return noiseMap;
	}
}
