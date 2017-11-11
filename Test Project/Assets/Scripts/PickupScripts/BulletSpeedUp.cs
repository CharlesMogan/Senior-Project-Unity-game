using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpeedUp : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		Shooting shooting = other.GetComponent<Shooting>();
		shooting.BulletSpeedUp();
		Destroy(this.gameObject);
		}
	}
}
