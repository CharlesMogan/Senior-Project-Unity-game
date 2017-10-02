using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBumper : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider other) {
       	if(other.gameObject.tag == "Enemy"){
    		//Destroy(this.gameObject);

    	}
    }

	void OnCollisionEnter(Collision collision) {
       	if(collision.gameObject.tag == "Enemy"){
    		//Destroy(this.gameObject);

    		Vector3 enemyDirection = collision.contacts[0].point - this.transform.position;
    		rb = collision.gameObject.rigedBody;

    		movement = enemyDirection*100*Time.fixedDeltaTime;
			rb.MovePosition(rb.position + movement);
    	}
    }
    



}
