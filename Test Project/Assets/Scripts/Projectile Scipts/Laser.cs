using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile{
	public int ticksPerSecond;
	private Transform laser;

	// Use this for initialization
	void Start () {
		laser = GetComponent<Transform>();
		laser.position += laser.forward * (laser.localScale.y -0.5f);
		laser.eulerAngles += new Vector3(0,90,90);
	}
	
}
