using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour {
	protected bool wasHit = false;
	protected int health = 20;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Damage(int damage){
		wasHit = true;
		health -= damage;
		Debug.Log ("Wat!");
	}
}
