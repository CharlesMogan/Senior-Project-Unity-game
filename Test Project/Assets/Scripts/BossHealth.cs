using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health{
	int phase;
	Transform bossTransform;
	protected WorldMaker.Room myRoom;

	protected override void Start(){
		base.Start();
		gameManager.LivingEnemies++;
		phase = 0;
		bossTransform = GetComponent<Transform>();
	}

	public void WhoIsMyRoom(WorldMaker.Room myRoom){
		this.myRoom = myRoom;
	}

	protected override void Die(){
		gameManager.EnemyDied(this.gameObject.transform.position);
		gameManager.EndGame();
		Destroy(this.gameObject);
	}

	public override void TakeDamage(float damage){
		base.TakeDamage(damage);
		Shooting shootingScript = GetComponent<Shooting>();
		Shooting shootingLaserScript = GetComponent<EnemyLaserShooting>();
		Movement rotater = GetComponent<EnemyRotater>();
		if((float) health / (float) maxHealth < .98f && phase == 0){
			shootingScript.enabled = true;
			rotater.enabled = true;
			phase = 1;
		}
		if((float) health / (float) maxHealth < .8f && phase == 1){
			myRoom.SpawnChasers(5);
			phase = 2;
		}
		if((float) health / (float) maxHealth < .6f && phase == 2){
			myRoom.SpawnChasers(5);
			myRoom.SpawnTurrets(8);
			phase = 3;
		}
		if((float) health / (float) maxHealth < .5f && phase == 3){
			myRoom.SpawnChasers(5);
			myRoom.SpawnTurrets(8);
			int childIndex = 0;
			foreach (Transform child in bossTransform) {
				if(childIndex % 2 == 0){
					Destroy(child.gameObject);
				}
       			childIndex++;
        	}
        	shootingLaserScript.enabled = true;
        	shootingScript.enabled = false;
			phase = 4;
		}

	}

}


// combos

//35 3