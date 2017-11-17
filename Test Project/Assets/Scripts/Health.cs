using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public float maxHealth;
	public float health;
	public float DamageDelay;
	protected float timeWhenDamageable = 0;
	protected WorldMaker.Room myRoom;


	public void WhoIsMyRoom(WorldMaker.Room myRoom){
		this.myRoom = myRoom;
	}
	
	protected virtual void Die(){
		Debug.Log("override this");
	}


	public virtual void TakeDamage(float damage){
		if(Time.time >= timeWhenDamageable){
			health-=damage;
			timeWhenDamageable = Time.time + DamageDelay;	
		}
		if(health <= 0){
			Debug.Log("this is get destroyed with a health of" + health);
			Die();
		}
		
	}


	public virtual void MaxHealthUp(){
		maxHealth += 10;
		health += 10;
	}

	public virtual void HealthUp(){
		health += 10;
		if(health>maxHealth){
			health = maxHealth;
		}
	}

}
