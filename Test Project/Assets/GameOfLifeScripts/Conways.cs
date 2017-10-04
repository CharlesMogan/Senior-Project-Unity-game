using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conways : MonoBehaviour {

	public GameObject cube;
	bool[,] world = new bool[50,25];
	float genDelay;


	// Use this for initialization
	void Start () {
		genDelay = 0;
		seed();
		if(world[1,5] == false){
			Debug.Log("genBad");
		}
	}

	void draw(){
		if(world[1,5] == false){
			Debug.Log("drawbad");
		}

		for (int i = 0; i < world.GetLength(0); i++){
			for (int j = 0; j < world.GetLength(1); j++){
				if(world[i,j]){
					GameObject myCube = Instantiate(cube, new Vector3(i,-20,j), Quaternion.identity);
					Destroy(myCube,1f);
				}
			}
		}
	}


	void seed(){
		for (int i = 0; i < world.GetLength(0); i++){
			for (int j = 0; j < world.GetLength(1); j++){
				world[i,j] = false;
			}
		}
		
		int offset = 4;
		world[1+offset,5+offset] = true;
		world[1+offset,4+offset] = true;

		world[2+offset,5+offset] = true;
		world[2+offset,4+offset] = true;
		
		world[11+offset,5+offset] = true;
		world[11+offset,4+offset] = true;
		world[11+offset,3+offset] = true;

		world[12+offset,6+offset] = true;
		world[12+offset,2+offset] = true;

		world[13+offset,7+offset] = true;
		world[13+offset,1+offset] = true;

		world[14+offset,7+offset] = true;
		world[14+offset,1+offset] = true;

		world[15+offset,4+offset] = true;

		world[16+offset,6+offset] = true;
		world[16+offset,2+offset] = true;

		world[17+offset,5+offset] = true;
		world[17+offset,4+offset] = true;
		world[17+offset,3+offset] = true;

		world[18+offset,4+offset] = true;

		world[21+offset,7+offset] = true;
		world[21+offset,6+offset] = true;
		world[21+offset,5+offset] = true;

		world[22+offset,7+offset] = true;
		world[22+offset,6+offset] = true;
		world[22+offset,5+offset] = true;

		world[23+offset,8+offset] = true;
		world[23+offset,4+offset] = true;

		world[25+offset,9+offset] = true;
		world[25+offset,8+offset] = true;
		world[25+offset,4+offset] = true;
		world[25+offset,3+offset] = true;


		world[35+offset,7+offset] = true;
		world[35+offset,6+offset] = true;

		world[36+offset,7+offset] = true;
		world[36+offset,6+offset] = true;

	}


	void nextGeneration(){
		bool[,] tempWorld = new bool[50,25];
		for (int i = 0; i < world.GetLength(0); i++){
			for (int j = 0; j < world.GetLength(1); j++){
				tempWorld[i,j] = world[i,j];
			}
		}
		if(world[1,5] == false){
			Debug.Log("bad");
		}

		for (int i = 0; i < world.GetLength(0); i++){
			for (int j = 0; j < world.GetLength(1); j++){
				if(world[1,5] == false){
					Debug.Log("bad for" +i+" "+j);
				}
				int neighbors = getMooreNeighborhood(i,j);
				if(i == 1 && j == 5){
					Debug.Log("neighbors= "+neighbors);
				}
				if(neighbors < 2){
					tempWorld[i,j] = false;
				}else if(neighbors > 3){
					if(world[1,5] == false){
						Debug.Log("bad3");
					}
					tempWorld[i,j] = false;
				}else if(world[i,j] == false && neighbors == 3){
					tempWorld[i,j] = true;
				}
			}
		}
		world = tempWorld;
		if(world[1,5] == false){
			Debug.Log("bad3");
		}
	}
	


	int getMooreNeighborhood(int x, int y){
		if(world[1,5] == false){
			Debug.Log("moorebad");
		}
		int sum = 0;
		for(int i = -1; i <= 1; i++){
			for(int j = -1; j <= 1; j++){
				//Debug.Log("tryingTo acsess"+ (x+i)+" "+(y+j));
				if(x+i>-1 && y+j>-1 && x+i<world.GetLength(0) && y+j < world.GetLength(1)){           ///this is real bad, need to fix
					if(world[x+i,y+j]){
						sum += 1;
					}
				}
			}
		}
		if (world[x,y]){
			sum-=1;
		}
		if(world[1,5] == false){
			Debug.Log("moore2bad");
		}
		return sum;
	}




	// Update is called once per frame
	void Update () {
		if(Time.time> genDelay){
			draw();
			nextGeneration();
			genDelay = Time.time + 1f;
		}

	}
}

			//12-13
/*                      24
........................O           35       0
......................O.O                  1     
............OO......OO............OO       2
...........O...O....OO............OO       3
OO........O.....O...OO                     4
OO........O...O.OO....O.O                  5
..........O.....O.......O                  6
...........O...O                           7
............OO                             8
              
*/