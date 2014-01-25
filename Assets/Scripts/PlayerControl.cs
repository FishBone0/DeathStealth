using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public Camera myCamera;
	public float h = 2;
	public float maxSpeed = 1.2f;
	public Sprite[] mySprite;
	string direction = "right";
	private Animator anim;
	private int blinkLength = 5;
	private bool blink = true;
	void Start(){
		anim = GetComponent<Animator> ();
	}
	// Update is called once per frame
	void Update () {
		//Movement();
	}

	void FixedUpdate () 
    {
    	Movement();
		MoveCamera();
    }

	void MoveCamera(){
		int amount = 2;
		//myCamera.transform.position = transform.position;
		/*
		int xNudge = 0;
		if (xDirection == "right") {
			xNudge = amount;
		} else if (xDirection == "left") {
			xNudge = -amount;
		} else {
			xNudge = 0;
		}
		int yNudge = 0;
		if (yDirection == "up") {
			yNudge = amount;
		} else if (yDirection == "down") {
			yNudge = -amount;
		} else {

			yNudge = 0;
		}
		myCamera.transform.position = new Vector2(this.transform.position.x + xNudge, this.transform.position.y + yNudge);
*/
	}

	void Movement () {
		bool moving = false; 
		if(Input.GetKey( KeyCode.D )){
			
			rigidbody2D.velocity = new Vector2(h, rigidbody2D.velocity.y);
						
			transform.eulerAngles = new Vector2(0, 0);
			GetComponent<SpriteRenderer>().sprite = mySprite[2];
			direction = "right";
			moving = true;

		} else  if(Input.GetKey( KeyCode.A )){
			
			rigidbody2D.velocity = new Vector2(-h, rigidbody2D.velocity.y);
			transform.eulerAngles = new Vector2(0, 180);
			GetComponent<SpriteRenderer>().sprite = mySprite[2];
			direction = "left";
			moving = true;
		} else {

			rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
		}

		if(Input.GetKey( KeyCode.S )){
			moving = true;
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -h);
			GetComponent<SpriteRenderer>().sprite = mySprite[0];
			direction = "down";

		} else if(Input.GetKey( KeyCode.W )){
			moving = true;
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, h);
			GetComponent<SpriteRenderer>().sprite = mySprite[1];
			direction = "up";
		} else {

			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
		}

		if (moving) {
			anim.SetFloat ("Speed", Mathf.Abs (2));
		} else {
			anim.SetFloat ("Speed", Mathf.Abs (0));
		}

		if (Input.GetKeyDown (KeyCode.E) && blink) { 
			if(direction == "up"){
				transform.position = new Vector2(transform.position.x, transform.position.y + blinkLength);
			} else if(direction == "down"){
				transform.position = new Vector2(transform.position.x, transform.position.y - blinkLength);
			} else if(direction == "right"){
				transform.position = new Vector2(transform.position.x + blinkLength, transform.position.y);
			} else if(direction == "left"){
				transform.position = new Vector2(transform.position.x - blinkLength, transform.position.y);
			}
		}
	
	}
}
