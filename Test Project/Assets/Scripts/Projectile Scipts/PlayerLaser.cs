using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : Laser {

	protected void OnTriggerEnter(Collider other) {
       	if(other.gameObject.tag == "Enemy"){
       		Health health = other.GetComponent<Health>();
       		health.TakeDamage(projectileDamage);
       	}
    }
}
