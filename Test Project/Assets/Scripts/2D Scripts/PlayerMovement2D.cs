using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : Movement2D {


	// Use this for initialization
	private float xInput, zInput, xInputFixed, zInputFixed;
	
	private float nextFire;
	public Camera playerCamera;
	public Transform crosshair;
	private Vector2 mousePosition;

	

	private GameObject playerObject;
	void Update () {
		xInput = Input.GetAxis("Horizontal");
		zInput = Input.GetAxis("Vertical");
			
		//Mouse position is designed forr 2d you have to do this shenanages for it to work
		mousePosition = Input.mousePosition;
		character.LookAt(mousePosition);
		crosshair.position = mousePosition;
		//Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

		//from fixed update
		

	}
	// Update is called once per frame
	void FixedUpdate () {
		
		xInputFixed= xInput * Time.fixedDeltaTime*speed;	
		zInputFixed = zInput* Time.fixedDeltaTime*speed;
		movement = new Vector2(xInputFixed,zInputFixed);
		rb.MovePosition(rb.position + movement);

	}
}
