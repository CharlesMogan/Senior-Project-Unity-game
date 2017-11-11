using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamageUp : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		Shooting shooting = other.GetComponent<Shooting>();
		shooting.LaserDamageUp();
		Destroy(this.gameObject);
		}
	}
}
