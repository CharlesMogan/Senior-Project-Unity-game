using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotater : Movement {

	public bool isRotatingClockwise;
		
	void FixedUpdate () {
		//character.RotateAround(Vector3.zero, Vector3.up, speed * Time.fixedDeltaTime);
		if(isRotatingClockwise){
			character.Rotate(Vector3.up * Time.deltaTime * speed, Space.World);
		}else{
			character.Rotate( -1 * Vector3.up * Time.deltaTime * speed, Space.World);	
		}
	}
}
