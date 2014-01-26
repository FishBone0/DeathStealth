using UnityEngine;
using System.Collections;

public class PlayerControl : Damagable {

	static PlayerControl _instance;

	Collider2D[] array = new Collider2D[10];
	public Camera myCamera;
	public float h = 2;
	public float maxSpeed = 1.2f;
	public Sprite[] mySprite;
	string direction = "right";
	private Animator anim;
	private int blinkLength = 5;
	private float attackCooldown = 0.5f;
	private bool blink = true;
	private bool attacking = false;


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

	public static PlayerControl Instance
	{
		get {return _instance;}
	}

	void Start(){
		anim = GetComponent<Animator> ();
	}
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate () 
    {
		if (!attacking) {
			Movement ();
		} else {
			anim.SetBool("started_atk", false);
		}
    }

	void EndAttack(){
		attacking = false;
		anim.SetBool("attack_right", false);
		anim.SetBool("started_atk", false);
	}

	void CheckInFront(){


		Debug.Log ("Checking hit");
		if (direction == "right") {
			Debug.Log ("Checking hit right");
			Physics2D.OverlapAreaNonAlloc(new Vector2 (transform.position.x, transform.position.y), new Vector2 (transform.position.x + 1, transform.position.y - 1), array);
			//var array = Physics2D.OverlapAreaAll(new Vector2 (transform.position.x, transform.position.y), new Vector2 (transform.position.x + 1, transform.position.y - 1));

		} else if (direction == "left") {
			Debug.Log ("Checking hit left");
			Physics2D.OverlapAreaNonAlloc (new Vector2 (transform.position.x, transform.position.y), new Vector2 (transform.position.x - 1, transform.position.y - 1), array);
			//var array = Physics2D.OverlapAreaAll (new Vector2 (transform.position.x, transform.position.y), new Vector2 (transform.position.x - 1, transform.position.y - 1));

		} else if (direction == "up") {
			Debug.Log ("Checking hit up");
			Physics2D.OverlapAreaNonAlloc (new Vector2 (transform.position.x, transform.position.y), new Vector2 (transform.position.x, transform.position.y + 1), array);
			//var array = Physics2D.OverlapAreaAll (new Vector2 (transform.position.x, transform.position.y), new Vector2 (transform.position.x, transform.position.y + 1));

		} else if (direction == "down") {
			Debug.Log ("Checking hit down");
			Physics2D.OverlapAreaNonAlloc (new Vector2 (transform.position.x, transform.position.y), new Vector2 (transform.position.x, transform.position.y - 1), array);
			//var array = Physics2D.OverlapAreaAll (new Vector2 (transform.position.x, transform.position.y), new Vector2 (transform.position.x, transform.position.y - 1));

		}

		foreach(Collider2D temp in array){
			Debug.Log ("Checking temp in array");
			Damagable dmgble = temp.GetComponent<Damagable>();
			if(dmgble != null){
				Debug.Log ("Damagable not null");
				dmgble.Damage(10);
			}
		}
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
		if (direction == "down") {
			anim.SetBool ("down", true);
		} else {
			anim.SetBool ("down", false);
		}
		if (direction == "up") {
			anim.SetBool ("up", true);
		} else {
			anim.SetBool ("up", false);
		}
		if (moving) {
			anim.SetFloat ("Speed", Mathf.Abs (2));
		} else {
			anim.SetFloat ("Speed", Mathf.Abs (0));
		}
		if (Input.GetKeyDown (KeyCode.Space) && !attacking) {
			attacking = true;
			Invoke("EndAttack", attackCooldown);
			anim.SetBool("attack_right", true);
			anim.SetBool("started_atk", true);
			rigidbody2D.velocity = new Vector2(0, 0);
			Invoke("CheckInFront", 0.2f);
		}

		/*
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
		}*/
	
	}
}
