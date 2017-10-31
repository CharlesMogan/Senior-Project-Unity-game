//rooms can be made.









using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMaker : MonoBehaviour {
	




	class Cell{
		int xLocationInRoom;
		int yLocationInRoom;
		int absoluteXLocation;
		int absoluteyLocation;
		bool isOn;
		bool isOuterWall;
		bool isDoor;
		GameObject myCube;
		public Cell(bool isOn, bool isOuterWall, bool isDoor, int xInRoom, int yInRoom, int xOffset, int yOffset){
			this.isOn = isOn;
			this.isOuterWall = isOuterWall; 
			this.isDoor = isDoor;
			this.xLocationInRoom = xInRoom;
			this.yLocationInRoom = yInRoom;
			this.absoluteXLocation = xInRoom + xOffset;
			this.absoluteyLocation = yInRoom + yOffset;
			// if it is not on the outer wall I need to add health at somepoint.
		}

		public bool IsOn{
			get{return isOn;}

			set{isOn = value;}
		}

		public bool IsOuterWall{
			get{return isOuterWall;}

			set{isOuterWall = value;}
		}

		public bool IsDoor{
			get{return isDoor;}

			set{isDoor = value;}
		}



		public void Draw(){
			Destroy(myCube);
			if(isDoor){
				Debug.Log("implement me!");
			}else if(isOuterWall){
				myCube = Instantiate(Resources.Load("OuterCuber") as GameObject, new Vector3(absoluteXLocation,-20,absoluteyLocation), Quaternion.identity);
			}else if(isOn){
				myCube = Instantiate(Resources.Load("Cuber") as GameObject, new Vector3(absoluteXLocation,-20,absoluteyLocation), Quaternion.identity);
			}
		}
	}

//--------------------------------------------------------------------









	class Room{
		int fill = 32;

		GameObject gameManager;
       	GameManager gameManagerScript;
		public GameObject cube;
		int xOffset;
		int yOffset;
		int xDimension;
		int yDimension;

		int northBounds;
		int eastBounds;
		int southBounds;
		int westBounds;
		Cell[,] room;

		public Room(int xDimension, int yDimension, int xLocation, int yLocation){
			gameManager = GameObject.FindWithTag("GameController");
       		gameManagerScript = gameManager.GetComponent<GameManager>();
       		this.xOffset = xLocation;
       		this.yOffset = yLocation;
			this.xDimension = xDimension;
			this.yDimension = yDimension;
			northBounds = yDimension+yOffset;
			eastBounds = xDimension+xOffset;
			southBounds = yOffset;
			westBounds = xOffset;

			room = new Cell[xDimension,yDimension];
			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j++){
					int ranNum = gameManagerScript.NextRandom(0,100);
					if( i == 0 || j == 0 || i == xDimension-1 || j == yDimension-1){
						room[i,j]= new Cell(true,true,false,i,j,xOffset,yOffset);
					}else if(ranNum < fill){
						room[i,j]= new Cell(true,false,false,i,j,xOffset,yOffset);
					}else{
						room[i,j]= new Cell(false,false,false,i,j,xOffset,yOffset);
					}
				}
			}

			for(int i = 0; i < 5; i++){  //i do ever even
				nextGeneration();
			}
			Draw();	
		}





		void Draw(){
			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j++){
					room[i,j].Draw();
				}
			}	
		}



		public bool IsInRoom(int x, int y){
			if(y < northBounds && y > southBounds && x > westBounds && x <eastBounds){
				return true;
			}
			return false;
		}


		void nextGeneration(){
			bool[,] tempRoom = new bool[xDimension,yDimension];
			

			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j++){
					tempRoom[i,j] = room[i,j].IsOn;
				}
			}


			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j++){
					tempRoom[i,j] = B45678S45678(i,j);
				}
			}


			for (int i = 1; i < xDimension-1; i++){    //so the sides are uneffected
				for (int j = 1; j < yDimension-1; j++){
					room[i,j].IsOn = tempRoom[i,j];
				}
			}
			
		}



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

		bool B45678S45678(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors < 4){
				return false;
			}else{
				return true;
			}

		}


		public int Width{
			get{return xDimension;}
		}

		public int Height{
			get{return yDimension;}
		}


		public int[] NESWBounds{
			get{return new int[4]{northBounds,eastBounds,southBounds,westBounds};}///////////////// might be backwoards
		}
		



		int getMooreNeighborhood(int x, int y){
			int sum = 0;
			for(int i = -1; i <= 1; i++){
				for(int j = -1; j <= 1; j++){
					//Debug.Log("tryingTo acsess"+ (x+i)+" "+(y+j));
					if(x+i>-1 && y+j>-1 && x+i < xDimension && y+j < yDimension){           ///this is real bad, need to fix
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
		}////////////----------














//---------------------------------------------------------------------------------------------------------






	class World{
		List<Room> roomArray;

		public World(){


			//make a room 
			//make a touching room off of one of the sides
			//propose a third room at a location
			//check if that conflicts with other rooms.




			GameObject gameManager = GameObject.FindWithTag("GameController");
       		GameManager gameManagerScript = gameManager.GetComponent<GameManager>();
       		int ranRoomSize1 = gameManagerScript.NextRandom(40,125);
       		int ranRoomSize2 = gameManagerScript.NextRandom(40,75);
			//int ranRoomSize3 = gameManagerScript.NextRandom(60,125);
       		//int ranRoomSize4 = gameManagerScript.NextRandom(40,125);
       			


			/*Room firstRoom = new Room(250,250,0,0);
			Room secondRoom = new Room(250,250,250,0);
			Room thirdRoom = new Room(250,250,0,250);
			Room fourthRoom = new Room(250,250,250,250);*/
			//firstRoom.getDoors();
		

			//first room
			//Room firstRoom = new Room(250,25,0,0);
			//second room
			//Room firstRoom = new Room(ranRoomSize1,ranRoomSize2,0,0);

			//int nextSide = gameManagerScript.NextRandom(0,4);

			/*Room secondRoom;												// switch statement?----------------------
			if(nextSide == 0){
				secondRoom = new Room(ranRoomSize3,ranRoomSize4,0,ranRoomSize2);
			}else if(nextSide == 1){
				secondRoom = new Room(ranRoomSize3,ranRoomSize4,ranRoomSize1,0);
			}else if(nextSide == 2){
				secondRoom = new Room(ranRoomSize3,ranRoomSize4,0,-ranRoomSize4);
			}else {
				Debug.Assert(nextSide == 3,"random number bounds error");
				secondRoom = new Room(ranRoomSize3,ranRoomSize4,-ranRoomSize3,0);
			}
*/

			roomArray = new List<Room>();
			Room firstRoom = new Room(50,50,0,0);
			roomArray.Add(firstRoom);


			while(roomArray.Count < 6){
				int sideToBuildOn = gameManagerScript.NextRandom(0,4);
				int whichRoomToBuldNextTo = gameManagerScript.NextRandom(0,roomArray.Count);
       			int ranRoomSize = gameManagerScript.NextRandom(50,100);
       			Room[] tempArray = roomArray.ToArray();
       			int[] neighborRoomBounds = tempArray[whichRoomToBuldNextTo].NESWBounds;
       			int xLocation = 0;        //location for the lower left corner of the room
       			int yLocation = 0;
  				if(sideToBuildOn == 0){   //north
  					xLocation = neighborRoomBounds[3];
					yLocation = neighborRoomBounds[0];
					if(IsSafeToBuild(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation)){
						roomArray.Add(new Room(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation));
					}else{
						Debug.Log("room Collision sucsessfullyy detected");
					}		
				}else if(sideToBuildOn == 1){   //east
					xLocation = neighborRoomBounds[1];
					yLocation = neighborRoomBounds[2];
					if(IsSafeToBuild(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation)){
						roomArray.Add(new Room(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation));
					}else{
						Debug.Log("room Collision sucsessfullyy detected");
					}
				}else if(sideToBuildOn == 2){   // south
					xLocation = neighborRoomBounds[3];
					yLocation =  neighborRoomBounds[2]-ranRoomSize;
					
					if(IsSafeToBuild(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation)){
						roomArray.Add(new Room(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation));
					}else{
						Debug.Log("room Collision sucsessfullyy detected");
					}
				}else if(sideToBuildOn == 3){   //west
					xLocation = neighborRoomBounds[3] - ranRoomSize;
					yLocation = neighborRoomBounds[2];
					
					if(IsSafeToBuild(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation)){
						roomArray.Add(new Room(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation));
					}else{
						Debug.Log("room Collision sucsessfullyy detected");
					}
				}else{Debug.Log("something went teribad wrong in room generation");}

			}
				//maybe havve avalable side list atached to rooms
				//pick a side
				//pick a cube 
				// check if works
				// if works add


		}

			


			//if min length room to max length room is only 3 times as big then checking at like 3 points along easch wall can confirm no intersections
			bool IsSafeToBuild(int xDimension, int yDimension, int xLocation, int yLocation){  ///this should be 100% safe

				int northBounds = yDimension+yLocation;
				int eastBounds = xDimension+xLocation;
				int southBounds = yLocation;
				int westBounds = xLocation;
				// Debug.Log(northBounds + " is northBounds");
				// Debug.Log(eastBounds + " is eastBounds");
				// Debug.Log(southBounds + " issouthhBounds");
				// Debug.Log(westBounds + " iswesthBounds");

/*				if(IsInWorld(eastBounds, northBounds) ||  //NE corner
					IsInWorld(eastBounds, southBounds) || //SE corner
					IsInWorld(westBounds, southBounds) || //SW corner
					IsInWorld(westBounds, northBounds)	|| //NW corner
					IsInWorld((eastBounds + westBounds)/2, northBounds) || // N middle
					IsInWorld(eastBounds, (northBounds + southBounds)/2)) || // E middle
					IsInWorld((eastBounds + westBounds)/2, southBounds) || // S middle
					IsInWorld(westBounds,(northBounds + southBounds)/2))){ // W middle
					return false;
				}*/
				if(IsInWorld(eastBounds, northBounds)){//NE corner
					Debug.Log(1);
					return false;
				}
				if(IsInWorld(westBounds, southBounds)){//SE corner
					Debug.Log("point tested is " +westBounds+" "+ southBounds);
					Debug.Log(2);
					return false;
				}
				if(IsInWorld(westBounds, southBounds)){//SW corner
					Debug.Log(3);
					return false;
				}
				if(IsInWorld(westBounds, northBounds)){ //NW corner
					Debug.Log(4);
					return false;
				}
				if(IsInWorld((eastBounds + westBounds)/2, northBounds)){// N middle
					Debug.Log(5);
					return false;
				}
				if(IsInWorld(eastBounds, (northBounds + southBounds)/2)){ // E middle
					//Debug.Log("point tested is " +eastBounds+" "+ (northBounds - southBounds));
					Debug.Log(6);
					return false;
				}
				if(IsInWorld((eastBounds + westBounds)/2, southBounds)){ // S middle
					Debug.Log(7);
					return false;
				}
				if(IsInWorld(westBounds, (northBounds + southBounds)/2)){ // W middle
					Debug.Log(8);
					return false;
				}
				return true;
			}
			


			bool IsInWorld(int x, int y){
				foreach(Room room in roomArray){
					if(room.IsInRoom(x,y)){
						return true;
					}
				}
				return false;
			}
	
	}






//---------------------------------------------------------------------------------------------






	// Use this for initialization
	void Start () {
		World myWorld = new World();

	}


}
