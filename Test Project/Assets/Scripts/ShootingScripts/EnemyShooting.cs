using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : BulletShooting {

	void Update () {
		if(Time.time > nextFire){
			Shoot();
		}
	}
}
