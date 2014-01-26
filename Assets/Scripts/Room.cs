using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	List<Tile> _tiles = new List<Tile>();

	Vector2 __entrenceNorth;
	Vector2 __entrenceWest;
	Vector2 __entrenceSouth;
	Vector2 __entrenceEast;

	Room _entrenceRoom;

	public static Room CreateStartRoom(Texture2D __roomDesign)
	{
		GameObject __go = new GameObject("StartRoom");
		Room __room = __go.AddComponent<Room>();

		int __height = __roomDesign.height;
		int __width = __roomDesign.width;

		TileData.TileType[] __tileTypes = CreateTileArray (__roomDesign, __room, ref __height, ref __width, Random.Range (0, 8));
		__room.CreateTiles(__height, __width, __tileTypes);
		__room.CreateEntrences();
		__room._entrenceRoom = __room;

		__room._height = __height;
		__room._width = __width;

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
					Level.startPosition = new Vector2 (__x, __y);
				}

				if (__type== TileData.TileType.Enemy)
				{
					SamuraiScript __samurai = Instantiate(TileData.GetSamurai()) as SamuraiScript;
					__samurai.roomParent = this;
					__samurai.transform.parent = transform;
					__samurai.transform.localPosition = new Vector3 (__x, __y);
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
					__tile.transform.localPosition = new Vector3 (__x, __y);
					_tiles.Add (__tile);
				}
				if (__type == TileData.TileType.Entrance) 
				{
					if (__south != TileData.TileType.Entrance && __west != TileData.TileType.Entrance) 
					{
						if (__x == 0)
						{
							__entrenceWest = new Vector2 (__x, __y);
						}
						else if (__x == __width - 1)
						{
							__entrenceEast = new Vector2 (__x, __y);
						}
						else if (__y == 0)
						{
							__entrenceSouth = new Vector2 (__x, __y);
						}
						else if (__y == __height - 1)
						{
							__entrenceNorth = new Vector2 (__x, __y);
						}
					}
				}
			}
		}
	}

	public enum Direction
	{
		North,
		South,
		East,
		West
	}

	Direction ExitDir;
	Vector2 ExitPos;

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

			__room.transform.position = transform.position + new Vector3(__entrenceNorth.x - 1, __entrenceNorth.y + 1); 
			__room.MoveToPlace();

			Vector2 __midPos = new Vector2(-0.5f + __width / 2.0f, -0.5f + __height / 2.0f);
			BoxCollider2D __trigger = __room.gameObject.AddComponent<BoxCollider2D>();
			__trigger.center = __midPos;
			__trigger.size = new Vector2(3, 1);
			__trigger.isTrigger = true;

			__room.ExitDir = Direction.North;
			__room.ExitPos = new Vector2(1, __height - 1);

			__room.MoveToPlace(PlayerControl.Instance.transform);

			__room._entrenceRoom = this;
			_exitRooms.Add(__room);
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
			
			__room.transform.position = transform.position + new Vector3(__entrenceSouth.x - 1, __entrenceSouth.y - __height); 
			__room.MoveToPlace();

			Vector2 __midPos = new Vector2(-0.5f + __width / 2.0f, -0.5f + __height / 2.0f);
			BoxCollider2D __trigger = __room.gameObject.AddComponent<BoxCollider2D>();
			__trigger.center = __midPos;
			__trigger.size = new Vector2(3, 1);
			__trigger.isTrigger = true;
			
			__room.ExitDir = Direction.South;
			__room.ExitPos = new Vector2(1, 0);

			__room.MoveToPlace(PlayerControl.Instance.transform);

			__room._entrenceRoom = this;
			_exitRooms.Add(__room);
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
			
			__room.transform.position = transform.position + new Vector3(__entrenceWest.x - __width, __entrenceWest.y - 1); 
			__room.MoveToPlace();

			Vector2 __midPos = new Vector2(-0.5f + __width / 2.0f, -0.5f + __height / 2.0f);
			BoxCollider2D __trigger = __room.gameObject.AddComponent<BoxCollider2D>();
			__trigger.center = __midPos;
			__trigger.size = new Vector2(1, 3);
			__trigger.isTrigger = true;
			
			__room.ExitDir = Direction.West;
			__room.ExitPos = new Vector2(0, 1);

			__room.MoveToPlace(PlayerControl.Instance.transform);

			__room._entrenceRoom = this;
			_exitRooms.Add(__room);

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
			
			__room.transform.position = transform.position + new Vector3(__entrenceEast.x + 1, __entrenceEast.y - 1); 
			__room.MoveToPlace();

			Vector2 __midPos = new Vector2(-0.5f + __width / 2.0f, -0.5f + __height / 2.0f);
			BoxCollider2D __trigger = __room.gameObject.AddComponent<BoxCollider2D>();
			__trigger.center = __midPos;
			__trigger.size = new Vector2(1, 3);
			__trigger.isTrigger = true;
			
			__room.ExitDir = Direction.East;
			__room.ExitPos = new Vector2(__width - 1, 1);

			__room.MoveToPlace(PlayerControl.Instance.transform);

			__room._entrenceRoom = this;
			_exitRooms.Add(__room);

		}

		__entrenceNorth = Vector2.zero;
		__entrenceSouth = Vector2.zero;
		__entrenceEast = Vector2.zero;
		__entrenceWest = Vector2.zero;
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

	public Coroutine MoveOut(Transform __playerTransform = null)
	{
		return StartCoroutine(_MoveOut(__playerTransform));
	}

	IEnumerator _MoveOut(Transform __playerTransform = null)
	{
		foreach(Tile __tile in _tiles)
		{
			__tile.MoveOut(__playerTransform);
		}

		yield return null;

		bool __isLooping = true;

		while (true)
		{
			bool __foundLiveTile = false;

			foreach (Tile __tile in _tiles)
			{
				if (__tile != null)
				{
					__foundLiveTile = true;
					break;
				}
			}

			if (!__foundLiveTile)
			{
				break;
			}

			yield return null;
		}

		if (_entrenceRoom != null)
		{
			_entrenceRoom.MoveOut(PlayerControl.Instance.transform);
		}

		Destroy(gameObject);
	}


	void OnTriggerEnter2D(Collider2D __other)
	{
		if (!_hasCreatedExitRoom)
		{
			Room __room = CreateRoom((Vector2)transform.position + ExitPos, ExitDir);
			__room._entrenceRoom = this;

			__room.MoveToPlace(PlayerControl.Instance.transform);

			_hasCreatedExitRoom = true;

			if (_entrenceRoom != null)
			{
				_entrenceRoom.RemoveExits(this);
				_entrenceRoom.MoveOut(PlayerControl.Instance.transform);
			}

			PlayerControl.Instance.PauseOne();
		}
	}

	void RemoveExits(Room __except)
	{
		for (int i=0;i<_exitRooms.Count;i++)
		{
			if (_exitRooms[i] != __except)
			{
				_exitRooms[i].MoveOut();
			}
		}
	}

	List<Room> _exitRooms = new List<Room>();
	bool _hasCreatedExitRoom;
	int _width;
	int _height;

	public static Room CreateRoom(Vector2 __entrencePos, Direction __entrenceDirection)
	{
		Texture2D __roomDesign = TileData.GetRoomDesign();

		GameObject __go = new GameObject("StartRoom");
		Room __room = __go.AddComponent<Room>();
		
		int __height = __roomDesign.height;
		int __width = __roomDesign.width;
		
		TileData.TileType[] __tileTypes = CreateTileArray (__roomDesign, __room, ref __height, ref __width, Random.Range (0, 8));
		__room.CreateTiles(__height, __width, __tileTypes);

		switch(__entrenceDirection)
		{
		case Direction.North:
			__room.transform.position =  __entrencePos - __room.__entrenceSouth + Vector2.up;
			__room.__entrenceSouth = Vector2.zero;
			break;
		case Direction.South:
			__room.transform.position =  __entrencePos - __room.__entrenceNorth - Vector2.up;
			__room.__entrenceNorth = Vector2.zero;
			break;
		case Direction.East:
			__room.transform.position =  __entrencePos - __room.__entrenceWest + Vector2.right;
			__room.__entrenceWest = Vector2.zero;
			break;
		case Direction.West:
			__room.transform.position =  __entrencePos - __room.__entrenceEast - Vector2.right;
			__room.__entrenceEast = Vector2.zero;
			break;
		}

		__room.CreateEntrences();
		__room._height = __height;
		__room._width = __width;

		return __room;
	}

	List<int> GetRoute(int __fromX, int __fromY, int __toX, int __toY)
	{
		int __from = __fromX + __fromY * _width;
		int __to = __toX + __toY * _width;

		return GetRoute(__from, __to);
	}

	public Vector2 WorldPosFromIndex(int __index)
	{
		if (_width == 0)
		{
			return Vector2.zero;
		}

		int __x = __index % _width;
		int __y = __index / _width;

		return new Vector3(__x, __y) + transform.position;
	}

	public int IndexFromWorldPos(Vector3 __pos)
	{
		__pos -= transform.position;
		return Mathf.RoundToInt(__pos.x) + Mathf.RoundToInt(__pos.y) * _width;
	}

	public List<int> GetRoute(int __from, int __to)
	{
		int __toX = __to % _height;
		int __toY = __to / _height;
		
		int __size = _width * _height;
		List<int> closedSet = new List<int>();
		List<int> openSet = new List<int>();

		int[] _cameFrom = new int[__size];
		int[] g_score = new int[__size];
		int[] f_score = new int[__size];
	
		for (int i=0;i<__size;i++)
		{
			_cameFrom[i] = -1;
		}

		openSet.Add(__from);


		while(openSet.Count > 0)
		{
			int __lowest = int.MaxValue;
			int __current = 0;

			foreach(int __index in openSet)
			{
				int __score = g_score[__index] + f_score[__index];
				if (__score < __lowest)
				{
					__lowest = __score;
					__current = __index;
				}
			}

			if (__current == __to)
			{
				return ReconstructPath(_cameFrom, __current); 
			}

			openSet.Remove(__current);
			closedSet.Add(__current);

			foreach (int __neighbour in GetNeighbours(__current))
			{
				if (closedSet.Contains(__neighbour))
				{
					continue;
				}

				//Debug.Log(__neighbour);
				if (!_tiles[__neighbour].walkable)
				{
					continue;
				}

				if (!openSet.Contains(__neighbour) || (g_score[__current] + 1) < g_score[__neighbour])
				{
					_cameFrom[__neighbour] = __current;
					g_score[__neighbour] = g_score[__current] + 1;

					if (!openSet.Contains(__neighbour))
					{
						int __x = __neighbour % _height;
						int __y = __neighbour / _height;
						f_score[__neighbour] = Mathf.Abs(__x - __toX) + Mathf.Abs(__y - __toY);

						openSet.Add(__neighbour);
					}
				}
			}


		}
		return null;
	}

	List<int> ReconstructPath(int[] __cameFromArray, int __goalNode)
	{
		List<int> __route = new List<int>();
		int __index = __goalNode;

		while (__index >= 0)
		{
			__route.Add(__index);
			__index = __cameFromArray[__index];
		}

		__route.Reverse();
		return __route;
	}

						
	List<int> GetNeighbours(int __index)
	{
		int __currentX = __index % _height;
		int __currentY = __index / _height;
	
		List<int> __neighbours = new List<int>();

		if (__currentX - 1 >= 0 && __currentX - 1 < _width)
		{
			__neighbours.Add((__currentX - 1) + __currentY * _width);
		}
		if (__currentX + 1 >= 0 && __currentX + 1 < _width)
		{
			__neighbours.Add((__currentX + 1) + __currentY * _width);
		}
		if (__currentY - 1 >= 0 && __currentY - 1 < _height)
		{
			__neighbours.Add((__currentX) + (__currentY - 1) * _width);
		}
		if (__currentY + 1 >= 0 && __currentY + 1 < _height)
		{
			__neighbours.Add((__currentX) + (__currentY + 1) * _width);
		}
		
		return __neighbours;
	}
}
