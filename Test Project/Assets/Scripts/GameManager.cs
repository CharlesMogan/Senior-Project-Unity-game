using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private System.Random random;
	public bool seedIsSetable; 
	public int seed;

	void Start(){
		//Debug.Log(DateTime.Now.Millisecond);
		if(seedIsSetable){
			random = new System.Random(seed);
		}else{
			random = new System.Random(DateTime.Now.Millisecond);   //https://msdn.microsoft.com/en-us/library/system.random(v=vs.110).aspx       https://msdn.microsoft.com/en-us/library/system.datetime.millisecond(v=vs.110).aspx
		}
	}


	public int NextRandom(int lowerBound, int upperBound){
		return random.Next(lowerBound, upperBound);
	}


	void EndGame(){
		Debug.Log("game over script active");
	}
}