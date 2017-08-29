using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting {
	void Update () {
		if(Input.GetButton("Fire1") &&  Time.time > nextFire){
			Shoot();
		}
	}
}


