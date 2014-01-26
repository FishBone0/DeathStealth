using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SamuraiScript : Damagable {
	private int damage = 10;
	private Animator anim;
	private Collider2D[] array = new Collider2D[10]; 
	private string direction = "up";
	private int moveSpeed = 3;
	private GameObject target = null;
	public Transform gib;

	public Room roomParent;



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

	void CheckRoute()
	{
		int __from = roomParent.IndexFromWorldPos(transform.position);
		int __to = roomParent.IndexFromWorldPos(PlayerControl.Instance.transform.position);

		List<int> __wayPoints = roomParent.GetRoute(__from, __to);
		if (__wayPoints.Count>1)
		{
			Vector2 __goTo = roomParent.WorldPosFromIndex(__wayPoints[1]);
			Debug.DrawLine(__goTo, transform.position);
		}
	}
}
