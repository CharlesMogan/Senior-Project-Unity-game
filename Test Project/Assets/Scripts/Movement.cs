using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	public float speed;
	protected Rigidbody rb;
	protected Transform character;

	protected Vector3 movement;
	

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		character = GetComponent<Transform>();
	}
		
	
	
}
