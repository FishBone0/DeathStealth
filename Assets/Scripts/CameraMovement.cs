using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate (){
		var player = GameObject.FindGameObjectWithTag ("Player");
		transform.position = new Vector3 ( transform.position.x - (transform.position.x - player.transform.position.x)/10, transform.position.y - (transform.position.y - player.transform.position.y)/10, -10);
	}
}
