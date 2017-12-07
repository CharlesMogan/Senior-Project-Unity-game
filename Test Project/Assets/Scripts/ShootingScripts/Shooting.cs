using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooting : MonoBehaviour {
	protected float time, nextFire;
	protected List <Transform> guns;
	protected Transform shooter;
	protected Rigidbody shooterRB;
	protected Movement shooterMovement;
	//protected Vector3 previousPosition;
	//protected Vector3 characterVelocity;
	//protected Vector3 previousRotation;
	//protected Vector3 characterAngularVelocity;
	
	public float shotDamage;
	public float shotLifetime;
	public float shotDiameter;

	

	
	//public float transferedMomentum;
	
	
	
	// Use this for initialization
	void Start () {
		nextFire = Time.time;
		shooter = GetComponent<Transform>();
		shooterRB = GetComponent<Rigidbody>();
		shooterMovement = GetComponent<Movement>();
		guns = new List<Transform>();
		foreach(Transform child in shooter){
			if(child.tag == "Gun"){
				guns.Add(child);	
			}
		}
	}	

	/*void FixedUpdate(){
 		if (Time.fixedDeltaTime != 0){
			characterVelocity = (shooterRB.position - previousPosition) / Time.fixedDeltaTime;
			previousPosition = shooterRB.position;
			characterAngularVelocity = (shooter.eulerAngles - previousRotation) / Time.fixedDeltaTime;
			previousPosition = shooterRB.position;
			previousRotation = shooter.eulerAngles;
		}
	}*/


	






}





