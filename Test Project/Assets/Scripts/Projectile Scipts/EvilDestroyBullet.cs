using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilDestroyBullet : Bullet {

 protected override void OnTriggerEnter(Collider other) {
	 	base.OnTriggerEnter(other);
       	if(other.gameObject.tag == "Player" || other.gameObject.tag == "Wall"){
       		Health health = other.GetComponent<Health>();
       		health.TakeDamage(projectileDamage);
       	}
    }
}
