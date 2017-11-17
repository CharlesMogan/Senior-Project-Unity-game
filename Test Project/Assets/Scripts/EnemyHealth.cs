using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health {


	protected override void Die(){
		myRoom.EnemyDied(this.gameObject.transform.position);
		GameObject gameManager = GameObject.FindWithTag("GameController");
		GameManager gameManagerScript = gameManager.GetComponent<GameManager>();

		if(gameManagerScript.NextRandom(1,11) > 1){
			Instantiate(Resources.Load("HealthUpPickup") as GameObject, this.gameObject.transform.position, Quaternion.identity);
		}
		Destroy(this.gameObject);
	}

}
