using UnityEngine;

public class nextBiome : MonoBehaviour
{
    public GameObject currentStructure;
    Transform player;
    MapDisplay script;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform worldOrigin = GameObject.Find("WorldOrigin").transform;
        player = worldOrigin.Find("player");

        Transform manager = GameObject.Find("Manager").transform;
        GameObject mapDisplay = manager.Find("MapDisplay").gameObject;
        script = mapDisplay.GetComponent<MapDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        // consider the biome as completed if the player pos > spawn cord of the structure
        if (player.position.x >= currentStructure.transform.position.x){
            script.biome.isCompleted = true;
        }
    }
}
