using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour {
	public float speed;
	protected float characterScale = 1;
	protected Rigidbody rb;
	protected Transform character;
	protected Vector3 movement;

	


	protected void Start () {
		rb = GetComponent<Rigidbody>();
		character = GetComponent<Transform>();
	}


	public void SpeedUp(){
		speed += 2;
		if(speed > 20){
			speed = 20;
		}
	}

	
	/*public void ScaleDown(){
		characterScale = characterScale*.8f;
		if(characterScale < .5f){
			characterScale = .5f;
		}
		character.localScale = new Vector3(characterScale, characterScale, characterScale);
	}*/
		
}
