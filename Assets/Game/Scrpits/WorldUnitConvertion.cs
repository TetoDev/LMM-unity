using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldUnitConvertion : MonoBehaviour
{
    public MapDisplay mapDisplay;
    public int overFlowScreenDistanceInTile = 1;
    int idTerrain;
    float tileSizeInWorld;

    void Start(){
        idTerrain = mapDisplay.GetIdTerrain();
        tileSizeInWorld = GetSizeTileInWorldDimentions();
    }

    // compute how much terrain we have to display out of the screen
    public void ComputeOverFlowScreenDistance(){
        foreach (Tstructures structure in mapDisplay.biome.lstStructures){
            if ( structure.length > overFlowScreenDistanceInTile){
                overFlowScreenDistanceInTile = 2 * structure.length;
            }
        }
    }

    // concert tile unit to world unit
    public float TileToWorld(int nbTile){
        return tileSizeInWorld * nbTile;
    }

    // compute the position of the left edge of the terrain taking into account the overflow distance 
    public float GetLeftTerrainEdge(){
        Tilemap tilemap = mapDisplay.biome.map[idTerrain].tilemap;
		// update bounds
		tilemap.CompressBounds();
		BoundsInt bounds = tilemap.cellBounds;
		Vector3Int cellPosition = new Vector3Int(bounds.xMin, bounds.yMin, 0);
		Vector3 worldPositionLeftTile = tilemap.CellToWorld(cellPosition);
        return worldPositionLeftTile.x + overFlowScreenDistanceInTile / 2;
    }

    // compute the distance betwen the terrain and the left edge screen taking into account the overflow distance 
    public float GetDistanceTerrainToLeftEdgeScreen(){
		Vector3 worldLeftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

		float distance = GetLeftTerrainEdge() - 1 - worldLeftEdge.x;

		return distance;

	}

    // compute the size of a tile in world 
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
