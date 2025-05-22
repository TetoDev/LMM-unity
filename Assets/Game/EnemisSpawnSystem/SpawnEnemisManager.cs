using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemisManager : MonoBehaviour
{
    public List<TSpawnEnemis> lstEnemis;
    private MapDisplay mapDisplay;
    private GameObject worldOrigin;
    private GameObject enemisFolder;
    private WorldUnitConvertion convertionSysteme;
    
    Dictionary<Vector3, GameObject> enemiAlreadySpawnVectGO = new Dictionary<Vector3, GameObject>();
    Dictionary<GameObject, Vector3> enemiAlreadySpawnGOVect = new Dictionary<GameObject, Vector3>();

    int previousOffset;
    int previousMapWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get componants we need
        Transform manager = GameObject.Find("Manager").transform;
        GameObject mapDisplayGO = manager.Find("MapDisplay").gameObject;
        mapDisplay = mapDisplayGO.GetComponent<MapDisplay>();

        worldOrigin = GameObject.Find("WorldOrigin");
        Transform layers = worldOrigin.transform.Find("Layers");
        enemisFolder = layers.Find("SrcutureFolder").gameObject;

        GameObject convertionSystemeGO = GameObject.Find("ConvertionSysteme").gameObject;
        convertionSysteme = convertionSystemeGO.GetComponent<WorldUnitConvertion>();

        previousMapWidth = mapDisplay.mapWidth;
        previousOffset = mapDisplay.offsetX;
    }

    // Update is called once per frame
    void Update()
    {   
        // get componants we need
        Transform manager = GameObject.Find("Manager").transform;
        GameObject mapDisplayGO = manager.Find("MapDisplay").gameObject;
        mapDisplay = mapDisplayGO.GetComponent<MapDisplay>();

        worldOrigin = GameObject.Find("WorldOrigin");
        enemisFolder = worldOrigin.transform.Find("Enemis").gameObject;

        GameObject convertionSystemeGO = GameObject.Find("ConvertionSysteme").gameObject;
        convertionSysteme = convertionSystemeGO.GetComponent<WorldUnitConvertion>();
        if (!mapDisplay.structureIsSpawning){

            // if there is a terrain mouvement
            if ( mapDisplay.mapWidth != previousMapWidth ){
                Spawn(mapDisplay.mapWidth - previousMapWidth);
                previousMapWidth = mapDisplay.mapWidth;
            } else if (mapDisplay.offsetX != previousOffset){
                Spawn(mapDisplay.offsetX - previousOffset);
                previousOffset = mapDisplay.offsetX;
                Debug.Log("new spawn");
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
                    Debug.Log($"instantiate {newEnemy.name}");
                }   
            }

            // remove existing enemis
            if (reset){
                foreach (Transform child in enemisFolder.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                        Debug.Log($"destroy {child.gameObject.name}");
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
                                Debug.Log($"destroy {child.gameObject.name}");
                                
                            }
                            
                        }
                        
                    }

            }
            
            // spawner system
            Vector3 position;
            for (int x = 0; x < Mathf.Abs(spawnLenthInTiles); x++){
                int id = 0;
                foreach (TSpawnEnemis enemi in lstEnemis){
                    // tow spawn systemes : one if the player is walking forward and one if he is walking backward
                    // x are compute in a way that they don't spawn in the screen
                    int xForwardSpawnPos = mapDisplay.offsetX + mapDisplay.mapWidth - x  - convertionSysteme.overFlowScreenDistanceInTile;
                    int xBackwardSpawnPos = mapDisplay.offsetX + x - 3 * convertionSysteme.overFlowScreenDistanceInTile / 4;
                    if (spawnLenthInTiles >= 0 && enemi.ShouldSpawn(mapDisplay.seed, xForwardSpawnPos, id)){
                        position = new Vector3(convertionSysteme.TileToWorld(xForwardSpawnPos) , mapDisplay.biome.mapMaxHeight + worldOrigin.transform.localPosition.y, 0);
                        DisplayInWorld(position, enemi);
                    } else if ( enemi.ShouldSpawn(mapDisplay.seed, xBackwardSpawnPos, id)){
                        position = new Vector3(convertionSysteme.TileToWorld(xBackwardSpawnPos) , mapDisplay.biome.mapMaxHeight + worldOrigin.transform.localPosition.y, 0);
                        DisplayInWorld(position, enemi);
                    }
                    id += 1;
                }
            }
    }


}

[System.Serializable]
public class TSpawnEnemis {
	public GameObject prefab;
    [Range(0, 1000)] 
	public int proba;

    public bool ShouldSpawn(int seed, int x, int id){
        if (proba != null){
            System.Random prng = new System.Random(seed + x + id);
            float offsetX = prng.Next(0, 1000);
            if (offsetX < proba){
                return true;
            }
        }
        return false;
    }
}
