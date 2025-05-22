using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorsManager : MonoBehaviour
{
    public GameObject doors;
    public Vector3Int posLeftDoor;
    public Vector3Int posRightDoor;

    public GetteurForStructure getteur;

    private GameObject player;
    private bool firstLoop = true;
    private Tstructures mapDisplayCurrentStructure;
    // Update is called once per frame
    void Update()
    {
        if (firstLoop){ // we don't compute those things in start because getteur is getting those informations in his start
            player = getteur.GetPlayer();
            firstLoop = false;
            mapDisplayCurrentStructure = getteur.GetMapDisplayCurrentStructure();
        }

        // we display or not a tilemap if the player is in the structure
        if(!mapDisplayCurrentStructure.structureIsCompleted){
            if(player.transform.position.x - 1 > (posLeftDoor.x + mapDisplayCurrentStructure.codeSpawnCord) && player.transform.position.x + 1 < (posRightDoor.x + mapDisplayCurrentStructure.codeSpawnCord)){
                        doors.SetActive(true);
                    }
        }else{
            doors.SetActive(false);
        }
        
    }
}
