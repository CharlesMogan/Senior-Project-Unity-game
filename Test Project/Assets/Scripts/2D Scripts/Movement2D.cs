using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour {
	public float speed;
	protected Rigidbody2D rb;
	protected Transform character;

	protected Vector2 movement;
	

	// Use this for initialization
	protected void Start () {
		rb = GetComponent<Rigidbody2D>();
		character = GetComponent<Transform>();
	}
		
	
	
}
