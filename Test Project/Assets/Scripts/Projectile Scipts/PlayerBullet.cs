using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet {


	protected override void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag != "Player" && other.gameObject.tag != "Friendly Bullet" && other.gameObject.tag != "Unfriendly Bullet"){
    		Destroy(this.gameObject);
    	}
       	if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall"){
       		Health health = other.GetComponent<Health>();
       		health.TakeDamage(projectileDamage);
       	}
    }
}
