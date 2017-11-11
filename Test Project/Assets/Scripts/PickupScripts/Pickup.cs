using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

protected virtual void OnTriggerEnter(Collider other) {
if(other.gameObject.tag == "Player"){
GameObject gameManager = GameObject.FindWithTag("GameController");
GameManager gameManagerScript = gameManager.GetComponent<GameManager>(); //http://answers.unity3d.com/questions/305614/get-script-variable-from-collider.html
float randomNum = gameManagerScript.NextRandom(1,2);
if(randomNum < 1000){
	Health health = other.GetComponent<Health>();
Shooting shooting = other.GetComponent<Shooting>();
Movement movement = other.GetComponent<Movement>();
//movement.ScaleDown();
//shooting.LaserChargeDelayDown();
//shooting.BulletSizeUp();
//shooting.BulletLifetimeUp();
//shooting.BulletSpeedUp();
//movement.SpeedUp();
	//shooting.BulletFireRateUp();
//shooting.LaserDiameterUp();
//shooting.LaserDamageUp();
//health.MaxHealthUp();
}
Destroy(this.gameObject);
}
}

}
