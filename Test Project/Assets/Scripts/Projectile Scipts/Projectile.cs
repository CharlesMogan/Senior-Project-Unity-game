using UnityEngine;

public class Projectile : MonoBehaviour {
	protected float projectileDamage;
	private static float projectileDamage2;

	public float Damage{
		get{return projectileDamage;}

		set{projectileDamage = value;}
	}
}
