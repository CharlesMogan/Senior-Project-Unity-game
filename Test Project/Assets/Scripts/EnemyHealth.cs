using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health {
	protected override void Start(){
		base.Start();
		gameManager.LivingEnemies++;
	}

	protected override void Die(){
		gameManager.EnemyDied(this.gameObject.transform.position);

		if(gameManager.NextRandom(1,11) > 5){
			Instantiate(Resources.Load("HealthUpPickup") as GameObject, this.gameObject.transform.position, Quaternion.identity);
		}
		Destroy(this.gameObject);
	}

}
