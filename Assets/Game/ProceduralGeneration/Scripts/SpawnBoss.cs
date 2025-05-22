using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    public GameObject boss;
    public GameObject currentStructure;
    public Vector3 bossPos; // boss pos in the structure it's a local position
    public GetteurForStructure getteur; // use to find the elements we need in game scene (structures are created in an other scene)
    
    private bool haveToBeSpawn = true;
    private bool fisrtLoop = true;
    private MapDisplay script = null;
    private Transform sceneStructureForlder;
    private Transform worldOrigin;
    private Tstructures mapDisplayCurrentStructure;
    // Update is called once per frame
    void Update()
    {
        bool bossDead = false;
        if (fisrtLoop){ // we don't compute those things in start because getteur is getting those informations in his start
            script = getteur.GetMapDisplay();
            sceneStructureForlder = getteur.GetStructureFolder();
            mapDisplayCurrentStructure = getteur.GetMapDisplayCurrentStructure();
            worldOrigin = getteur.GetWorldOrigin();
        }

        // if the boss is spawned we try to see if he is dead
        if(mapDisplayCurrentStructure.bossSpawned){
            GameObject bossInScene = GameObject.Find($"{boss.name}_{mapDisplayCurrentStructure.prefab.name}_{mapDisplayCurrentStructure.name}");
            if(bossInScene == null || bossInScene.transform.position.y < -20){
                    mapDisplayCurrentStructure.structureIsCompleted = true;
                }
        }else { // spawn the boss at the right coordonate that is his local + coordonate of the structure in the world
            GameObject newBoss = Instantiate(boss, new Vector3(bossPos.x + mapDisplayCurrentStructure.codeSpawnCord, bossPos.y + currentStructure.transform.position.y, bossPos.z ), Quaternion.identity);
            mapDisplayCurrentStructure.bossSpawned = true;
            SpriteRenderer sr = newBoss.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 50;
            // to identify the boss of this structure
            newBoss.name = boss.name + "_" + mapDisplayCurrentStructure.prefab.name + "_" + mapDisplayCurrentStructure.name;
        }
        fisrtLoop = false;
        
        

    }
}
