using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {	

	 protected override void OnTriggerEnter(Collider other) {
	 	base.OnTriggerEnter(other);
	 	Debug.Log("player got kinda shot");
       	if(other.gameObject.tag == "Player"){
       		Debug.Log("player got shot");
       		Health health = other.GetComponent<Health>();
       		health.takeDamage(bulletDamage);

       	}

    }

}
