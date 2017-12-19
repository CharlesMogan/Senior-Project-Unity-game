using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletShooting : Shooting {
	public float bulletSpeed;
	public float fireRate;
	public Bullet bullet;





	protected void Shoot(){
		foreach(Transform gun in guns){
			if(gun != null){
				nextFire = Time.time + fireRate;
				Bullet lastBullet= Instantiate(bullet, gun.position+gun.forward*.55f + gun.forward*.5f*shotDiameter, gun.rotation);
				lastBullet.Damage = shotDamage; 
				Transform lastBulletTransform = lastBullet.GetComponent<Transform>();
				//Vector3 yLessVelocity = new Vector3(characterVelocity.x,0.0f,characterVelocity.z); 
				Rigidbody lastBulletRigedBody = lastBullet.GetComponent<Rigidbody>();
				lastBulletRigedBody.velocity = /*transferedMomentum*yLessVelocity + new Vector3(characterAngularVelocity.x,0,characterAngularVelocity.z) * transferedMomentum +*/ lastBulletTransform.forward*bulletSpeed; //http://answers.unity3d.com/questions/808262/how-to-instantiate-a-prefab-with-initial-velocity.html
				lastBulletTransform.localScale = new Vector3(shotDiameter, shotDiameter, shotDiameter);
				Destroy(lastBullet.gameObject, shotLifetime);  /// for some reason you have to get the game object, doesn't complain either way, just doesn't work.
			}
		}
	}




	public void BulletDamageUp(){
		shotDamage += 10;
		if(shotDamage > 100){
			shotDamage = 100;
		}
	}

	
	public void BulletSpeedUp(){
		bulletSpeed += 10;
		if(bulletSpeed > 100){
			bulletSpeed = 100;
		}
	}

	public void BulletLifetimeUp(){
		shotLifetime += .5f;
		if(shotLifetime > 10){
			shotLifetime = 10;
		}
	}

	


	public void BulletFireRateUp(){
		fireRate = fireRate*.75f;
		if(fireRate < 0.01f){
			fireRate = 0.01f;
		}
	}

	public void BulletSizeUp(){
		shotDiameter = shotDiameter*1.5f;
		if(shotDiameter > 0.99f){
			shotDiameter = 0.99f;
		}
	}


}
