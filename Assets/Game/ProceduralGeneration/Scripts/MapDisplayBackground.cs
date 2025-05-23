using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public class displayElement { // tile with his settings
	public string name = "test";
	public TileBase tile;
	public int minHeight = 0; // higher tile will not be display
	public int maxHeight = 1000;
}

[System.Serializable]
public class lstDisplayElements {
	public string name = "name of the list of display elements";
	public Tilemap tilemap;
	public List<displayElement> lstOfThisElementType;
}

[System.Serializable]
public class TParallaxBackground {
	public string name = "name of the list of display elements";
	public Texture2D texture;
	public float speed;
}

[System.Serializable]
public class TBiome{
	public string name = "biome name";
	public int mapMaxHeight = 10;
	public int noiseScale = 10;
	[Range(0, 100)] 
	public int probaDecor = 10;
	[Range(0, 100)] 
	public int probaTree = 20;
	[Range(0, 100)] 
	public int probaGrass = 60;
	public GameObject spawnEnemisManager;
	public List<Tstructures> lstStructures = new List<Tstructures>();  
	[Header("Warning Max 10 background")]
	public List<TParallaxBackground> lstParallaxBackground = new List<TParallaxBackground>();
	public List<lstDisplayElements> map = new List<lstDisplayElements>();
	[HideInInspector]
	public bool isCompleted = false;
}

[System.Serializable]
public class Tstructures {
	public int spawnCord = 0;
	public int idTile = 0;
	public GameObject prefab;
	public int length = 0;
	public string name = "null";
	[HideInInspector]
	public bool structureIsCompleted = false;
	[HideInInspector]
	public bool bossSpawned = false;
	[HideInInspector]
	public float codeSpawnCord = 0;

	public bool HaveToBeDisplay(int cordWithOffset){
		if (spawnCord == cordWithOffset){
			return true;
		}  else{
			return false;
		}
	}

	public bool IsSpawning(int cordWithOffset){
		if (cordWithOffset - length / 2 < spawnCord && spawnCord < cordWithOffset + length){
			return true;
		}  else{
			return false;
		}
	}
}
