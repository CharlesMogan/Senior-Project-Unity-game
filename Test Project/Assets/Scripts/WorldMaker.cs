using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMaker : MonoBehaviour {
public GameObject cube;

	class Cell{
		bool isOn;
		bool isOuterWall;
		bool isDoor;
		public Cell(bool isOn, bool isOuterWall, bool isDoor){
			this.isOn = isOn;
			this.isOuterWall = isOuterWall; 
			this.isDoor = isDoor;
			// if it is not on the outer wall I need to add health at somepoint.
		}
	}

	class World{
		public World(){
			Room firstRoom = new Room(50,50);
		}

	}
	


	class Room{
		int rows;
		int cols;
		public Room(int xDimension, int yDimension){
			int rows = xDimension;
			int cols = yDimension;
			Cell[,] room = new Cell[rows,cols];

			for (int i = 0; i < room.GetLength(0); i++){
				for (int j = 0; j < room.GetLength(1); j++){
					room[i,j]= new Cell(true,false,false);
				}
			}



			//testing

		for (int i = 0; i < room.GetLength(0); i++){
			for (int j = 0; j < room.GetLength(1); j++){
				if(room[i,j]){             ---------------------------------------------------------------
					GameObject myCube = Instantiate(cube, new Vector3(i,-20,j), Quaternion.identity);
					Destroy(myCube,.09f);
				}
			}
		}

		
		}



		/*int getMooreNeighborhood(int x, int y){
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
		return sum;
		}
		*/

	}

	

	// Use this for initialization
	void Start () {
	World myWorld = new World();		
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
