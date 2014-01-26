using UnityEngine;
using System.Collections;

public class SamuraiScript : Damagable {
	private int damage = 10;
	private Animator anim;
	private Collider2D[] array = new Collider2D[10]; 
	private string direction = "up";
	private int moveSpeed = 3;
	private GameObject target = null;
	public Transform gib;
	
	void Explode() {
		for (int y = 0; y < 10; y++) {
			Instantiate(gib, new Vector3(transform.position.x + Random.value, transform.position.y +Random.value, 0), Quaternion.identity);
		}
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		//Debug.Log ("animator get");
	}

	void CheckInFront(){

		if (direction == "right") {
			Debug.Log ("Checking hit right");
			Physics2D.OverlapAreaNonAlloc(new Vector2 (transform.position.x, transform.position.y+1), new Vector2 (transform.position.x + 2, transform.position.y - 1), array);
			
		} else if (direction == "left") {
			Debug.Log ("Checking hit left");
			Physics2D.OverlapAreaNonAlloc (new Vector2 (transform.position.x, transform.position.y+1), new Vector2 (transform.position.x - 2, transform.position.y - 1), array);
			
		} else if (direction == "up") {
			Debug.Log ("Checking hit up");
			Physics2D.OverlapAreaNonAlloc (new Vector2 (transform.position.x-1, transform.position.y+1), new Vector2 (transform.position.x+1, transform.position.y + 2), array);
			
		} else if (direction == "down") {
			Debug.Log ("Checking hit down");
			Physics2D.OverlapAreaNonAlloc (new Vector2 (transform.position.x-1, transform.position.y-1), new Vector2 (transform.position.x+1, transform.position.y - 2), array);
			
		}

		foreach(Collider2D temp in array){
			//Debug.Log ("Checking temp in array");
			if(temp != null && temp.gameObject != gameObject){
				//Debug.Log ("Temp not null");
				Debug.Log (temp.gameObject.ToString());
				Damagable dmgble = temp.gameObject.GetComponent<Damagable>();
				//Debug.Log (dmgble.GetType().ToString());
				if(dmgble != null){
					Debug.Log ("Damagable not null");
					dmgble.Damage(damage);
				}
			}
		}

		array = new Collider2D[10];
	}

	void FixedUpdate(){
		Movement ();
	}

	void Movement(){
		if (direction == "right") {
			transform.eulerAngles = new Vector2(0, 180);
		} else if (direction == "left") {
			transform.eulerAngles = new Vector2(0, 0);
		}
		if (Mathf.Abs(rigidbody2D.velocity.x) > 0 || Mathf.Abs(rigidbody2D.velocity.y) > 0 ) {
			anim.SetFloat ("speed", 2);
		} else {
			anim.SetFloat ("speed", 0);
		}
		//Vector2.MoveTowards (transform.position, target.transform.position, moveSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		if(wasHit == true){
			anim.SetBool("hit", true);
			wasHit = false;
			Invoke ("unHit", 0.3f);
			Debug.Log ("ouch!");

			Debug.Log(health.ToString());
			//unHit();
			//wasHit = false;
		}
		if(Input.GetKey(KeyCode.G)){
			CheckInFront();
		}
		if (health <= 0) {
			Explode ();
			Destroy (gameObject);
		}
	}

	void unHit(){
		anim.SetBool("hit", false);
	}
}
