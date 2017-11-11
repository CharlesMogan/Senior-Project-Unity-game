using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFireRateUp : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		Shooting shooting = other.GetComponent<Shooting>();
		shooting.BulletFireRateUp();
		Destroy(this.gameObject);
		}
	}
}
