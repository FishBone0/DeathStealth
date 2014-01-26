using UnityEngine;
using System.Collections;

public class SamuraiScript : Damagable {

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		//Debug.Log ("animator get");
	}
	
	// Update is called once per frame
	void Update () {
		if(wasHit == true){
			anim.SetBool("hit", true);
			wasHit = false;
			Invoke ("unHit", 0.3f);
			Debug.Log ("ouch!");
			//unHit();
			//wasHit = false;
		}
	}

	void unHit(){
		anim.SetBool("hit", false);
	}
}
