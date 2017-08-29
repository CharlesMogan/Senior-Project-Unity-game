using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//this enemy should face twords the player until right before it shoots and then pause to shoot. it should also follow the player relentlessly
//the time delay before shooting should be adjustable.


public class Enemy2Movement : Movement {
	private Transform player; 
	//protected override void Start(){
	//}


	void Start(){
		base.Start();
		player = null;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//float xInputFixed= xInput * Time.fixedDeltaTime*speed;	
		//float yInputFixed = yInput* Time.fixedDeltaTime*speed;
		if(player == null){
			player = GameObject.FindWithTag("Player").transform;
		}else{
		character.LookAt(player);
		movement = character.forward*speed*Time.fixedDeltaTime;
		rb.MovePosition(rb.position + movement);
		}
		//rb.Forwar(rb.position + movement);
	
		//rb.AddRelativeForce(Vector3.forward * speed,ForceMode.Impulse);  // http://answers.unity3d.com/questions/253254/rigidbodyaddforce-forward-locally.html
	}
}
