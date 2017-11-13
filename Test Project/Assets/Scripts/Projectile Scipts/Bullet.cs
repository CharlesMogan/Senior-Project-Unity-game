using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile {


	protected virtual void OnTriggerEnter(Collider other) {
       	if(other.gameObject.tag != "Friendly Bullet" && other.gameObject.tag != "Unfriendly Bullet"){
    		Destroy(this.gameObject);
    	}
    }
}
