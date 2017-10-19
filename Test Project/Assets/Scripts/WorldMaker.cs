using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMaker : MonoBehaviour {
	//public GameObject gameManager = GameObject.FindWithTag("GameController");
    //public GameManager gameManagerScript = gameManager.GetComponent<GameManager>(); //http://answers.unity3d.com/questions/305614/get-script-variable-from-collider.html

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
		public bool IsOn{
			get{return isOn;}

			set{isOn = value;}
		}
	}

	class World{
		public World(){
			Room firstRoom = new Room(50,50);
		}
	}
	


	class Room{
		GameObject gameManager;
       	GameManager gameManagerScript;
		public GameObject cube;
		int rows;
		int cols;


		public Room(int xDimension, int yDimension){
		gameManager = GameObject.FindWithTag("GameController");
       	gameManagerScript = gameManager.GetComponent<GameManager>();
			int rows = xDimension;
			int cols = yDimension;
			Cell[,] room = new Cell[rows,cols];
			for (int i = 0; i < room.GetLength(0); i++){
				for (int j = 0; j < room.GetLength(1); j++){
					float ranNum = gameManagerScript.NextRandom(1,3);
					Debug.Log("random number is" + ranNum);
					if(ranNum < 1.5){
						room[i,j]= new Cell(true,false,false);
					}else{
						room[i,j]= new Cell(false,false,false);
					}
				}
			}








			for (int i = 0; i < room.GetLength(0); i++){
				for (int j = 0; j < room.GetLength(1); j++){
					if(room[i,j].IsOn){
						//GameObject myCube = Instantiate(cube, new Vector3(i,-20,j), Quaternion.identity);
						GameObject myCube = Instantiate(Resources.Load("Cuber") as GameObject, new Vector3(i,-20,j), Quaternion.identity);
						Debug.Log("cuber loop");
						Destroy(myCube,111.09f);
					}
				}
			}			
		}

		



		void nextGeneration(){
			bool[,] tempRoom = new bool[rows,cols];
			for (int i = 0; i < rows; i++){
				for (int j = 0; j < cols; j++){
					tempWorld[i,j] = room[i,j].IsOn;
				}
			}

			for (int i = 0; i < rows; i++){
				for (int j = 0; j < cols; j++){
					tempWorld[i,j] = B3S23(i,j);
				}
			}


			for (int i = 0; i < rows; i++){
				for (int j = 0; j < cols; j++){
					room[i][j].IsOn = tempWorld[i][j];
				}
			}


			world = tempWorld;
			
		}



		bool B3S23(int x, int y){                             //conway's rules
			int neighbors = getMooreNeighborhood(x,y);
					if(neighbors < 2){
						return false;
					}else if(neighbors > 3){
						return false;
					}else if(room[i,j] == false && neighbors == 3){
						return true;
					}
				return false;
		}






















		int getMooreNeighborhood(int x, int y){
				int sum = 0;
				for(int i = -1; i <= 1; i++){
					for(int j = -1; j <= 1; j++){
						//Debug.Log("tryingTo acsess"+ (x+i)+" "+(y+j));
						if(x+i>-1 && y+j>-1 && x+i < rows && y+j < cols){           ///this is real bad, need to fix
							if(room[x+i,y+j]){
								sum += 1;
							}
						}
					}
				}
				if (room[x,y]){
					sum-=1;
				}
				return sum;
			}
			

	}	




	// Use this for initialization
	void Start () {
		World myWorld = new World();

	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
