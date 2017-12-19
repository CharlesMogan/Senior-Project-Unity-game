using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBumper : MonoBehaviour {
	Rigidbody rb;
	void Start(){
		rb = this.GetComponent<Rigidbody>();
	}


	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "Enemy"){

			Vector3 collisionPoint = new Vector3(collision.contacts[0].point.x, 0, collision.contacts[0].point.z);
			Vector3 playerPosition = new Vector3(this.transform.position.x, 0, this.transform.position.z);
			Vector3 enemyDirection = collisionPoint - playerPosition;   //try replacing with this https://docs.unity3d.com/ScriptReference/Collider.ClosestPoint.html
			Rigidbody otherRB = collision.rigidbody;
			Vector3 movement = enemyDirection*2.0f;
			//otherRB.MovePosition(otherRB.position + movement);
			rb.MovePosition(rb.position - movement);


			Health health = this.GetComponent<Health>();
			health.TakeDamage(10);

		}
		if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Door" || collision.gameObject.tag == "OuterWall"){
			Vector3 collisionPoint = new Vector3(collision.contacts[0].point.x, 0, collision.contacts[0].point.z);
			Vector3 playerPosition = new Vector3(this.transform.position.x, 0, this.transform.position.z);
			Vector3 enemyDirection = collisionPoint - playerPosition;
			Vector3 movement = enemyDirection*1.0f;
			rb.MovePosition(rb.position - movement);
		}
	}




}
