using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    public GameObject boss;
    public GameObject currentStructure;
    public Vector3 bossPos;
    bool haveToBeSpawn = true;
    bool fisrtLoop = true;
    MapDisplay script;
    Transform sceneStructureForlder;

    void Start(){
        Transform manager = GameObject.Find("Manager").transform;
        GameObject mapDisplay = manager.Find("MapDisplay").gameObject;
        script = mapDisplay.GetComponent<MapDisplay>();

        // find structure folder
        Transform worldOrigin = GameObject.Find("WorldOrigin").transform;
        Transform layers = worldOrigin.Find("Layers");
        sceneStructureForlder = layers.Find("SrcutureFolder");
    }

    private Tstructures GetMapDisplayCurrentStructure(){
        // get the right structure
        foreach (Tstructures structure in script.biome.lstStructures){
            if (sceneStructureForlder.Find(structure.name) != null){
                return structure;
            }
        }

        return null;
    }
    // Update is called once per frame
    void Update()
    {
        bool bossDead = false;

        // We don't need to compute all this stuff evry frame
        Tstructures mapDisplayCurrentStructure = GetMapDisplayCurrentStructure();

        if(mapDisplayCurrentStructure.bossSpawned){
            GameObject bossInScene = GameObject.Find($"{boss.name}{mapDisplayCurrentStructure.name}");
            if(bossInScene == null){
                    bossDead = true;
                }
        }

        if (!mapDisplayCurrentStructure.bossSpawned){
            bossPos.x += currentStructure.transform.position.x;
            bossPos.y += currentStructure.transform.position.y;
            GameObject newBoss = Instantiate(boss, bossPos, Quaternion.identity);
            mapDisplayCurrentStructure.bossSpawned = true;
            newBoss.name = boss.name + mapDisplayCurrentStructure.name;
            Debug.Log($"boss : {boss.name}, {mapDisplayCurrentStructure.name}");
        }

        if(bossDead && mapDisplayCurrentStructure.bossSpawned && ! fisrtLoop){
            mapDisplayCurrentStructure.structureIsCompleted = true;
        }
        fisrtLoop = false;
        
        

    }
}
