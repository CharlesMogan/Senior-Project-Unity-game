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
	protected bool isFiringLaser = false;
	
	
	public Laser laser;
	public float laserDamage;
	public float laserLifetime;
	protected float lasersPerBurst = 13;
	public float bulletSpeed;
	public float laserChargeDelay;
	public float laserDiameter;
	public float laserRange;
	
	
	public Bullet bullet;
	public float bulletLifetime;
	public float fireRate;
	public float transferedMomentum;
	public float bulletDamage;
	public float bulletScale;
	
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
		nextFire = Time.time + laserChargeDelay+laserLifetime;
		IEnumerator paralizeShooter = shooterMovement.ParalyzeForTime(laserChargeDelay+laserLifetime);
		StartCoroutine(paralizeShooter);
		yield return new WaitForSeconds(laserChargeDelay);
		float laserFragmentLifetime = laserLifetime/lasersPerBurst;
		for(int i = 0; i < lasersPerBurst; i++){
			ShootLaser(laserFragmentLifetime);			
			yield return new WaitForSeconds(laserFragmentLifetime);
		}
		isFiringLaser = false;
	}


	protected void ShootLaser(float laserLifetime){
		foreach(Transform gun in guns){
			Laser lastLaser= Instantiate(laser, gun.position, gun.rotation);
			lastLaser.Damage = laserDamage;
			Transform lastLaserTransform = lastLaser.GetComponent<Transform>();
			lastLaserTransform.localScale = new Vector3(laserDiameter,laserRange,laserDiameter);
			Vector3 yLessVelocity = new Vector3(characterVelocity.x,0.0f,characterVelocity.z);
			Destroy(lastLaser.gameObject, laserLifetime);
		}
	}



	protected void Shoot(){
		foreach(Transform gun in guns){
			nextFire = Time.time + fireRate;
			Bullet lastBullet= Instantiate(bullet, gun.position+gun.forward*.55f + gun.forward*.5f*bulletScale, gun.rotation);
			lastBullet.Damage = bulletDamage; 
			Transform lastBulletTransform = lastBullet.GetComponent<Transform>();
			Vector3 yLessVelocity = new Vector3(characterVelocity.x,0.0f,characterVelocity.z); 
			Rigidbody lastBulletRigedBody = lastBullet.GetComponent<Rigidbody>();
			lastBulletRigedBody.velocity = transferedMomentum*yLessVelocity + lastBulletTransform.forward*bulletSpeed; //http://answers.unity3d.com/questions/808262/how-to-instantiate-a-prefab-with-initial-velocity.html
			lastBulletTransform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
			Debug.Log(lastBullet);
			Destroy(lastBullet.gameObject, bulletLifetime);  /// for some reason you have to get the game object, compiles either way, just doesn't work.
		}
	}


	public void BulletDamageUp(){
		bulletDamage += 10;
		if(bulletDamage > 100){
			bulletDamage = 100;
		}
	}

	
	public void BulletSpeedUp(){
		bulletSpeed += 10;
		if(bulletSpeed > 100){
			bulletSpeed = 100;
		}
	}

	public void BulletLifetimeUp(){
		bulletLifetime += .5f;
		if(bulletLifetime > 10){
			bulletLifetime = 10;
		}
	}

	public void LaserDamageUp(){
		laserDamage += 10;
		if(laserDamage > 100){
			laserDamage = 100;
		}
	}

	public void laserRangeUp(){
		laserRange += 10;
		if(laserRange > 100){
			laserLifetime = 100;
		}
	}

	public void LaserLifetimeUp(){
		laserLifetime += 10;
		if(laserLifetime > 100){
			laserLifetime = 100;
		}
	}


	public void BulletFireRateUp(){
		fireRate = fireRate*.75f;
		if(fireRate < 0.01f){
			fireRate = 0.01f;
		}
	}

	public void BulletSizeUp(){
		bulletScale = bulletScale*1.5f;
		if(bulletScale > 0.99f){
			bulletScale = 0.99f;
		}
	}

	public void LaserDiameterUp(){
		laserDiameter = laserDiameter*1.35f;
		if(laserDiameter > 2){
			laserDiameter = 2;
		}
	}

	public void LaserChargeDelayDown(){
		laserChargeDelay = laserChargeDelay*.75f;
		if(laserChargeDelay < .1f){
			laserChargeDelay = .1f;
		}
	}
}





