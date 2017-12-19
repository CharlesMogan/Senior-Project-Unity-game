using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifetimeUp : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		BulletShooting shooting = other.GetComponent<BulletShooting>();
		shooting.BulletLifetimeUp();
		Destroy(this.gameObject);
		}
	}
}
