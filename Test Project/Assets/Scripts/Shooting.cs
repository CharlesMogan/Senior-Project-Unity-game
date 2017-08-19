using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
	protected float time, nextFire;
	protected List <Transform> guns;
	protected Transform shooter;
	protected Rigidbody shooterRB;

	
	public GameObject bullet;
	public float bulletLifetime;
	public float bulletSpeed;
	public float fireRate;

	// Use this for initialization
	void Start () {
		nextFire = Time.time;
		shooter = GetComponent<Transform>();
		shooterRB = GetComponent<Rigidbody>();
		guns = new List<Transform>();
		foreach(Transform child in shooter){
			if(child.tag == "Gun"){
				guns.Add(child);	
			}
		}
	}	




	protected void shoot(){
			nextFire = Time.time + fireRate;
			foreach(Transform gun in guns){
				GameObject lastBullet= Instantiate(bullet, gun.position, gun.rotation);
				Transform lastBulletTransform = lastBullet.GetComponent<Transform>();
				Rigidbody lastBulletRigedBody = lastBullet.GetComponent<Rigidbody>();
				Vector3 yLessVelocity = new Vector3(shooterRB.velocity.x,0.0f,shooterRB.velocity.z); 
				lastBulletRigedBody.velocity = yLessVelocity + lastBulletTransform.forward*bulletSpeed; //http://answers.unity3d.com/questions/808262/how-to-instantiate-a-prefab-with-initial-velocity.html
				Destroy(lastBullet, bulletLifetime);
			}
	}
}
