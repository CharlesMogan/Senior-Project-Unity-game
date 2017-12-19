using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private System.Random random;
	private WorldMaker.World world;
	private WorldMaker.Room activeRoom;
	public bool seedIsSetable; 
	public int seed;
	private int livingEnemies;

	void Start(){
		livingEnemies = 0;
		if(seedIsSetable){
			random = new System.Random(seed);
		}else{
			seed = DateTime.Now.Millisecond;
			random = new System.Random(seed);   //https://msdn.microsoft.com/en-us/library/system.random(v=vs.110).aspx       https://msdn.microsoft.com/en-us/library/system.datetime.millisecond(v=vs.110).aspx
		}
		world = new WorldMaker.World();
	}


	public int NextRandom(int lowerBound, int upperBound){
		return random.Next(lowerBound, upperBound);
	}


	public int LivingEnemies{
		get{return livingEnemies;}

		set{livingEnemies = value;}
	}


	public void EnemyDied(Vector3 deathLocation){
		livingEnemies--;
		if (livingEnemies < 1){
			activeRoom.EndCombat(deathLocation);
		}
	}

	public void EnteredCombatInRoom(WorldMaker.Room room){
		activeRoom = room;
	}




	public int Seed{
		get{return seed;}
	}

	public void EndGame(){
		SceneManager.LoadScene(0);
	}
}