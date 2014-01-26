using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	static CameraMovement _instance;
	public Transform _playerTransform;

	// Use this for initialization
	void Awake ()
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

	public static void SetPlayerTransform(Transform __player)
	{
		if (_instance != null)
		{
			_instance._playerTransform = __player;
		}
	}

	void FixedUpdate ()
	{
		if (_playerTransform != null)
		{
			transform.position = new Vector3 ( transform.position.x - (transform.position.x - _playerTransform.position.x)/10, transform.position.y - (transform.position.y - _playerTransform.position.y)/10, -10);
		}
	}
}
