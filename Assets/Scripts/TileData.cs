using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileData : MonoBehaviour
{
	static TileData _instance;
	
	[SerializeField]
	TileColor[] _tilePrefabs;

	[SerializeField]
	Texture2D[] _roomDesigns;

	[SerializeField]
	AnimationCurve _moveUpCurve;

	Dictionary<Color, Tile> _tileDict = new Dictionary<Color, Tile>();
	
	Dictionary<Color, TileType> _tileType = new Dictionary<Color, TileType>();

	[SerializeField]
	Tile _wallUp;
	[SerializeField]
	Tile _wallCorner;

	[SerializeField]
	Tile _vent;

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

	void Start()
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
			_tileType.Add(Color.yellow, TileType.Entrance);
			_tileType.Add(new Color(128,64,0,255), TileType.Box);
			_tileType.Add(new Color(127,127,127,255), TileType.Vents);
			_tileType.Add(new Color(0,162,232,255), TileType.Spawn);
			_tileType.Add(new Color(34,177,76,255), TileType.House);

			foreach (TileColor __tileColor in _tilePrefabs)
			{
				_tileDict.Add(__tileColor.color, __tileColor.tile);
			}
		}
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

	public static Tile GetTile(TileType __type, TileType __north, TileType __east, TileType __south, TileType __west, TileType __northNorth)
	{
		if (_instance != null)
		{
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

				return __tile;
			}
			if (__type == TileType.Vents)
			{
				return Instantiate(_instance._vent) as Tile;
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
//			if (__south == TileType.Wall)
//			{
//				return Instantiate(_instance._floorWallDown) as Tile;
//			}
			
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