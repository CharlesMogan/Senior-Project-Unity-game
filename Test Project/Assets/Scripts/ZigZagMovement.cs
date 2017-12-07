using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagMovement : Movement {
	private Transform player; 
	
	void Start(){
		base.Start();
		player = null;
	}
	
	void FixedUpdate () {
		if(player == null){
			player = GameObject.FindWithTag("Player").transform;
		}else{
			if(((int)Time.realtimeSinceStartup) % 2 == 0){
				Debug.Log("going right");
				character.LookAt(player);
				movement = (character.forward-character.right-character.right)*speed*Time.fixedDeltaTime;
				rb.MovePosition(rb.position + movement);
			}else if(((int)Time.realtimeSinceStartup) % 2 == 1){
				Debug.Log("going left");
				character.LookAt(player);
				movement = (character.forward+character.right+character.right)*speed*Time.fixedDeltaTime;
				rb.MovePosition(rb.position + movement);
			}
		}
	}
}
