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

       		Vector3 collisionPoint = new Vector3(collision.contacts[0].point.x, 0, collision.contacts[0].point.z);
       		Vector3 playerPosition = new Vector3(this.transform.position.x, 0, this.transform.position.z);
    		Vector3 enemyDirection = collisionPoint - playerPosition;   //try replacing with this https://docs.unity3d.com/ScriptReference/Collider.ClosestPoint.html
    		Rigidbody rb = collision.rigidbody;

    		Vector3 movement = enemyDirection*2.0f;
			rb.MovePosition(rb.position + movement);
			rb = this.GetComponent<Rigidbody>();
			rb.MovePosition(rb.position - movement);


            Health health = this.GetComponent<Health>();
            health.TakeDamage(10);

    	}
    }
    



}
