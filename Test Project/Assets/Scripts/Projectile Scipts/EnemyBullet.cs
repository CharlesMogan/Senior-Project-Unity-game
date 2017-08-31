using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {	

	 protected override void OnTriggerEnter(Collider other) {
	 	base.OnTriggerEnter(other);
       	if(other.gameObject.tag == "Player"){
       		Health health = other.GetComponent<Health>();
       		health.TakeDamage(projectileDamage);
       	}
    }
}
