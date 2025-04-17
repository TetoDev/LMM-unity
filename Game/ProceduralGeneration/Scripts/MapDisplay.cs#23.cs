using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.SceneManagement;

public class MapDisplay : MonoBehaviour {
	
	public enum Mode {GameMode, EditorMode}
	[Header("Warning make sure that you are in GameMode before click on play")]
	public Mode mode;
	public int mapWidth;

	public int seed;
	public int offsetX;

	public bool autoUpdate;

	public TBiome biome;

	public SaveAndLoad saveAndLoad;

	// find the id of the tile in the list List<displayElement> to assotiate it with a string key
	int idTerrain;
	int idStructures;
	int idGrass;
	int idTree;
	int idDecors;
	Dictionary<string, int> IdTerrainTiles;

	bool structureIsSpawning = false;

	void Start(){
		// find the id of the tile in the list List<displayElement> to assotiate it with a string key
		idTerrain = reacherIndexInBiome("terrain");
		idStructures = reacherIndexInBiome("structures");
		idGrass = reacherIndexInBiome("grass");
		idTree = reacherIndexInBiome("tree");
		idDecors = reacherIndexInBiome("decors");
		IdTerrainTiles = saveAndLoad.BuildDicoIdTerrain(biome.name);

		structureIsSpawning = false;
	}

	public void EditorMode(){
		Start();
	}

	public void GenerateMap() {
		if (mode == Mode.EditorMode){
			EditorMode();
		}
		int[] noiseMap = Noise.GenerateNoiseMap (mapWidth, seed, biome.noiseScale, biome.mapMaxHeight, offsetX, biome);
		DrawMap(noiseMap);
	}

	private int reacherIndexInBiome(String name){
			int id = -1;
			for (int i = 0 ; i < biome.map.Count ; i++){
				if (biome.map[i].name == name){
					id = i;
					break;
				}
			}
			return id;
		}

		private int reacherIndexInLstOfThisElementType(List<displayElement> lst, String name){
			int id = -1;
			for (int i = 0 ; i < lst.Count ; i++){
				if (lst[i].name == name){
					id = i;
					break;
				}
			}
			return id;
		}

	private void DrawMap(int[] noiseMap){
		
		int previousHeight = noiseMap[0];
		int currentHeight = noiseMap[1];
		int nextHeight = noiseMap[2];
		
		for (int i = 0 ; i < biome.map.Count ; i ++){
			biome.map[i].tilemap.ClearAllTiles();
		}

		bool ShouldDrawScale(int prev, int curr) {
			return (Math.Abs(prev - curr) >= 2) && idTerrain >= 0;
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

		void SetTerrain(int x, int y, bool drawScale, int min, int max){
			Vector3Int position = new Vector3Int(x + offsetX, y, 0);
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

		void SetDecor(Vector3Int position, int x){
			if (! structureIsSpawning){
				void PlaceDecor(Vector3Int position, int idElement, int x, System.Random prng){
					int id = prng.Next (0,  biome.map[idElement].lstOfThisElementType.Count);
					SetTile(position, idElement, id);
				}
					
				System.Random prng = new System.Random (seed + offsetX + x);
				int random = prng.Next (0, 100);
				int total = biome.probaDecor + biome.probaTree + biome.probaGrass;
				
				if (random <= ((float)biome.probaDecor / total) * 100f && idDecors >= 0){
					PlaceDecor(new Vector3Int(x + offsetX, currentHeight, 0), idDecors, x, prng);

				} else if (random <= ((float)(biome.probaDecor + biome.probaTree) / total) * 100f && idTree >= 0) {
					PlaceDecor(new Vector3Int(x + offsetX, currentHeight + 1, 0), idTree, x, prng);

				} else if (random <= ((float)(biome.probaDecor + biome.probaTree + biome.probaGrass) / total) * 100f && idGrass >= 0){
					PlaceDecor(new Vector3Int(x + offsetX, currentHeight, 0), idGrass, x, prng);
				}
			}
			
  	  	}

		void SetBuilding(Vector3Int position, int offsetX){
			structureIsSpawning = false;
			foreach (Tstructures structure in biome.lstStructures){
				if (structure.HaveToBeDisplay(position.x)){
					SetTile(position , idStructures, structure.idTile);
				} if (structure.IsSpawning(position.x)){
					structureIsSpawning = true;
				}
			}
		}

		for (int x = 2; x < mapWidth - 1; x++)
		{
			int max = Math.Max(previousHeight, nextHeight);
			int min = Math.Min(previousHeight, nextHeight);
			bool drawScale = ShouldDrawScale(previousHeight, currentHeight);
			// random numbre for evry x and evry seed
			// place les décors :
			Vector3Int position = new Vector3Int(x + offsetX, currentHeight, 0);
			SetDecor(position, x);
			SetBuilding(position, offsetX);
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

	/*public void DrawSlope(Vector3Int position, int currentHeight, int nextHeight, int x, int y){
		if ((nextHeight - currentHeight) == 1){	// increase
			tilemap.SetTile(position, increaseConnectionTile);
			tilemap.SetTile(new Vector3Int(x, y + 1, 0), increaseTile);
		} else if (((nextHeight - currentHeight) == -1 ) && (y > 0)){	// decrease
			tilemap.SetTile(new Vector3Int(x, y - 1, 0), decreaseConnectionTile);
			tilemap.SetTile(position, decreaseTile);
		}
	}*/