using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conways : MonoBehaviour {

	public GameObject cube;
	bool[,] world = new bool[36,9];
	float genDelay;


	// Use this for initialization
	void Start () {
		genDelay = 0;
		seed();
		draw();
	}

	void draw(){
		for (int i = 0; i < world.GetLength(0); i++){
			for (int j = 0; j < world.GetLength(1); j++){
				if(world[i,j]){
					GameObject myCube = Instantiate(cube, new Vector3(i+20,-20,j+20), Quaternion.identity);
					Destroy(myCube,500);
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
		world[24,0] = true;
		
		world[22,1] = true;
		world[24,1] = true;
		
		world[12,2] = true;
		world[13,2] = true;
		world[20,2] = true;
		world[21,2] = true;
		world[34,2] = true;
		world[35,2] = true;


		world[11,3] = true;
		world[15,3] = true;
		world[20,3] = true;
		world[21,3] = true;
		world[34,3] = true;
		world[35,3] = true;


		world[0,4] = true;
		world[1,4] = true;
		world[10,4] = true;
		world[16,4] = true;
		world[20,4] = true;
		world[21,4] = true;
		

		world[0,5] = true;
		world[1,5] = true;
		world[10,5] = true;
		world[14,5] = true;
		world[16,5] = true;
		world[17,5] = true;
		world[22,5] = true;
		world[24,5] = true;



		world[10,6] = true;
		world[16,6] = true;
		world[24,6] = true;


		world[11,7] = true;
		world[15,7] = true;

		world[12,8] = true;
		world[13,8] = true;
		


	}


	void nextGeneration(){
		bool[,] tempWorld = new bool[36,9];
		for (int i = 0; i < world.GetLength(0); i++){
			for (int j = 0; j < world.GetLength(1); j++){
				int neighbors = getMooreNeighborhood(i,j);

				if(neighbors < 2){
					tempWorld[i,j] = false;
				}else if(neighbors > 3){
					tempWorld[i,j] = false;
				}else if(world[i,j] && (neighbors == 2 || neighbors == 3)){
					Debug.Log("tacos");
				}else if(world[i,j] == false && neighbors == 3){
					tempWorld[i,j] = true;
				}
			}
		}
		world = tempWorld;
	}



	int getMooreNeighborhood(int x, int y){
		int sum = 0;
		for(int i = -1; i <= 1; i++){
			for(int j = -1; j <= 1; j++){
				Debug.Log("tryingTo acsess"+ (x+i)+" "+(y+j));
				if(x+i>-1 && y+j>-1 && x+i<36 && y+j < 9 && (y!=0 || x!=0)){           ///this is real bad, need to fix
					if(world[x+i,y+j]){
						sum += 1;
					}
				}
			}
		}
		return sum;
	}




	// Update is called once per frame
	void Update () {
		if(Time.time> genDelay){
		//	nextGeneration();
		//	draw();
		//	genDelay = Time.time + 2.2f;
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