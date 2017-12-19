using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserShooting : LaserShooting {
	void Update () {
		if(Input.GetButton("Fire2") &&  Time.time > nextFire){
			IEnumerator delayShot = ShootWithDelay();
			StartCoroutine(delayShot);
		}

	}
}
