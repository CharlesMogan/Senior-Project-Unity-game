﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public float maxHealth;
	public float health;
	public float DamageDelay;
	protected float timeWhenDamageable = 0;
	// Update is called once per frame
	
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


	public void MaxHealthUp(){
		maxHealth += 10;
		health += 10;
	}

	public void healthUp(){
		health += 10;
		if(health>maxHealth){
			health = maxHealth;
		}
	}

}
