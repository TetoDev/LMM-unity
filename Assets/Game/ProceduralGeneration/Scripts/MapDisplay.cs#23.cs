using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.SceneManagement;

public class MapDisplay : MonoBehaviour {

	public GameObject StructureFolder; // where the structure will be instanciate 
	public GameObject layers; // contains all components related to the terrain
    public WorldUnitConvertion convertionSysteme;
	public GameObject worldOrigin; // contain all world game object

	public int mapWidth;

	public int seed;
	public int offsetX;
	
	public bool autoUpdate;

	public TBiome biome;

	public SaveAndLoad saveAndLoad;

	// find the id of the tile in the list List<displayElement> to assotiate it with a string key
	private int idTerrain;
	private int idGrass;
	private int idTree;
	private int idDecors;
	private int structureFlatZoneHeigth = 1;

	private Dictionary<string, int> IdTerrainTiles;

	public bool structureIsSpawning = false;

	public void Start(){
		// find the id of the tile in the list List<displayElement> to assotiate it with a string key
		idTerrain = reacherIndexInBiome("terrain");
		idGrass = reacherIndexInBiome("grass");
		idTree = reacherIndexInBiome("tree");
		idDecors = reacherIndexInBiome("decors");
		IdTerrainTiles = saveAndLoad.BuildDicoIdTerrain(biome.name);

		structureIsSpawning = false;
		structureFlatZoneHeigth = biome.mapMaxHeight /2;

        GenerateMap();
		
	}

	public int GetIdTerrain(){
		return idTerrain;
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

	// generate a noise map ( a list of height ) and color it with tiles
	public void GenerateMap() {
		int[] noiseMap = Noise.GenerateNoiseMap (mapWidth, seed, offsetX, biome, structureFlatZoneHeigth);
		DrawMap(noiseMap);
	}

	// color the map with tile structurs decors grass ...
	private void DrawMap(int[] noiseMap){
		
		// use to compute wath kind of tile we need to place to color the map in the right way
		int previousHeight = noiseMap[0];
		int currentHeight = noiseMap[1];
		int nextHeight = noiseMap[2];
		
		// clear the terrain 
		for (int i = 0 ; i < biome.map.Count ; i ++){
			biome.map[i].tilemap.ClearAllTiles();
		}
		if (Application.isPlaying && StructureFolder != null){
			foreach (Transform structure in StructureFolder.transform)
			{
				GameObject.Destroy(structure.gameObject);
			}
		}

		// methodes use to color the terrain depending of the position (x,y)
		bool ShouldDrawScale(int prev, int curr) {
			return (Math.Abs(prev - curr) >= 2) && idTerrain >= 0;
		} 

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
				// spawn the structure
				if (structure.HaveToBeDisplay(position.x) && Application.isPlaying){
					structureIsSpawning = true;
					Vector3 world = worldOrigin.transform.position;
					structure.codeSpawnCord = convertionSysteme.TileToWorld(position.x ) + world.x - 3 * convertionSysteme.TileToWorld(structure.length) / 2;
					GameObject temp = Instantiate(structure.prefab, new Vector3(structure.codeSpawnCord, structureFlatZoneHeigth - 0.5f + layers.transform.localPosition.y + world.y, 0), Quaternion.identity, StructureFolder.transform);
					temp.name = structure.prefab.name;
				} if (structure.IsSpawning(position.x)){ // avoid decors & grass
					structureIsSpawning = true;
				}
			}
		}

		// main loop who brwose noise map list
		for (int x = 2; x < mapWidth - 1; x++)
		{
			int max = Math.Max(previousHeight, nextHeight);
			int min = Math.Min(previousHeight, nextHeight);
			bool drawScale = ShouldDrawScale(previousHeight, currentHeight);

			Vector3Int position = new Vector3Int(x + offsetX, currentHeight, 0);
			SetDecor(position, x);
			SetBuilding(position, offsetX);
			
			// place the tiles one by one 
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