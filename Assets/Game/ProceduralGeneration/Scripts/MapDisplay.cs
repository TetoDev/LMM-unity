using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;

public class MapDisplay : MonoBehaviour {

	public int mapWidth;

	public int seed;
	public int offsetX;

	public bool autoUpdate;

	public TBiome biome;

	public SaveAndLoad saveAndLoad;

	public void GenerateMap() {
		int[] noiseMap = Noise.GenerateNoiseMap (mapWidth, seed, biome.noiseScale, biome.mapMaxHeight, offsetX);
		DrawMap(noiseMap);
	}

	public void DrawMap(int[] noiseMap){

		for (int i = 0 ; i < biome.map.Count ; i ++){
			biome.map[i].tilemap.ClearAllTiles();
		}

		int previousHeight = noiseMap[0];
		int currentHeight = noiseMap[1];
		int nextHeight = noiseMap[2];
		// find the id of the tile in the list List<displayElement> to assotiate it with a string key
		int idTerrain = reacherIndexInBiome("terrain");
		int idStructures = reacherIndexInBiome("structures");
		int idGrass = reacherIndexInBiome("grass");
		int idTree = reacherIndexInBiome("tree");
		int idDecors = reacherIndexInBiome("decors");
		List<string> LstIdTerrainTiles = saveAndLoad.BuildDicoIdTerrain(biome.name);
		Dictionary<string, int> IdTerrainTiles = new Dictionary<string, int>{};
		foreach (string key in LstIdTerrainTiles){
			Debug.Log(key);
			IdTerrainTiles[key] = reacherIndexInLstOfThisElementType(biome.map[idTerrain].lstOfThisElementType, key);
		}

		bool ShouldDrawScale(int prev, int curr) {
			return (Math.Abs(prev - curr) >= 2) && idTerrain >= 0;
		} 

		int reacherIndexInBiome(String name){
			int id = -1;
			for (int i = 0 ; i < biome.map.Count ; i++){
				if (biome.map[i].name == name){
					id = i;
					break;
				}
			}
			return id;
		}

		int reacherIndexInLstOfThisElementType(List<displayElement> lst, String name){
			int id = -1;
			for (int i = 0 ; i < lst.Count ; i++){
				if (lst[i].name == name){
					id = i;
					break;
				}
			}
			return id;
		}

		/*void int CountTileAtTheSameHeight(Vector3Int position ; int height = position.x){
			x = 0;
			while (noiseMap[x] == height){
				x += 1
			}
			return x
		}*/

		void SetTile(Vector3Int position, int idLstDisplay, int idElInLstDisplay, bool drawScale = false)
		{	
			displayElement element = biome.map[idLstDisplay].lstOfThisElementType[idElInLstDisplay];
			if (idLstDisplay >= 0 && idElInLstDisplay >= 0 && position.y >= element.minHeight && position.y <= element.maxHeight){
			biome.map[idLstDisplay].tilemap.SetTile(position, element.tile);
			if (drawScale)
			{
				biome.map[idTerrain].tilemap.SetTile(new Vector3Int(position.x - 1, position.y, 0), biome.map[idTerrain].lstOfThisElementType[IdTerrainTiles["scaleTile"]].tile);
			}
			}
		}

		void SetDecor(Vector3Int position, int x){
			void PlaceDecor(Vector3Int position, int idElement, int x, System.Random prng){
				int id = prng.Next (0,  biome.map[idElement].lstOfThisElementType.Count);
				SetTile(position, idElement, id);
			}
				
			System.Random prng = new System.Random (seed + offsetX + x);
			int random = prng.Next (1, 101);

			if (random <= 10 && idDecors >= 0){
				PlaceDecor(new Vector3Int(x, currentHeight, 0), idDecors, x, prng);
			} else if (random <= 30 && idTree >= 0) {
				PlaceDecor(new Vector3Int(x, currentHeight + 1, 0), idTree, x, prng);
			} else if (random <= 90 && idGrass >= 0){
				PlaceDecor(new Vector3Int(x, currentHeight, 0), idGrass, x, prng);
			}
  	  	}

		void SetTerrain(int x, int y, bool drawScale, int min, int max){
			Vector3Int position = new Vector3Int(x, y, 0);
			// first layer
			if (y == currentHeight - 1)
			{
				SetTile(position, idTerrain, IdTerrainTiles["topTile"]);
			}
			else
			{
				SetTile(position, idTerrain, IdTerrainTiles["tile"]);
			}

			// we over draw the cover tiles 
			if (Math.Abs(nextHeight - currentHeight) >= 1 || Math.Abs(previousHeight - currentHeight) >= 1)
			{
				if (max < currentHeight)
				{
					if (y == min - 1)
					{
						SetTile(position, idTerrain, IdTerrainTiles["bottomDoubleEdgeTile"], drawScale);
					}
					else if (y >= min && y < currentHeight - 1)
					{
						SetTile(position, idTerrain, IdTerrainTiles["doubleEdgeTile"], drawScale);
					}
					else if (y == currentHeight - 1)
					{
						SetTile(position, idTerrain, IdTerrainTiles["topDoubleEdgeTile"], drawScale);
					}
				}

				if (previousHeight < nextHeight && currentHeight != previousHeight)
				{
					if (y == previousHeight - 1)
					{
						SetTile(position, idTerrain, IdTerrainTiles["bottomLeftEdgeTile"]);
					}
					else if (y >= previousHeight && y < currentHeight - 1)
					{
						SetTile(position, idTerrain, IdTerrainTiles["leftEdgeTile"], drawScale);
					}
					else if (y == currentHeight - 1)
					{
						SetTile(position, idTerrain, IdTerrainTiles["topLeftEdgeTile"], drawScale);
					}
				}

				if (previousHeight > nextHeight && nextHeight != currentHeight)
				{
					if (y == nextHeight - 1)
					{
						SetTile(position, idTerrain, IdTerrainTiles["bottomRightEdgeTile"]);
					}
					else if (y >= nextHeight && y < currentHeight - 1)
					{
						SetTile(position, idTerrain, IdTerrainTiles["rightEdgeTile"]);
					}
					else if (y == currentHeight - 1)
					{
						SetTile(position, idTerrain, IdTerrainTiles["topRightEdgeTile"]);
					}
				}
			}
		}

		void SetBuilding(List<int> spawnCoordonate, Vector3Int position, int x){
			if (spawnCoordonate.Count > 0){
				
				//while (spawnCoordonate[0] <= x){
				//SetTile(position, idStructures, 1);
				//Debug.Log('');
				//spawnCoordonate.Remove(spawnCoordonate[0]);
			}
			//}	
		}

		List<int> spawnCoordonate = new List<int>{1};

		for (int x = 2; x < mapWidth - 1; x++)
		{
			int max = Math.Max(previousHeight, nextHeight);
			int min = Math.Min(previousHeight, nextHeight);
			bool drawScale = ShouldDrawScale(previousHeight, currentHeight);
			// random numbre for evry x and evry seed
			// place les décors :
			Vector3Int position = new Vector3Int(x, currentHeight, 0);
			SetDecor(position, x);
			SetBuilding(spawnCoordonate, position, x);
			for (int y = 0; y < currentHeight; y++)
			{
				SetTerrain(x, y, drawScale, min, max);
			}

			previousHeight = currentHeight;
			currentHeight = nextHeight;
			nextHeight = noiseMap[x + 1];
		}
}

}

[System.Serializable]
public class displayElement {
	public string name = "test";
	public TileBase tile;
	public int minHeight = 0;
	public int maxHeight = 1000;
}

[System.Serializable]
public class lstDisplayElements {
	public string name = "name of the list of display elements";
	public Tilemap tilemap;
	public List<displayElement> lstOfThisElementType;
}

[System.Serializable]
public class TBiome{
	public string name = "biome name";
	public int mapMaxHeight = 10;
	public int noiseScale = 10;
	public List<lstDisplayElements> map;
}

	/*public void DrawSlope(Vector3Int position, int currentHeight, int nextHeight, int x, int y){
		if ((nextHeight - currentHeight) == 1){	// increase
			tilemap.SetTile(position, increaseConnectionTile);
			tilemap.SetTile(new Vector3Int(x, y + 1, 0), increaseTile);
		} else if (((nextHeight - currentHeight) == -1 ) && (y > 0)){	// decrease
			tilemap.SetTile(new Vector3Int(x, y - 1, 0), decreaseConnectionTile);
			tilemap.SetTile(position, decreaseTile);
		}
	}*/