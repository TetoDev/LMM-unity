using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainMouvementManagment : MonoBehaviour
{
    public MapDisplay mapDisplay;

    public GameObject worldOrigin;
    public GameObject target;
	public GameObject layers;
	public WorldUnitConvertion convertionSysteme; // Reference to the script WorldUnitConvertion who have convertion methode betwen tilemap pixel and world

    private float previousPlayerPos = 0;
	private float playerPosCounter = 0;
	private float screenDistance = 0;

	private int idTerrain;

    private int previousScreenWidth = 0;
	private int previousScreenHeight = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		// we set the right width depending of the user screen 
        mapDisplay.mapWidth = convertionSysteme.NbTilesToFillScreen(convertionSysteme.GetSizeTileInWorldDimentions());
		
		// we add an over flow distance how will display terrain out of the screen in order to spawn the enemis and the structure
		convertionSysteme.ComputeOverFlowScreenDistance();
		layers.transform.localPosition = new Vector3(layers.transform.localPosition.x - convertionSysteme.TileToWorld(convertionSysteme.overFlowScreenDistanceInTile / 2), layers.transform.localPosition.y, layers.transform.localPosition.z);
		
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

		// to compute the player mouvement 
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
