using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : Movement {

	// Use this for initialization
	private float xInput, yInput, xInputFixed, yInputFixed;
	
	private float nextFire;
	public Camera playerCamera;
	public Transform crosshair;
	private Vector3 mousePosition3D;

	

	private GameObject playerObject;
void Update () {
		xInput = Input.GetAxis("Horizontal");
		yInput = Input.GetAxis("Vertical");
			
		//Mouse position is designed forr 2d you have to do this shenanages for it to work
		mousePosition3D = new Vector3(playerCamera.ScreenToWorldPoint(Input.mousePosition).x,character.position.y,playerCamera.ScreenToWorldPoint(Input.mousePosition).z);
		character.LookAt(mousePosition3D);
		crosshair.position = mousePosition3D;
		//Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

		//from fixed update
		

	}







	// Update is called once per frame
	void FixedUpdate () {
		
		xInputFixed= xInput * Time.fixedDeltaTime*speed;	
		yInputFixed = yInput* Time.fixedDeltaTime*speed;
		movement = new Vector3(xInputFixed,0.0f,yInputFixed);
		rb.MovePosition(rb.position + movement);

	}

	/*

	public float movementForce = 1700f;
	public bool moveRight = false,moveLeft = false ,moveForward = false,moveBackwards = false;
	



void Update () {
		if( Input.GetKey("w") ){
			moveForward = true;
		}else{
			moveForward = false;

		}


		if( Input.GetKey("s") ){
			moveBackwards = true;
		}else{
			moveBackwards = false;
		}


		if( Input.GetKey("a") ){
			moveLeft = true;
		}else{
			moveLeft = false;
		}

		if( Input.GetKey("d") ){
			moveRight = true;
		}else{
			moveRight = false;
		}


		
	}







	// Update is called once per frame
	void FixedUpdate () {
		if( moveForward ){
			rb.AddForce(0,0,movementForce*Time.deltaTime,ForceMode.VelocityChange);
		}


		if( moveBackwards){
			rb.AddForce(0,0,-movementForce*Time.deltaTime,ForceMode.VelocityChange);
		}


		if( moveLeft ){
			rb.AddForce(-movementForce*Time.deltaTime,0,0,ForceMode.VelocityChange);
		}

		if( moveRight ){
			rb.AddForce(movementForce*Time.deltaTime,0,0,ForceMode.VelocityChange);
		}


		
	}



*/



}
