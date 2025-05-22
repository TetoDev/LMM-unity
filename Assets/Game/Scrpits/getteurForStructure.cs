using UnityEngine;

public class GetteurForStructure : MonoBehaviour
{
    public GameObject currentStructure;

    private MapDisplay mapDisplayScripts; // Reference to the script map display 
    private Transform sceneStructureForlder; // transform of the folder used to store structures
    private GameObject player;
    private Tstructures structureRefInMapDisplay = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { // we want to compute those things only one time 
        // find the script
        Transform manager = GameObject.Find("Manager").transform;
        GameObject mapDisplay = manager.Find("MapDisplay").gameObject;
        mapDisplayScripts = mapDisplay.GetComponent<MapDisplay>();

        // find structure folder
        Transform worldOrigin = GameObject.Find("WorldOrigin").transform;
        Transform layers = worldOrigin.Find("Layers");
        sceneStructureForlder = layers.Find("SrcutureFolder");

        // find the player
        player = worldOrigin.Find("player").gameObject;

    }

    public MapDisplay GetMapDisplay(){
        Transform manager = GameObject.Find("Manager").transform;
        GameObject mapDisplay = manager.Find("MapDisplay").gameObject;
        mapDisplayScripts = mapDisplay.GetComponent<MapDisplay>();
        return mapDisplayScripts;
    }

    public Transform GetStructureFolder(){
        Transform worldOrigin = GameObject.Find("WorldOrigin").transform;
        Transform layers = worldOrigin.Find("Layers");
        sceneStructureForlder = layers.Find("SrcutureFolder");
        return sceneStructureForlder;
    }

    public Transform GetWorldOrigin(){
        return GameObject.Find("WorldOrigin").transform;
    }

    public GameObject GetPlayer(){
        Transform worldOrigin = GameObject.Find("WorldOrigin").transform;
        Transform layers = worldOrigin.Find("Layers");
        player = worldOrigin.Find("player").gameObject;
        return player;
    }

    public Tstructures GetMapDisplayCurrentStructure(){
        foreach (Tstructures structure in mapDisplayScripts.biome.lstStructures){
            if (currentStructure.name == structure.prefab.name){
                structureRefInMapDisplay = structure;
            }
        }
        return structureRefInMapDisplay;
    }

}
