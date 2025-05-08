using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldUnitConvertion : MonoBehaviour
{
    public MapDisplay mapDisplay;
    public int overFlowScreenDistanceInTile = 4;
    int idTerrain;
    float tileSizeInWorld;

    void Start(){
        idTerrain = mapDisplay.GetIdTerrain();
        tileSizeInWorld = GetSizeTileInWorldDimentions();
    }

    public float TileToWorld(int nbTile){
        return tileSizeInWorld * nbTile;
    }

    public float GetLeftTerrainEdge(){
        Tilemap tilemap = mapDisplay.biome.map[idTerrain].tilemap;
		// update bounds
		tilemap.CompressBounds();
		BoundsInt bounds = tilemap.cellBounds;
		Vector3Int cellPosition = new Vector3Int(bounds.xMin, bounds.yMin, 0);
		Vector3 worldPositionLeftTile = tilemap.CellToWorld(cellPosition);
        return worldPositionLeftTile.x;
    }

    public float GetDistanceTerrainToLeftEdgeScreen(){
		Vector3 worldLeftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

		float distance = GetLeftTerrainEdge() - worldLeftEdge.x;

		return distance;

	}

	public float GetSizeTileInWorldDimentions(){
        Tilemap tilemap = mapDisplay.biome.map[idTerrain].tilemap;
        Grid grid = tilemap.layoutGrid;

        Vector3 cellSize = grid.cellSize;

        return cellSize.x;
	}
    
	public int NbTilesToFillScreen(float tileSizeInWorld){
        if (tileSizeInWorld > 0) {
            Camera cam = Camera.main;
            float widthInWorld = 2f * cam.orthographicSize * cam.aspect;

            return Mathf.CeilToInt(widthInWorld / tileSizeInWorld) + overFlowScreenDistanceInTile;
        } else {
            return mapDisplay.mapWidth + overFlowScreenDistanceInTile;
        }
	}
}
