using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		Health health = other.GetComponent<Health>();
		health.HealthUp();
		Destroy(this.gameObject);
		}
	}
}
