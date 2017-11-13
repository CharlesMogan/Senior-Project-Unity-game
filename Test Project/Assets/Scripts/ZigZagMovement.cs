using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagMovement : Movement {
	private Transform player; 
	
	void Start(){
		base.Start();
		player = null;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(player == null){
			player = GameObject.FindWithTag("Player").transform;
		}else{
			if(!isParalyzed && ((int)Time.realtimeSinceStartup) % 2 == 0){
				Debug.Log("going right");
				character.LookAt(player);
				movement = (character.forward-character.right-character.right)*speed*Time.fixedDeltaTime;
				rb.MovePosition(rb.position + movement);
			}else if(!isParalyzed && ((int)Time.realtimeSinceStartup) % 2 == 1){
				Debug.Log("going left");
				character.LookAt(player);
				movement = (character.forward+character.right+character.right)*speed*Time.fixedDeltaTime;
				rb.MovePosition(rb.position + movement);
			}
		}
	}
}
