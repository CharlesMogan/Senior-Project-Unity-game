using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health {
	WorldMaker.Room myRoom;
	


	public void WhoIsMyRoom(WorldMaker.Room myRoom){
		this.myRoom = myRoom;
	}

	protected override void Die(){
		Debug.Log("you died");
		myRoom.EnemyDied();
		GameObject gameManager = GameObject.FindWithTag("GameController");
		GameManager gameManagerScript = gameManager.GetComponent<GameManager>();

		if(gameManagerScript.NextRandom(1,11) > 9){
			Debug.Log("should spawn health now");
		}
		Destroy(this.gameObject);
	}

}
