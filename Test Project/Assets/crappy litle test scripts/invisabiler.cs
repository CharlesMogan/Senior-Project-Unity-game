using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisabiler : MonoBehaviour {

	MeshRenderer myCube;
	// Use this for initialization
	void Start () {
		myCube = GetComponent<MeshRenderer>();
		//gameObject.SetActive(false);
		//myCube.enabled = false;
	}

	void OnBecameInvisible() {
     	//gameObject.SetActive(false);
     	//gameObject.meshRenderer.enabled = false;
       myCube.enabled = false;
    }
    void OnBecameVisible() {
    	myCube.enabled = true;
    	//gameObject.SetActive(true);
        //enabled = true;
    }
}
