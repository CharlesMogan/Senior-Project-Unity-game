using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMaker : MonoBehaviour {
	//public GameObject gameManager = GameObject.FindWithTag("GameController");
    //public GameManager gameManagerScript = gameManager.GetComponent<GameManager>(); //http://answers.unity3d.com/questions/305614/get-script-variable-from-collider.html

	class Cell{
		int xLocation;
		int yLocation;
		bool isOn;
		bool isOuterWall;
		bool isDoor;
		GameObject myCube;
		public Cell(bool isOn, bool isOuterWall, bool isDoor, int x, int y){
			this.isOn = isOn;
			this.isOuterWall = isOuterWall; 
			this.isDoor = isDoor;
			this.xLocation = x;
			this.yLocation = y;
			// if it is not on the outer wall I need to add health at somepoint.
		}

		public bool IsOn{
			get{return isOn;}

			set{isOn = value;}
		}


		public void Draw(){
			Destroy(myCube);
			if(isOn){
				myCube = Instantiate(Resources.Load("Cuber") as GameObject, new Vector3(xLocation,-20,yLocation), Quaternion.identity);
			}
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
		Cell[,] room;

		public Room(int xDimension, int yDimension){
		gameManager = GameObject.FindWithTag("GameController");
       	gameManagerScript = gameManager.GetComponent<GameManager>();
			rows = xDimension;
			cols = yDimension;
			room = new Cell[rows,cols];
			for (int i = 0; i < room.GetLength(0); i++){
				for (int j = 0; j < room.GetLength(1); j++){
					float ranNum = gameManagerScript.NextRandom(1,20);
					if(ranNum == 1){
						Debug.Log("nooooobaadddx");
					}
					if( i == 0 || j == 0 || i == room.GetLength(0)-1 || j == room.GetLength(1)-1){
						room[i,j]= new Cell(true,true,false,i,j);
					}else if(ranNum > 11){
						room[i,j]= new Cell(true,false,false,i,j);
					}else{
						room[i,j]= new Cell(false,false,false,i,j);
					}
				}
			}


			for(int i = 0; i < 4; i++){  //i do ever even
				nextGeneration();
			}
				Draw();	
		}




		void Draw(){
			for (int i = 0; i < room.GetLength(0); i++){
				for (int j = 0; j < room.GetLength(1); j++){
					room[i,j].Draw();
				}
			}	
		}




		void nextGeneration(){
			bool[,] tempRoom = new bool[rows,cols];
			

			for (int i = 0; i < rows; i++){
				for (int j = 0; j < cols; j++){
					tempRoom[i,j] = room[i,j].IsOn;
				}
			}


			for (int i = 0; i < rows; i++){
				for (int j = 0; j < cols; j++){
					tempRoom[i,j] = B45678S45678(i,j);
				}
			}


			for (int i = 0; i < rows; i++){
				for (int j = 0; j < cols; j++){
					room[i,j].IsOn = tempRoom[i,j];
				}
			}
			
		}

/*

		bool B3S23(int x, int y){                             //conway's rules
			int neighbors = getMooreNeighborhood(x,y);
					if(neighbors < 2){
						return false;
					}else if(neighbors > 3){
						return false;
					}else if(room[x,y].IsOn == false && neighbors == 3){
						return true;
					}
				return false;
		}
*/

		bool B45678S45678(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
					if(neighbors < 4){
						return false;
					}else if(neighbors > 8){
						return false;
						Debug.Log("negighbors =");
					}else if(neighbors == 4 || neighbors == 5 || neighbors == 6 || neighbors == 7 || neighbors == 8){
						return true;
					}
				return true;
		}
/*

		bool B12S23(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
					if(neighbors < 2){
						return false;
					}else if(neighbors > 3){
						return false;
					}else if(room[x,y].IsOn == false && (neighbors == 1 || neighbors == 2)){
						return true;
					}
				return false;
		}


		bool B1S23(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
					if(neighbors < 2){
						return false;
					}else if(neighbors > 3){
						return false;
					}else if(room[x,y].IsOn == false && neighbors == 1){
						return true;
					}
				return false;
		}



		bool B4S34(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
					if(neighbors < 3){
						return false;
					}else if(neighbors > 3){
						return false;
					}else if(room[x,y].IsOn == false && neighbors == 4){
						return true;
					}
				return false;
		}



		*/
















		int getMooreNeighborhood(int x, int y){
				int sum = 0;
				for(int i = -1; i <= 1; i++){
					for(int j = -1; j <= 1; j++){
						//Debug.Log("tryingTo acsess"+ (x+i)+" "+(y+j));
						if(x+i>-1 && y+j>-1 && x+i < rows && y+j < cols){           ///this is real bad, need to fix
							if(room[x+i,y+j].IsOn){
								sum += 1;
							}
						}
					}
				}
				if (room[x,y].IsOn){
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
