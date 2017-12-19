using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthUp : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		Health health = other.GetComponent<Health>();
		health.MaxHealthUp();
		Destroy(this.gameObject);
		}
	}
}
