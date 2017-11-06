using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenTimer : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
		if(2 == Time.frameCount){  // times the procedular generation.
			Debug.Log("Time: " + Time.time);
		}
		
	}
}
