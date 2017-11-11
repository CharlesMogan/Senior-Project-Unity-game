using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserChargeDelayDown : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		Shooting shooting = other.GetComponent<Shooting>();
		shooting.LaserChargeDelayDown();
		Destroy(this.gameObject);
		}
	}
}
