using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting {
	void Update () {
		if(Input.GetButton("Fire1") &&  Time.time > nextFire){
			Shoot();
		}
		if(Input.GetButton("Fire2") &&  Time.time > nextFire){
			IEnumerator delayShot = ShootWithDelay();
			StartCoroutine(delayShot);
		}

	}
}


