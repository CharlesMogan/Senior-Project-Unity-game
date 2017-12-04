using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooting : Shooting {
	protected float lasersPerBurst = 13;
	public float laserChargeDelay;
	public float laserRange;
	public Laser laser;



	protected IEnumerator ShootWithDelay(){    
		nextFire = Time.time + laserChargeDelay+shotLifetime;
		shooterMovement.enabled = false;
		yield return new WaitForSeconds(laserChargeDelay);
		float laserFragmentLifetime = shotLifetime/lasersPerBurst;
		for(int i = 0; i < lasersPerBurst; i++){
			ShootLaser(laserFragmentLifetime);			
			yield return new WaitForSeconds(laserFragmentLifetime);
		}
		shooterMovement.enabled = true;
		//isFiringLaser = false;
	}


	protected void ShootLaser(float shotLifetime){
		foreach(Transform gun in guns){
			if(gun != null){
				Laser lastLaser= Instantiate(laser, gun.position, gun.rotation);
				lastLaser.Damage = shotDamage;
				Transform lastLaserTransform = lastLaser.GetComponent<Transform>();
				lastLaserTransform.localScale = new Vector3(shotDiameter,laserRange,shotDiameter);
				//Vector3 yLessVelocity = new Vector3(characterVelocity.x,0.0f,characterVelocity.z);
				Destroy(lastLaser.gameObject, shotLifetime);
			}
		}
	}

	public void LaserDiameterUp(){
		shotDiameter = shotDiameter*1.35f;
		if(shotDiameter > 2){
			shotDiameter = 2;
		}
	}

	public void LaserChargeDelayDown(){
		laserChargeDelay = laserChargeDelay*.75f;
		if(laserChargeDelay < .1f){
			laserChargeDelay = .1f;
		}
	}

	public void LaserDamageUp(){
		shotDamage += 10;
		if(shotDamage > 100){
			shotDamage = 100;
		}
	}

	public void laserRangeUp(){
		laserRange += 10;
		if(laserRange > 100){
			shotLifetime = 100;
		}
	}



	/*public void LaserLifetimeUp(){   //this should be considered nonfunctional until the way a lasers damage is calculated changes
		laserLifetime += 10;
		if(laserLifetime > 100){
			laserLifetime = 100;
		}
	}*/
}
