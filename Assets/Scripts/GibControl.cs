using UnityEngine;
using System.Collections;

public class GibControl : MonoBehaviour {
	public Sprite[] mySprite;
	// Use this for initialization
	void Start () {
		var xDir = 1;
		var yDir = 1;
		if(Random.value > 0.5){
			xDir = -1;
		}
		if(Random.value > 0.5){
			yDir = -1;
		}
		rigidbody2D.AddForce (new Vector2 (Random.value * 100 * xDir, Random.value * 100 * yDir));
		var rand = Mathf.FloorToInt (Random.value * 5);
		GetComponent<SpriteRenderer>().sprite = mySprite[rand];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
