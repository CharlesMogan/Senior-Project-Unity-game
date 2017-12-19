using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamageUp : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		LaserShooting shooting = other.GetComponent<LaserShooting>();
		shooting.LaserDamageUp();
		Destroy(this.gameObject);
		}
	}
}
