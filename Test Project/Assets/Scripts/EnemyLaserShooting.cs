using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserShooting : Shooting {

	void Update () {
		if(Time.time > nextFire && isFiring == false){
			isFiring = true;
			IEnumerator delayShot = ShootWithDelay();
			StartCoroutine(delayShot);
		}
	}
}
