using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour
{
	public static Vector2 startPosition;

	[SerializeField]
	GameObject _playerPrefab;

    List<Room> _activeRooms;

	// Use this for initialization
	IEnumerator Start ()
    {
		Room.CreateStartRoom(TileData.GetStartRoomDesign());

		//yield return new WaitForSeconds(3.0f);
		GameObject __player = Instantiate(_playerPrefab, startPosition, Quaternion.identity) as GameObject;
		CameraMovement.SetPlayerTransform(__player.transform);

		yield break;
		//yield return CreateStartRoom();

		//GameObject __player = Instantiate(_playerPrefab) as GameObject;
		//__player.transform.position = Vector3.zero;

        //Room __room = _rooms[Random.Range(0, _rooms.Length)];
		//
		//_activeRooms.Add(__room);
		//AddHallways(__room);
	}


}
