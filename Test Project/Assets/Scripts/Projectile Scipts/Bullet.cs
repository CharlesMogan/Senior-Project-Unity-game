using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : Projectile {
	public abstract void OnTriggerEnter(Collider other);
}
