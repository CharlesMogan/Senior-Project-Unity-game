using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	protected virtual void OnTriggerEnter(Collider other) {
      	Debug.Log("Impact1");
       	if(other.gameObject.tag == "Player"){
       		Debug.Log("Impact2");
       		GameObject gameManager = GameObject.FindWithTag("GameController");
       		GameManager gameManagerScript = gameManager.GetComponent<GameManager>(); //http://answers.unity3d.com/questions/305614/get-script-variable-from-collider.html

       		float randomNum = gameManagerScript.nextRandom();
       		if(randomNum < 1000){
       			Health health = other.GetComponent<Health>();
       			health.TakeDamage(100000000000);
       		}
    		Destroy(this.gameObject);
    	}
    }

}
