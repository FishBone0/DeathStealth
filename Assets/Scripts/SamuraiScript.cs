using UnityEngine;
using System.Collections;

public class SamuraiScript : Damagable {

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(wasHit == true){
			anim.SetBool("hit", true);
			Invoke ("unHit", 0.3f);
			wasHit = false;
			//unHit();
			//wasHit = false;
		}
	}

	void unHit(){
		anim.SetBool("hit", false);
	}
}
