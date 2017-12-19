using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour {
	protected virtual void OnTriggerEnter(Collider other) {
	if(other.gameObject.tag == "Player"){
		Movement movement = other.GetComponent<Movement>();
		movement.SpeedUp();
		Destroy(this.gameObject);
		}
	}
}
