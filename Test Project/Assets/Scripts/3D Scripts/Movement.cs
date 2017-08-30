using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	public float speed;
	protected Rigidbody rb;
	protected Transform character;
	protected Vector3 movement;
	protected bool isParalyzed;
	

	// Use this for initialization
	protected void Start () {
		rb = GetComponent<Rigidbody>();
		character = GetComponent<Transform>();
	}



	public IEnumerator ParalyzeForTime(float time){
		isParalyzed = true;
		yield return new WaitForSeconds(time);
		isParalyzed = false;

	}

	public bool Paralyzed{
		get{return isParalyzed;}

		set{isParalyzed = value;}
	}
		
	
	
}
