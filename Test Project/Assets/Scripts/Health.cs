using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public float maxHealth, health;
	
	// Update is called once per frame
	protected virtual void Update () {
		if(health <= 0){
			Debug.Log("this is get destroyed");
			Destroy(this.gameObject);
		}

		//if(maxHealth < health){
		//	health = maxHealth;
		//}
		
	}


	public void takeDamage(float damage){
		health-=damage;
	}

	/*public float Health{
		get{return health;}

		set{health = value;}

	}*/	
}
