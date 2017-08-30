using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
	protected float time, nextFire;
	protected List <Transform> guns;
	protected Transform shooter;
	protected Rigidbody shooterRB;
	protected Movement shooterMovement;
	protected Vector3 previousPosition;
	protected Vector3 characterVelocity;

	
	public GameObject bullet;
	public float bulletLifetime;
	public float bulletSpeed;
	public float fireRate;
	public float shotChargeTime;

	public float transferedMomentum;

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

	void FixedUpdate(){
		if (Time.fixedDeltaTime != 0){
			characterVelocity = (shooterRB.position - previousPosition) / Time.fixedDeltaTime;
			previousPosition = shooterRB.position;
		}
	}




	protected IEnumerator ShootWithDelay(){    
		nextFire = Time.time + fireRate + shotChargeTime;
		if(shotChargeTime != 0){
			IEnumerator paralizeShooter = shooterMovement.ParalyzeForTime(shotChargeTime);
			StartCoroutine(paralizeShooter);
		}
		yield return new WaitForSeconds(shotChargeTime);
		Shoot();
	}


	protected void Shoot(){
		foreach(Transform gun in guns){
			GameObject lastBullet= Instantiate(bullet, gun.position, gun.rotation);
			Transform lastBulletTransform = lastBullet.GetComponent<Transform>();
			Rigidbody lastBulletRigedBody = lastBullet.GetComponent<Rigidbody>();
			Vector3 yLessVelocity = new Vector3(characterVelocity.x,0.0f,characterVelocity.z); 
			lastBulletRigedBody.velocity = transferedMomentum*yLessVelocity + lastBulletTransform.forward*bulletSpeed; //http://answers.unity3d.com/questions/808262/how-to-instantiate-a-prefab-with-initial-velocity.html
			Destroy(lastBullet, bulletLifetime);
		}
	}



}
