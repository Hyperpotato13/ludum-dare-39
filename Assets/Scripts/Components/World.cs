using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {
	public int width {get; protected set;}
	public int height {get; protected set;}
	WorldTile[,] tiles;

	public World(int width, int height)
	{
		this.width = width;
		this.height = height;
		tiles = new WorldTile[width,height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tiles[i,j] = new WorldTile(this,i,j,GenericTileType.None);
				WorldHandler.handler.registerTile(tiles[i,j]);
			}
		}

		generate();
	}

	void generate()
	{
		for(int i=0;i<width;i++)
		{
			for (int j = 0; j < height; j++) {
				tiles[i,j].Type = (Random.Range(0,4) == 1 ? GenericTileType.Solid : GenericTileType.None);
			}
		}

		int x = Random.Range(4, width-5);
		int y = Random.Range(4, height-5);
		tiles[x,y].Type = GenericTileType.CentralComputer;
	}

//	public void updateOxygenLevels()
//	{
//		for (int x = 0; x < width; x++) {
//			for (int y = 0; y < height; y++) {
//				WorldTile tile = tiles[x,y];
//				if(!(tile.Type == GenericTileType.None || tile.Type == GenericTileType.Floor)) continue;
//					
//				foreach(WorldTile neighbor in tile.getNeighbors(true, false))
//				{
//					if(neighbor == null) continue;
//					if(neighbor.Type == GenericTileType.None || neighbor.Type == GenericTileType.Floor)
//					if(neighbor.oxygen<tile.oxygen)
//					{
//						float diff = Mathf.Abs(tile.oxygen-neighbor.oxygen)/300f;
//						neighbor.insertOxygen(diff);
//						tile.depeteOxygen(diff);
//					}
//				}
//
//				foreach(WorldTile neighbor in tile.getNeighbors(false, true))
//				{
//					if(neighbor == null) continue;
//					if(neighbor.Type == GenericTileType.None)
//					if(neighbor.oxygen<tile.oxygen)
//					{
//						float diff = Mathf.Abs(tile.oxygen-neighbor.oxygen)/9000f;
//						neighbor.insertOxygen(diff);
//						tile.depeteOxygen(diff);
//					}
//				}
//			}
//		}
//	}

	public WorldTile getTileAt(int x, int y)
	{
		if(x<0 || y<0 || x>=width || y>=height) {
			Debug.LogError("Attempted to retrieve tile outside bounds.");
			return null;
		}

		return tiles[x,y];
	}
		
	public void claimArea(WorldTile tile)
	{
		if(tile.Type == GenericTileType.None)
		{
			tile.Type = GenericTileType.Floor;
			foreach(WorldTile n in tile.getNeighbors())
				claimArea(n);
		}
	}
}
