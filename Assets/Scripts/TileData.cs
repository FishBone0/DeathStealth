using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileData : MonoBehaviour
{
	static TileData _instance;
	
	[SerializeField]
	TileColor[] _tilePrefabs;

	[SerializeField]
	Texture2D _startRoom;

	[SerializeField]
	Texture2D[] _roomDesigns;

	[SerializeField]
	Texture2D _hallway;

	[SerializeField]
	AnimationCurve _moveUpCurve;

	Dictionary<Color, Tile> _tileDict = new Dictionary<Color, Tile>();
	
	Dictionary<Color, TileType> _tileType = new Dictionary<Color, TileType>();

	[SerializeField]
	Tile _wallUp;
	[SerializeField]
	Tile _wallCorner;

	[SerializeField]
	Tile _floor;

	[SerializeField]
	Tile _floorWallLeft;
	[SerializeField]
	Tile _floorWallUp;
	[SerializeField]
	Tile _floorWallRight;
	[SerializeField]
	Tile _floorWallDown;
	[SerializeField]
	Tile _floorWall;

	[SerializeField]
	Tile _grassTile;

	[SerializeField]
	Tile _house;

	public enum TileType
	{
		None,
		Floor,
		Vents,
		Wall,
		Enemy,
		Box,
		Entrance,
		Spawn,
		House
	}


	void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;

			_tileType.Add(Color.black, TileType.Wall);
			_tileType.Add(Color.white, TileType.Floor);
			_tileType.Add(Color.red, TileType.Enemy);
			_tileType.Add(new Color32(255, 242, 0, 255), TileType.Entrance);
			_tileType.Add(new Color32(128,64,0,255), TileType.Box);
			_tileType.Add(new Color32(127,127,127,255), TileType.Vents);
			_tileType.Add(new Color32(0,162,232,255), TileType.Spawn);
			_tileType.Add(new Color32(34,177,76,255), TileType.House);

			foreach (TileColor __tileColor in _tilePrefabs)
			{
				_tileDict.Add(__tileColor.color, __tileColor.tile);
			}
		}
	}

	public static Texture2D GetStartRoomDesign()
	{
		if (_instance != null)
		{
			return _instance._startRoom;
		}
		
		return null;
	}

	public static Texture2D GetRoomDesign()
	{
		if (_instance != null)
		{
			int __randomRoomIndex = Random.Range(0, _instance._roomDesigns.Length);
			return _instance._roomDesigns[__randomRoomIndex];
		}

		return null;
	}

	public static Texture2D GetHallway()
	{
		if (_instance != null)
		{
			return _instance._hallway;
		}

		return null;
	}

	public static TileType GetTileType(Color __color)
	{
		if (_instance != null)
		{
			if (_instance._tileType.ContainsKey(__color))
			{
				return _instance._tileType[__color];
			}
		}
		
		return TileType.None;
	}

	public static Tile GetTile(TileType __type, TileType __north, TileType __east, TileType __south, TileType __west, TileType __northNorth, TileType __northWest, TileType __northEast)
	{
		if (_instance != null)
		{
			if (__type == TileType.House)
			{
				if (__south != TileType.House && __west != TileType.House)
				{
					return Instantiate(_instance._house) as Tile;
				}
				else
				{
					return null;
				}
			}
			if (__type == TileType.Wall)
			{
				Tile __tile = null;

				if (__north != TileType.Wall && __north != TileType.Vents && __east != TileType.Wall && __east != TileType.Vents)
				{
					__tile = Instantiate(_instance._wallCorner) as Tile;
				}
				else if (__east != TileType.Wall && __east != TileType.Vents && __south != TileType.Wall && __south != TileType.Vents)
				{
					__tile = Instantiate(_instance._wallCorner) as Tile;
					__tile.RotateTile(90);
				}
				else if (__south != TileType.Wall && __south != TileType.Vents && __west != TileType.Wall && __west != TileType.Vents)
				{
					__tile = Instantiate(_instance._wallCorner) as Tile;
					__tile.RotateTile(180);
				}
				else if (__west != TileType.Wall && __west != TileType.Vents && __north != TileType.Wall && __north != TileType.Vents)
				{
					__tile = Instantiate(_instance._wallCorner) as Tile;
					__tile.RotateTile(270);
				}
				else if (__north != TileType.Wall && __north != TileType.Vents)
				{
					__tile = Instantiate(_instance._wallUp) as Tile;
				}
				else if (__east != TileType.Wall && __east != TileType.Vents)
				{
					__tile = Instantiate(_instance._wallUp) as Tile;
					__tile.RotateTile(90);
				}
				else if (__south != TileType.Wall && __south != TileType.Vents)
				{
					__tile = Instantiate(_instance._wallUp) as Tile;
					__tile.RotateTile(180);
				}
				else if (__west != TileType.Wall && __west != TileType.Vents)
				{
					__tile = Instantiate(_instance._wallUp) as Tile;
					__tile.RotateTile(270);
				}
				else
				{
					__tile = Instantiate(_instance._wallUp) as Tile;
				}

				return __tile;
			}
			if (__type == TileType.Vents)
			{
				//Add grass
				Tile __tile = Instantiate(_instance._grassTile) as Tile;
				if (__north == TileType.Vents)
				{
					if (__south == TileType.Vents)
					{
						__tile.ChangeSprite(9);
					}
					else if (__west == TileType.Vents)
					{
						__tile.ChangeSprite(5);
					}
					else if (__east == TileType.Vents)
					{
						__tile.ChangeSprite(4);
					}
					else
					{
						__tile.ChangeSprite(9);
					}
				}
				else if (__south == TileType.Vents)
				{
					if (__west == TileType.Vents)
					{
						__tile.ChangeSprite(1);
					}
					else if (__east == TileType.Vents)
					{
						__tile.ChangeSprite(0);
					}
					else
					{
						__tile.ChangeSprite(9);
					}
				}
				else if (__east == TileType.Vents)
				{
					if (__west == TileType.Vents)
					{
						__tile.ChangeSprite(10);
					}
					else
					{
						__tile.ChangeSprite(10);
					}
				}
				else
				{
					__tile.ChangeSprite(10);
				}
				
				return __tile;
			}
			if (__type != TileType.Wall && (__north == TileType.Vents || __east == TileType.Vents || __west == TileType.Vents || __south == TileType.Vents))
			{
				if (__north == TileType.Vents)
				{
					//Add grass
					Tile __tile = Instantiate(_instance._grassTile) as Tile;
					__tile.ChangeSprite(8);
					return __tile;
				}
				else
				{
					return Instantiate(_instance._floor) as Tile;
				}
			}

			if (__north == TileType.Wall)
			{
				return Instantiate(_instance._floorWall) as Tile;
			}
			if (__west == TileType.Wall)
			{
				return Instantiate(_instance._floorWallLeft) as Tile;
			}
			if (__east == TileType.Wall)
			{
				return Instantiate(_instance._floorWallRight) as Tile;
			}
			if (__northNorth == TileType.Wall)
			{
				return Instantiate(_instance._floorWallUp) as Tile;
			}
			if (__northEast == TileType.Wall)
			{
				return Instantiate(_instance._floorWallLeft) as Tile;
			}
			if (__northWest == TileType.Wall)
			{
				return Instantiate(_instance._floorWallRight) as Tile;
			}
			if (__south == TileType.Wall)
			{
				return Instantiate(_instance._floorWallDown) as Tile;
			}
			
			//Return default-tile if none mathces
			return Instantiate(_instance._floor) as Tile;
		}
		
		return null;
	}

	public static Tile GetTile(Color __color)
	{
		if (_instance != null)
		{
			if (_instance._tileDict.ContainsKey(__color))
			{
				return _instance._tileDict[__color];
			}

			//Return default-tile if none mathces
			return _instance._tilePrefabs[0].tile;
		}

		return null;
	}

	public static AnimationCurve GetCurve()
	{
		if (_instance !=null)
		{
			return _instance._moveUpCurve;
		}
		return new AnimationCurve();
	}
}

[System.Serializable]
public class TileColor
{
	public Tile tile;
	public Color color;
	
}