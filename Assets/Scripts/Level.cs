using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour
{
	public static Vector2 startPosition;

	[SerializeField]
	GameObject _playerPrefab;

    List<Room> _activeRooms = new List<Room>();

	// Use this for initialization
	IEnumerator Start ()
    {
		Room __startRoom = Room.CreateStartRoom(TileData.GetStartRoomDesign());
		_activeRooms.Add(__startRoom);

		CameraMovement.Instance.transform.position = new Vector3(startPosition.x, startPosition.y, CameraMovement.Instance.transform.position.z);

		GameObject __player = Instantiate(_playerPrefab, startPosition, Quaternion.identity) as GameObject;
		CameraMovement.SetPlayerTransform(__player.transform);

		__player.SetActive(false);

		__startRoom.MoveToPlace(__player.transform);

		yield return new WaitForSeconds(3.0f);
		__player.SetActive(true);
		



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
