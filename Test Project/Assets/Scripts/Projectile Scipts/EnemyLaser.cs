using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : Laser {

	protected void OnTriggerEnter(Collider other) {
       	if(other.gameObject.tag == "Player"){
       		Health health = other.GetComponent<Health>();
       		health.TakeDamage(projectileDamage);
       	}
    }
}
