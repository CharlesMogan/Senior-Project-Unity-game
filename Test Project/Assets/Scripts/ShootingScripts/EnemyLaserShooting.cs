using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserShooting : LaserShooting {

	void Update () {
		if(Time.time > nextFire){
			IEnumerator delayShot = ShootWithDelay();
			StartCoroutine(delayShot);
		}
	}
}
