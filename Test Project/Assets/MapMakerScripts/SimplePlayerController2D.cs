using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController2D : MonoBehaviour {


	Rigidbody2D rb;
	Vector2 velocity;
	// Use this for initialization
	void Start () {
		rb =  GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized*10;
	}

	void FixedUpdate(){
		rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
	}
}
