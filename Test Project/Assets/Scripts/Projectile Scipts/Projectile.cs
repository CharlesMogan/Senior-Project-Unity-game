using UnityEngine;

public class Projectile : MonoBehaviour {
	protected float projectileDamage;
	
	public float Damage{
		get{return projectileDamage;}

		set{projectileDamage = value;}
	}
}
