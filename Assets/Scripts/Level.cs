using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour
{
	public static Vector2 startPosition;

	static Level _instance;

	[SerializeField]
	GameObject _playerPrefab;

	void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	// Use this for initialization
	IEnumerator Start ()
    {
		GameObject __player = Instantiate(_playerPrefab, startPosition, Quaternion.identity) as GameObject;
		__player.SetActive(false);

		Room __startRoom = Room.CreateStartRoom(TileData.GetStartRoomDesign());

		__player.transform.position = startPosition;
		CameraMovement.Instance.transform.position = new Vector3(startPosition.x, startPosition.y, CameraMovement.Instance.transform.position.z);
		CameraMovement.SetPlayerTransform(__player.transform);

		__startRoom.MoveToPlace(__player.transform);
		yield return new WaitForSeconds(3.0f);
		__player.SetActive(true);
	}


}
