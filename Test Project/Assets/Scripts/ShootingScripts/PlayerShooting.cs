using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : BulletShooting {
	void Update () {
		if(Input.GetButton("Fire1") &&  Time.time > nextFire){
			Shoot();
		}
	}
}


