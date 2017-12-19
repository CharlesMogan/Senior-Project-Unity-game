using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is a class for testing how many cube can be displayed before perfomance degredation occurs.
//600 by 600 is about as high as you can go without crashing the editor. but is almost playable when built.
//450 by 450 is about as high as you can go with small camera
// 250 by 250 is stutery with a small preview window.

//with the camera pulled back even 200 by 200 is too much.
public class TooManyCubes : MonoBehaviour {
	public int howManyCubes;
	public GameObject cube;
	
	// Update is called once per frame
	void Start () {
		for(int x = 0; x < howManyCubes; x++){
			for(int z = 0; z < howManyCubes; z++){
				GameObject myCube = Instantiate(cube, new Vector3(x,-20,z), Quaternion.identity);
			}
		}
	}
}
