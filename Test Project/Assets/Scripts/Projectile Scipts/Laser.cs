using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile{
	private Transform laser;
	// Use this for initialization
	void Start () {
		laser = GetComponent<Transform>();
		laser.position += laser.forward * (laser.localScale.y - 1);
		laser.eulerAngles += new Vector3(0,90,90);
	}
	
}
