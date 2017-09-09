using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	protected virtual void OnTriggerEnter(Collider other) {
       	if(other.gameObject.tag != "Player"){
       		
    		Destroy(this.gameObject);
    	}
    }

}
