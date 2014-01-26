using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	List<Tile> _tiles = new List<Tile>();

	public static Room CreateStartRoom(Texture2D __roomDesign)
	{
		GameObject __go = new GameObject("StartRoom");
		Room __room = __go.AddComponent<Room>();

		int __height = __roomDesign.height;
		int __width = __roomDesign.width;

		TileData.TileType[] __tileTypes = new TileData.TileType[__height*__width];

		int __rotation = Random.Range(0,8);

		for (int __x=0;__x <__width;__x++)
		{
			for (int __y=0;__y<__height;__y++)
			{
				__tileTypes[GetIndex(__x, __y, __width, __height, __rotation)] = TileData.GetTileType(__roomDesign.GetPixel(__x, __y));

//				Color __pixelColor = __roomDesign.GetPixel(__x, __y);
//				Tile __tile = Tile.CreateTile(__pixelColor);
//
//				if (__tile != null)
//				{
//					__tile.transform.parent = __go.transform;
//					__tile.transform.localPosition = new Vector3(__x - __width / 2, __y - __height / 2);
//					__room._tiles.Add(__tile);
//				}
			}
		}

		for (int __x=0;__x <__width;__x++)
		{
			for (int __y=0;__y<__height;__y++)
			{
				TileData.TileType __type = __tileTypes[GetIndex(__x, __y, __width, __height)];

				TileData.TileType __north = TileData.TileType.None;
				TileData.TileType __east = TileData.TileType.None;
				TileData.TileType __south = TileData.TileType.None;
				TileData.TileType __west = TileData.TileType.None;
				TileData.TileType __northNorth = TileData.TileType.None;

				if (__y - 1 >= 0)
				{
					__south = __tileTypes[GetIndex(__x, __y - 1, __width, __height)];
				}
				if (__x + 1 < __width)
				{
					__east = __tileTypes[GetIndex(__x + 1, __y, __width, __height)];
				}
				if (__y + 1 < __height)
				{
					__north = __tileTypes[GetIndex(__x, __y + 1, __width, __height)];
				}
				if (__y + 2 < __height)
				{
					__northNorth = __tileTypes[GetIndex(__x, __y + 2, __width, __height)];
				}
				if (__x - 1 >= 0)
				{
					__west = __tileTypes[GetIndex(__x - 1, __y, __width, __height)];
				}

				Tile __tile = TileData.GetTile(__type, __north, __east, __south, __west, __northNorth);

				if (__tile != null)
				{
					__tile.transform.parent = __go.transform;
					__tile.transform.localPosition = new Vector3(__x - __width / 2, __y - __height / 2);
					__room._tiles.Add(__tile);
				}
			}
		}

		return null;

		foreach(Tile __tile in __room._tiles)
		{
			__tile.MoveToPlace();
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
}
