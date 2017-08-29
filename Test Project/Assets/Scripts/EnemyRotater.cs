using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotater : Movement {

	public int rotationSpeed;
	public bool isRotatingClockwise;
	private Quaternion rotation;
	


	// Update is called once per frame
	void FixedUpdate () {
		character.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.fixedDeltaTime);
		
	}
}
