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

	void Start()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
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