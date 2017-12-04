using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFireRateUp : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		BulletShooting shooting = other.GetComponent<BulletShooting>();
		shooting.BulletFireRateUp();
		Destroy(this.gameObject);
		}
	}
}
