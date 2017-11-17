using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserShooting : Shooting {

	void Update () {
		if(Time.time > nextFire && isFiringLaser == false){
			//isFiringLaser  = true;
			IEnumerator delayShot = ShootWithDelay();
			StartCoroutine(delayShot);
		}
	}
}
