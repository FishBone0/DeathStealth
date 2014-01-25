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
		
		for (int __x=0;__x <__width;__x++)
		{
			for (int __y=0;__y<__height;__y++)
			{
				Color __pixelColor = __roomDesign.GetPixel(__x, __y);
				
				Tile __tile = Instantiate(TileData.GetTile(__pixelColor)) as Tile;

				Debug.Log(__pixelColor);
				__tile.transform.parent = __go.transform;
				__tile.transform.localPosition = new Vector3(__x - __width / 2, __y - __height / 2);
				__room._tiles.Add(__tile);
			}
		}

		return null;

		foreach(Tile __tile in __room._tiles)
		{
			__tile.MoveToPlace();
		}
	}
}
