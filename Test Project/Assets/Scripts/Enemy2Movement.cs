using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//this enemy should face twords the player until right before it shoots and then pause to shoot. it should also follow the player relentlessly
//the time delay before shooting should be adjustable.


public class Enemy2Movement : Movement {
	private Transform player; 


	void Start(){
		base.Start();
		player = null;

	}
	

	void FixedUpdate () {		
		if(player == null){
			player = GameObject.FindWithTag("Player").transform;
		}else{
				character.LookAt(player);
				movement = character.forward*speed*Time.fixedDeltaTime;
				rb.MovePosition(rb.position + movement);
		}
	}
}
