using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainMouvementManagment : MonoBehaviour
{
    public MapDisplay mapDisplay;

    public GameObject worldOrigin;
    public GameObject target;
	public WorldUnitConvertion convertionSysteme;

    float previousPlayerPos = 0;
	float playerPosCounter = 0;
	float screenDistance = 0;

	int idTerrain;

    int previousScreenWidth = 0;
	int previousScreenHeight = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapDisplay.mapWidth = convertionSysteme.NbTilesToFillScreen(convertionSysteme.GetSizeTileInWorldDimentions());
		
		if (target != null){
			previousPlayerPos = target.transform.localPosition.x;
		}
        
		idTerrain = mapDisplay.GetIdTerrain();
    }


    void Update(){
		bool mapHaveToBeDisplay = false;

		if ((previousScreenWidth != Screen.width ) || (previousScreenHeight != Screen.height)){
            float TileSize = convertionSysteme.GetSizeTileInWorldDimentions();
            // compute the new offset for the new screen
            Vector3 origin = worldOrigin.transform.position;
            worldOrigin.transform.position =  new Vector3(origin.x - convertionSysteme.GetDistanceTerrainToLeftEdgeScreen(), origin.y, origin.z);
            
            // compute the width of the terrain
            mapDisplay.mapWidth = convertionSysteme.NbTilesToFillScreen(TileSize);

            previousScreenWidth = Screen.width;
			previousScreenHeight = Screen.height;
			mapHaveToBeDisplay = true;

			if (target != null){
				previousPlayerPos = target.transform.localPosition.x;
				playerPosCounter = 0f;
			}
		}

		
		if (target != null){
			float playerPos = target.transform.localPosition.x;
			if (previousPlayerPos != playerPos){
			playerPosCounter += playerPos - previousPlayerPos;
			previousPlayerPos = playerPos;
			} if (Mathf.Abs(playerPosCounter) >= 1){
				int temp = Mathf.RoundToInt(playerPosCounter);
				mapDisplay.offsetX += temp;
				mapHaveToBeDisplay = true;
				playerPosCounter -= temp;
			}
		}

		if (mapHaveToBeDisplay){
			mapDisplay.GenerateMap();
		}
		
	}
}
