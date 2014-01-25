using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour
{
	[SerializeField]
	GameObject _playerPrefab;

    [SerializeField]
    Room[] _rooms;

    [SerializeField]
    Room[] _hallways;

    List<Room> _activeRooms;

	[SerializeField]
	GameObject _wallPrefab;

	[SerializeField]
	Texture2D[] _roomDesigns;

	// Use this for initialization
	IEnumerator Start ()
    {
		yield return CreateStartRoom();

		//GameObject __player = Instantiate(_playerPrefab) as GameObject;
		//__player.transform.position = Vector3.zero;

        //Room __room = _rooms[Random.Range(0, _rooms.Length)];
		//
		//_activeRooms.Add(__room);
		//AddHallways(__room);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator CreateStartRoom()
	{
		new GameObject("StartRoom");

		int __randomRoomIndex = Random.Range(0, _roomDesigns.Length);
		Texture2D __roomDesign = _roomDesigns[__randomRoomIndex];

		int __height = __roomDesign.height;
		int __width = __roomDesign.width;

		for (int __x=0;__x <__width;__x++)
		{
			for (int __y=0;__y<__height;__y++)
			{
				Color __pixelColor = __roomDesign.GetPixel(__x, __y);

				if (__pixelColor.r == 1 && __pixelColor.g == 1 && __pixelColor.b == 1)
				{
					Tile __tile = Instantiate(_wallPrefab) as Tile;


				}
			}
		}

		yield break;
	}

    void AddHallways(Room __room)
    {
    }
}
