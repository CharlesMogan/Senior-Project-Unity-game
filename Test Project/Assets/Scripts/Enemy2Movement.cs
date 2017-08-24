using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//this enemy should face twords the player until right before it shoots and then pause to shoot. it should also follow the player relentlessly
//the time delay before shooting should be adjustable.


public class Enemy2Movement : Movement {
	public Transform player; 
	//protected override void Start(){
	//}

	
	// Update is called once per frame
	void FixedUpdate () {
		//float xInputFixed= xInput * Time.fixedDeltaTime*speed;	
		//float yInputFixed = yInput* Time.fixedDeltaTime*speed;
		character.LookAt(player);
		movement = character.forward*speed*Time.fixedDeltaTime;
		rb.MovePosition(rb.position + movement);
		//rb.Forwar(rb.position + movement);
	
		character.LookAt(player);
		//rb.AddRelativeForce(Vector3.forward * speed,ForceMode.Impulse);  // http://answers.unity3d.com/questions/253254/rigidbodyaddforce-forward-locally.html
	}
}
