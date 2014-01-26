using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	List<Tile> _tiles = new List<Tile>();

	Vector2 __entrenceNorth;
	Vector2 __entrenceWest;
	Vector2 __entrenceSouth;
	Vector2 __entrenceEast;

	public static Room CreateStartRoom(Texture2D __roomDesign)
	{
		GameObject __go = new GameObject("StartRoom");
		Room __room = __go.AddComponent<Room>();

		int __height = __roomDesign.height;
		int __width = __roomDesign.width;

		TileData.TileType[] __tileTypes = CreateTileArray (__roomDesign, __room, ref __height, ref __width, Random.Range (0, 8));
		__room.CreateTiles(__height, __width, __tileTypes);
		__room.CreateEntrences();

		return __room;
	}

	static TileData.TileType[] CreateTileArray(Texture2D __roomDesign, Room __room, ref int __height, ref int __width, int __rotation = 0)
	{
		TileData.TileType[] __tileTypes = new TileData.TileType[__height * __width];

		for (int __x = 0; __x < __width; __x++)
		{
			for (int __y = 0; __y < __height; __y++)
			{
				__tileTypes[GetIndex(__x, __y, __width, __height, __rotation)] = TileData.GetTileType(__roomDesign.GetPixel(__x, __y));
			}
		}

		if (__rotation >= 4)
		{
			int __tmp = __height;
			__height = __width;
			__width = __tmp;
		}

		return __tileTypes;
	}

	void CreateTiles (int __height, int __width, TileData.TileType[] __tileTypes)
	{
		for (int __x = 0; __x < __width; __x++) {
			for (int __y = 0; __y < __height; __y++) {
				TileData.TileType __type = __tileTypes [GetIndex (__x, __y, __width, __height)];
				if (__type == TileData.TileType.Spawn) {
					Level.startPosition = new Vector2 (__x - __width / 2, __y - __height / 2);
				}
				TileData.TileType __north = TileData.TileType.None;
				TileData.TileType __east = TileData.TileType.None;
				TileData.TileType __south = TileData.TileType.None;
				TileData.TileType __west = TileData.TileType.None;
				TileData.TileType __northNorth = TileData.TileType.None;
				TileData.TileType __northEast = TileData.TileType.None;
				TileData.TileType __northWest = TileData.TileType.None;
				if (__y - 1 >= 0) {
					__south = __tileTypes [GetIndex (__x, __y - 1, __width, __height)];
				}
				if (__x + 1 < __width) {
					__east = __tileTypes [GetIndex (__x + 1, __y, __width, __height)];
				}
				if (__y + 1 < __height) {
					__north = __tileTypes [GetIndex (__x, __y + 1, __width, __height)];
				}
				if (__y + 2 < __height) {
					__northNorth = __tileTypes [GetIndex (__x, __y + 2, __width, __height)];
				}
				if (__x - 1 >= 0) {
					__west = __tileTypes [GetIndex (__x - 1, __y, __width, __height)];
				}
				if (__y + 1 < __height) {
					if (__x - 1 >= 0) {
						__northWest = __tileTypes [GetIndex (__x - 1, __y + 1, __width, __height)];
					}
					if (__x + 1 < __width) {
						__northEast = __tileTypes [GetIndex (__x + 1, __y + 1, __width, __height)];
					}
				}
				Tile __tile = TileData.GetTile (__type, __north, __east, __south, __west, __northNorth, __northEast, __northWest);
				if (__tile != null) {
					__tile.transform.parent = transform;
					__tile.transform.localPosition = new Vector3 (__x - __width / 2, __y - __height / 2);
					_tiles.Add (__tile);
				}
				if (__type == TileData.TileType.Entrance) 
				{
					if (__south != TileData.TileType.Entrance && __west != TileData.TileType.Entrance) 
					{
						if (__x == 0)
						{
							__entrenceWest = new Vector2 (__x - __width / 2, __y - __height / 2);
						}
						else if (__x == __width - 1)
						{
							__entrenceEast = new Vector2 (__x - __width / 2, __y - __height / 2);
						}
						else if (__y == 0)
						{
							__entrenceSouth = new Vector2 (__x - __width / 2, __y - __height / 2);
						}
						else if (__y == __height - 1)
						{
							__entrenceNorth = new Vector2 (__x - __width / 2, __y - __height / 2);
						}
					}
				}
			}
		}
	}

	void CreateEntrences()
	{
		if (__entrenceNorth.x != 0 || __entrenceNorth.y != 0)
		{
			Texture2D __roomDesign = TileData.GetHallway();
			GameObject __go = new GameObject("Hallway");
			Room __room = __go.AddComponent<Room>();
			
			int __height = __roomDesign.height;
			int __width = __roomDesign.width;
			
			TileData.TileType[] __tileTypes = CreateTileArray (__roomDesign, __room, ref __height, ref __width, 6);
			__room.CreateTiles(__height, __width, __tileTypes);

			__room.transform.position = new Vector3(__entrenceNorth.x + 1, __entrenceNorth.y + 2 + __width / 2); 
			__room.MoveToPlace();
		}
		if (__entrenceSouth.x != 0 || __entrenceSouth.y != 0)
		{
			Texture2D __roomDesign = TileData.GetHallway();
			GameObject __go = new GameObject("Hallway");
			Room __room = __go.AddComponent<Room>();
			
			int __height = __roomDesign.height;
			int __width = __roomDesign.width;
			
			TileData.TileType[] __tileTypes = CreateTileArray (__roomDesign, __room, ref __height, ref __width, 6);
			__room.CreateTiles(__height, __width, __tileTypes);
			
			__room.transform.position = new Vector3(__entrenceSouth.x + 1, __entrenceSouth.y - 1 - __width / 2); 
			__room.MoveToPlace();
		}
		if (__entrenceWest.x != 0 || __entrenceWest.y != 0)
		{
			Texture2D __roomDesign = TileData.GetHallway();
			GameObject __go = new GameObject("Hallway");
			Room __room = __go.AddComponent<Room>();
			
			int __height = __roomDesign.height;
			int __width = __roomDesign.width;
			
			TileData.TileType[] __tileTypes = CreateTileArray (__roomDesign, __room, ref __height, ref __width, 0);
			__room.CreateTiles(__height, __width, __tileTypes);
			
			__room.transform.position = new Vector3(__entrenceWest.x - __width / 2, __entrenceWest.y + 1); 
			__room.MoveToPlace();
		}
		if (__entrenceEast.x != 0 || __entrenceEast.y != 0)
		{
			Texture2D __roomDesign = TileData.GetHallway();
			GameObject __go = new GameObject("Hallway");
			Room __room = __go.AddComponent<Room>();
			
			int __height = __roomDesign.height;
			int __width = __roomDesign.width;
			
			TileData.TileType[] __tileTypes = CreateTileArray (__roomDesign, __room, ref __height, ref __width, 0);
			__room.CreateTiles(__height, __width, __tileTypes);
			
			__room.transform.position = new Vector3(__entrenceEast.x + 1 +  __width / 2, __entrenceEast.y + 1); 
			__room.MoveToPlace();
		}
	}

	static int GetIndex(int __x, int __y, int __width, int __height, int __rotation = 0)
	{
		switch(__rotation)
		{
		case 0:
			return __x + __y * __width;
		case 1:
			return (__width - 1 - __x) + __y * __width;
		case 2:
			return __x + (__height - 1 - __y) * __width;
		case 3:
			return (__width - 1 - __x) + (__height - 1 - __y) * __width;

		case 4:
			return __x * __height + __y;
		case 5:
			return (__width - 1 - __x) * __height + __y;
		case 6:
			return __x * __height + (__height - 1 - __y);
		case 7:
		default:
			return (__width - 1 - __x) * __height + (__height - 1 - __y);
		}
	}



	public void MoveToPlace(Transform __playerTransform = null)
	{
		foreach(Tile __tile in _tiles)
		{
			__tile.MoveToPlace(__playerTransform);
		}
	}
}
