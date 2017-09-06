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
	protected bool isFiring = false;
	
	public GameObject bullet;
	public GameObject laser;
	public float bulletLifetime;
	public float bulletSpeed;
	public float fireRate;
	public float shotChargeDelay;
	public float BurstDelay;
	public float transferedMomentum;
	public int shotsPerBurst;


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




	/*protected IEnumerator ShootWithDelay(){    
		nextFire = Time.time + fireRate;
		if(shotChargeDelay != 0){
			IEnumerator paralizeShooter = shooterMovement.ParalyzeForTime(shotChargeDelay+bulletLifetime);
			StartCoroutine(paralizeShooter);
		}
		yield return new WaitForSeconds(shotChargeDelay);
		if(bullet.tag == "Friendly Bullet" || bullet.tag == "Unfriendly Bullet"){
			Shoot();
		}else if(laser.tag == "Friendly Laser" || laser.tag == "Unfriendly Laser"){
			nextFire = Time.time + fireRate + shotChargeDelay;
			shotsPerBurst = 13;
			float laserLifetime = bulletLifetime/shotsPerBurst;
			for(int i = 0; i < shotsPerBurst; i++){
				ShootLaser(laserLifetime);			
				yield return new WaitForSeconds(laserLifetime);
				isFiring = false;
			}
		}else{

			Debug.Log("it is just as I feared");
		}
	}*/


	protected IEnumerator ShootWithDelay(){    
		nextFire = Time.time + fireRate;
		IEnumerator paralizeShooter = shooterMovement.ParalyzeForTime(shotChargeDelay+bulletLifetime);
		StartCoroutine(paralizeShooter);
		yield return new WaitForSeconds(shotChargeDelay);
		nextFire = Time.time + fireRate + shotChargeDelay;
		shotsPerBurst = 13;
		float laserLifetime = bulletLifetime/shotsPerBurst;
		for(int i = 0; i < shotsPerBurst; i++){
			ShootLaser(laserLifetime);			
			yield return new WaitForSeconds(laserLifetime);
			isFiring = false;
		}
		
	}


	protected void ShootLaser(float laserLifetime){
		foreach(Transform gun in guns){
			Debug.Log("actually shooting a laser");
			GameObject lastLaser= Instantiate(laser, gun.position, gun.rotation);
			Transform lastLaserTransform = lastLaser.GetComponent<Transform>();
			Vector3 yLessVelocity = new Vector3(characterVelocity.x,0.0f,characterVelocity.z);
			Destroy(lastLaser, laserLifetime);
		}
	}



	protected void Shoot(){
		foreach(Transform gun in guns){
			nextFire = Time.time + fireRate;
			GameObject lastBullet= Instantiate(bullet, gun.position, gun.rotation);
			Transform lastBulletTransform = lastBullet.GetComponent<Transform>();
			Vector3 yLessVelocity = new Vector3(characterVelocity.x,0.0f,characterVelocity.z); 
			Rigidbody lastBulletRigedBody = lastBullet.GetComponent<Rigidbody>();
				lastBulletRigedBody.velocity = transferedMomentum*yLessVelocity + lastBulletTransform.forward*bulletSpeed; //http://answers.unity3d.com/questions/808262/how-to-instantiate-a-prefab-with-initial-velocity.html
			Destroy(lastBullet, bulletLifetime);
		}
	}

}
