using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemisManager : MonoBehaviour
{
    public MapDisplay mapDisplay;
    public List<TSpawnEnemis> lstEnemis;
    public GameObject worldOrigin;
    public GameObject enemisFolder;
    public WorldUnitConvertion convertionSysteme;

    Dictionary<Vector3, GameObject> enemiAlreadySpawnVectGO = new Dictionary<Vector3, GameObject>();
    Dictionary<GameObject, Vector3> enemiAlreadySpawnGOVect = new Dictionary<GameObject, Vector3>();

    int previousOffset;
    int previousMapWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousMapWidth = mapDisplay.mapWidth;
        previousOffset = mapDisplay.offsetX;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mapDisplay.structureIsSpawning){

            // if there is a terrain mouvement
            if ( mapDisplay.mapWidth != previousMapWidth ){
                Spawn(mapDisplay.mapWidth - previousMapWidth);
                previousMapWidth = mapDisplay.mapWidth;
            } else if (mapDisplay.offsetX != previousOffset){
                Spawn(mapDisplay.offsetX - previousOffset);
                previousOffset = mapDisplay.offsetX;
            }
            
        }
    }

    private void Spawn(int spawnLenthInTiles, bool reset = false) {

            void DisplayInWorld(Vector3 position, TSpawnEnemis enemi){
                // if the enemi is not spawn he spawn
                if (!enemiAlreadySpawnVectGO.ContainsKey(position)) {
                    GameObject newEnemy = Instantiate(enemi.prefab, position, Quaternion.identity, enemisFolder.transform);
                    enemiAlreadySpawnVectGO[position] = newEnemy;
                    enemiAlreadySpawnGOVect[newEnemy] = position;
                }   
            }

            // remove existing enemis
            if (reset){
                foreach (Transform child in enemisFolder.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
            } else{

                foreach (Transform child in enemisFolder.transform)
                    {
                        float terrainLeftEdge = convertionSysteme.GetLeftTerrainEdge();
                        if (( child.position.x < terrainLeftEdge - convertionSysteme.overFlowScreenDistanceInTile / 2) || ( child.position.x > terrainLeftEdge + convertionSysteme.TileToWorld(mapDisplay.mapWidth) + convertionSysteme.overFlowScreenDistanceInTile / 2)){
                            // because when enemi die they destroy there self
                            if (enemiAlreadySpawnGOVect.ContainsKey(child.gameObject)){
                                enemiAlreadySpawnVectGO.Remove(enemiAlreadySpawnGOVect[child.gameObject]);
                                enemiAlreadySpawnGOVect.Remove(child.gameObject);
                                GameObject.Destroy(child.gameObject);
                            }
                            
                        }
                        
                    }

            }
            
            // spawner system
            Vector3 position;
            for (int x = 0; x < Mathf.Abs(spawnLenthInTiles); x++){
                foreach (TSpawnEnemis enemi in lstEnemis){
                    // tow spawn systemes : one if the player is walking forward and one if he is walking backward
                    // x are compute in a way that they don't spawn in the screen
                    int xForwardSpawnPos = mapDisplay.offsetX + mapDisplay.mapWidth - x  - convertionSysteme.overFlowScreenDistanceInTile;
                    int xBackwardSpawnPos = mapDisplay.offsetX + x - 3 * convertionSysteme.overFlowScreenDistanceInTile / 4;
                    if (spawnLenthInTiles >= 0 && enemi.ShouldSpawn(mapDisplay.seed, xForwardSpawnPos)){
                        position = new Vector3(convertionSysteme.TileToWorld(xForwardSpawnPos) , mapDisplay.biome.mapMaxHeight + worldOrigin.transform.localPosition.y, 0);
                        DisplayInWorld(position, enemi);
                    } else if ( enemi.ShouldSpawn(mapDisplay.seed, xBackwardSpawnPos)){
                        position = new Vector3(convertionSysteme.TileToWorld(xBackwardSpawnPos) , mapDisplay.biome.mapMaxHeight + worldOrigin.transform.localPosition.y, 0);
                        DisplayInWorld(position, enemi);
                    }
                }
            }
    }


}

[System.Serializable]
public class TSpawnEnemis {
	public GameObject prefab;
    [Range(0, 1000)] 
	public int proba;

    public bool ShouldSpawn(int seed, int x){
        if (proba != null){
            System.Random prng = new System.Random(seed + x);
            float offsetX = prng.Next(0, 1000);
            if (offsetX < proba){
                return true;
            }
        }
        return false;
    }
}
