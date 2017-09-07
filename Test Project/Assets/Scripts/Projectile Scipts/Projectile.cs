using UnityEngine;

public class Projectile : MonoBehaviour {
	public float projectileDamage;
	private static float projectileDamage2;

	public float Damage{
		get{return projectileDamage;}

		set{projectileDamage = value;}
	}
	
}
