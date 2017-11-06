//rooms can be made.









using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Assertions;



using UnityEngine;

public class WorldMaker : MonoBehaviour {
	public static float globalScaler = 2.1f;
	public static int fill = 32;




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

		public int XInRoom{
			get{return xLocationInRoom;}
		}

		public int YInRoom{
			get{return yLocationInRoom;}
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
			if(isDoor && isOn){
				myCube = Instantiate(Resources.Load("DoorCuber") as GameObject, new Vector3(absoluteXLocation*globalScaler,-20,absoluteyLocation*globalScaler), Quaternion.identity);
			}else if(isOuterWall && isOn){
				myCube = Instantiate(Resources.Load("OuterCuber") as GameObject, new Vector3(absoluteXLocation*globalScaler,-20,absoluteyLocation*globalScaler), Quaternion.identity);
			}else if(isOn){
				myCube = Instantiate(Resources.Load("Cuber") as GameObject, new Vector3(absoluteXLocation*globalScaler,-20,absoluteyLocation*globalScaler), Quaternion.identity);
			}
			if(isOn){
				Transform myCubeTransform = myCube.GetComponent<Transform>();
				myCubeTransform.localScale = new Vector3(globalScaler, 1, globalScaler);
			}

		}
	}

//--------------------------------------------------------------------









	class Room{
		
		
		
		GameObject gameManager;
       	GameManager gameManagerScript;
		public GameObject cube;
		GameObject groundplane;
		Transform groundplaneTransform;
		int xOffset;
		int yOffset;
		int xDimension;
		int yDimension;

		int northBounds;
		int eastBounds;
		int southBounds;
		int westBounds;
		Cell[,] room;
		List<Cell> northDoors;
		List<Cell> eastDoors;
		List<Cell> southDoors;
		List<Cell> westDoors;

		public Room(int xDimension, int yDimension, int xLocation, int yLocation){
			gameManager = GameObject.FindWithTag("GameController");
       		gameManagerScript = gameManager.GetComponent<GameManager>();
       		this.xOffset = xLocation;
       		this.yOffset = yLocation;
			this.xDimension = xDimension;
			this.yDimension = yDimension;
			northBounds = yDimension+yOffset -1;
			eastBounds = xDimension+xOffset -1;
			southBounds = yOffset;
			westBounds = xOffset;
			northDoors = new List<Cell>();
			eastDoors = new List<Cell>();
			southDoors = new List<Cell>();
			westDoors = new List<Cell>();



			room = new Cell[xDimension,yDimension];
			
			MakeOuterWalls();

			//FillRoom();
			
		}



		void MakeOuterWalls(){
			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j += (yDimension -1)){
					room[i,j]= new Cell(true,true,false,i,j,xOffset,yOffset); 
				}
			}
			for (int i = 0; i < xDimension; i+=(xDimension -1)){
				for (int j = 1; j < yDimension; j ++){
					room[i,j]= new Cell(true,true,false,i,j,xOffset,yOffset);   
				}
			}

		}

		public void Fill(){
			System.Random randomNumGenerator = new System.Random(gameManagerScript.Seed); 
			for (int i = 1; i < xDimension-1; i++){
				for (int j = 1; j < yDimension-1; j++){
					int ranNum = randomNumGenerator.Next(0,100);
					if(ranNum < fill){
						room[i,j]= new Cell(true,false,false,i,j,xOffset,yOffset);
					}else{
						room[i,j]= new Cell(false,false,false,i,j,xOffset,yOffset);
					}
				}
			}

			for(int i = 0; i < 5; i++){  
				nextGeneration();
			}
			
			

			//-------------------------------------------------------------test code delete
			int onRooms = 1;
			int offRooms = 0;

			for (int i = 1; i < xDimension-1; i++){    //so the sides are uneffected
				for (int j = 1; j < yDimension-1; j++){
					if(room[i,j].IsOn){
						onRooms++;
					}else{
						offRooms++;
					}

				}
			}
			Assert.IsTrue(onRooms < offRooms);
			//--------------------------------------------------------------------------------------------------------------

		}

		public void ClearDoors(){
			int cellsToClear = 12;
			foreach(Cell door in northDoors){
				for(int i = door.YInRoom-1; i > door.YInRoom- cellsToClear; i--){
					room[door.XInRoom,i].IsOn = false;
				}
			}
			foreach(Cell door in eastDoors){
				for(int i = door.XInRoom-1; i > door.XInRoom- cellsToClear; i--){
					room[i,door.YInRoom].IsOn = false;
				}

			}
			foreach(Cell door in southDoors){
				for(int i = door.YInRoom+1; i < door.YInRoom + cellsToClear; i++){
					room[door.XInRoom,i].IsOn = false;
				}
			}
			foreach(Cell door in westDoors){
				for(int i = door.XInRoom+1; i < door.XInRoom + cellsToClear; i++){
					room[i,door.YInRoom].IsOn = false;
				}
			}


		}




		public void Draw(){
			float xToDrawAt = (eastBounds + westBounds) / 2f;
			float yToDrawAt = (northBounds + southBounds) / 2f;
			groundplane = Instantiate(Resources.Load("GroundPlane2") as GameObject, new Vector3(xToDrawAt*globalScaler,-21,yToDrawAt*globalScaler), Quaternion.identity);   //xToDrawAt+.5f,-21,yToDrawAt+.5f
			Transform groundplaneTransform = groundplane.GetComponent<Transform>();
			//groundplaneTransform.localScale = new Vector3((xDimension/10)+.1f, 1, (yDimension/10)+.1f);
			groundplaneTransform.localScale = new Vector3(xDimension*globalScaler, 1, yDimension*globalScaler);

			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j++){		
					room[i,j].Draw();
				}
			}	
		}

		public void AddDoorAtAbsoluteLocation(int x, int y){
			if(y == northBounds){
				northDoors.Add(room[x - xOffset,y - yOffset]);
			}else if(x == eastBounds){
				eastDoors.Add(room[x - xOffset,y - yOffset]);
			}else if(y == southBounds){
				southDoors.Add(room[x - xOffset,y - yOffset]);
			}else if(x == westBounds){
				westDoors.Add(room[x - xOffset,y - yOffset]);
			}
			room[x - xOffset,y - yOffset].IsDoor = true;
			room[x - xOffset,y - yOffset].IsOn = false;
		}

		public void AddDoors(World world){

			int indexOfRoomBeingTracked = -1;
			int upperBoundsOfBorder = 0;
			int lowerBoundsOfBorder = 0;
			int borderingRoom = -1;
			int yToBuildAt = 0;


			for(int i = southBounds; i < northBounds; i++){  //checking east wall
				borderingRoom = world.IsInWorld(eastBounds+1,i);

				if(indexOfRoomBeingTracked == -1 && borderingRoom != -1){
					lowerBoundsOfBorder = i;
					indexOfRoomBeingTracked = borderingRoom;
				}	
				if(indexOfRoomBeingTracked != borderingRoom){   // the room being bordered changes
					upperBoundsOfBorder = i -1;
					yToBuildAt = (upperBoundsOfBorder + lowerBoundsOfBorder) / 2;
					if(upperBoundsOfBorder - lowerBoundsOfBorder > 4){
	 					AddDoorAtAbsoluteLocation(eastBounds, yToBuildAt);
	 					world.AddDoor(indexOfRoomBeingTracked, eastBounds+1, yToBuildAt);
	 				}
	 				indexOfRoomBeingTracked = borderingRoom;
	 				lowerBoundsOfBorder = i;
				}
			}
			if(indexOfRoomBeingTracked != -1){
				upperBoundsOfBorder = northBounds;
				yToBuildAt = (upperBoundsOfBorder + lowerBoundsOfBorder) / 2;
				if(upperBoundsOfBorder - lowerBoundsOfBorder > 4){
					AddDoorAtAbsoluteLocation(eastBounds, yToBuildAt);
	 				world.AddDoor(indexOfRoomBeingTracked, eastBounds+1, yToBuildAt);
	 			}
			}

			indexOfRoomBeingTracked = -1;
			upperBoundsOfBorder = 0;
			lowerBoundsOfBorder = 0;
			borderingRoom = -1;
			int xToBuildAt = 0;


			for(int i = westBounds; i < eastBounds; i++){  //checking north wall
				borderingRoom = world.IsInWorld(i,northBounds+1);
				Debug.Log("rawr");
				if(indexOfRoomBeingTracked == -1 && borderingRoom != -1){
					lowerBoundsOfBorder = i;
					indexOfRoomBeingTracked = borderingRoom;
				}	
				if(indexOfRoomBeingTracked != borderingRoom){   // the room being bordered changes
					upperBoundsOfBorder = i -1;
					xToBuildAt = (upperBoundsOfBorder + lowerBoundsOfBorder) / 2;
					if(upperBoundsOfBorder - lowerBoundsOfBorder > 4){
	 					AddDoorAtAbsoluteLocation(xToBuildAt, northBounds);
	 					world.AddDoor(indexOfRoomBeingTracked, xToBuildAt, northBounds+1);
	 				}
	 				indexOfRoomBeingTracked = borderingRoom;
	 				lowerBoundsOfBorder = i;
				}
			}
			if(indexOfRoomBeingTracked != -1){
				upperBoundsOfBorder = eastBounds;
				xToBuildAt = (upperBoundsOfBorder + lowerBoundsOfBorder) / 2;
				if(upperBoundsOfBorder - lowerBoundsOfBorder > 4){
					AddDoorAtAbsoluteLocation(xToBuildAt, northBounds);
	 				world.AddDoor(indexOfRoomBeingTracked, xToBuildAt, northBounds+1);
	 			}
			}



			
		}


		public bool IsInRoom(int x, int y){
			if(y <= northBounds && y >= southBounds && x >= westBounds && x <= eastBounds){
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


		void lastGeneration(){
			bool[,] tempRoom = new bool[xDimension,yDimension];
			

			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j++){

					tempRoom[i,j] = room[i,j].IsOn;
				}
			}


			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j++){
					tempRoom[i,j] = B678S678(i,j);
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


		bool B5678S5678(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors < 5){
				return false;
			}else{
				return true;
			}

		}

		bool B678S678(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors < 6){
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


			


			GameObject gameManager = GameObject.FindWithTag("GameController");
       		GameManager gameManagerScript = gameManager.GetComponent<GameManager>();	




			roomArray = new List<Room>();
			Room firstRoom = new Room(50,50,0,0);
			roomArray.Add(firstRoom);

			
			//for(int i = 0; i < 500; i++){
			while(roomArray.Count < 20){
				int sideToBuildOn = gameManagerScript.NextRandom(0,4);
				int whichRoomToBuldNextTo = gameManagerScript.NextRandom(0,roomArray.Count);
				int ranRoomSize = gameManagerScript.NextRandom(50,100);
       			Room[] tempArray = roomArray.ToArray();
       			int[] neighborRoomBounds = tempArray[whichRoomToBuldNextTo].NESWBounds;
       			int xLocation = 0;        //location for the lower left corner of the room
       			int yLocation = 0;
  				if(sideToBuildOn == 0){   //north
  					xLocation = neighborRoomBounds[3];
					yLocation = neighborRoomBounds[0] + 1;
					if(IsSafeToBuild(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation)){
						roomArray.Add(new Room(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation));
					}else{
						Debug.Log("room Collision sucsessfullyy detected");
					}		
				}else if(sideToBuildOn == 1){   //east
					xLocation = neighborRoomBounds[1] + 1;
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
			
			// for(int i = 0; i < roomArray.Count; i++){
			//  	Debug.Log("room " + i + " has a NESW bounds of " + roomArray[i].NESWBounds[0] + " " + roomArray[i].NESWBounds[1] + " " + roomArray[i].NESWBounds[2] + " " + roomArray[i].NESWBounds[3]);
			// }  

			AddDoors();
			Fill();
			ClearDoors();
			Draw();

		}



		void Fill(){
			



			// foreach(Room room in roomArray){
			// 	room.Fill();
			// }


			//------------------------------

			System.Threading.Tasks.Parallel.ForEach(roomArray, room =>
			{
				room.Fill();
			});



			//-----------------------------


		}

		void ClearDoors(){
			foreach(Room room in roomArray){
				room.ClearDoors();
			}
		}

		void Draw(){
			foreach(Room room in roomArray){
				room.Draw();
			}
		}






			
		void AddDoors(){
			foreach(Room room in roomArray){
				room.AddDoors(this);
			}
		}

		public void AddDoor(int RoomIndex, int x, int y){
			roomArray[RoomIndex].AddDoorAtAbsoluteLocation(x,y);
		}


		//if min length room to max length room is only 3 times as big then checking at like 3 points along easch wall can confirm no intersections
		bool IsSafeToBuild(int xDimension, int yDimension, int xLocation, int yLocation){  ///this should be 100% safe

			int northBounds = yDimension+yLocation-1;
			int eastBounds = xDimension+xLocation-1;
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
			if(IsInWorld(eastBounds, northBounds) != -1){//NE corner
				Debug.Log(1);
				return false;
			}
			if(IsInWorld(eastBounds, southBounds) != -1){//SE corner
				Debug.Log(2);
				return false;
			}
			if(IsInWorld(westBounds, southBounds) != -1){//SW corner
				Debug.Log(3);
				return false;
			}
			if(IsInWorld(westBounds, northBounds) != -1){ //NW corner
				Debug.Log(4);
				return false;
			}
			if(IsInWorld((eastBounds + westBounds)/2, northBounds) != -1){// N middle
				Debug.Log(5);
				return false;
			}
			if(IsInWorld(eastBounds, (northBounds + southBounds)/2) != -1){ // E middle
				//Debug.Log("point tested is " +eastBounds+" "+ (northBounds - southBounds));
				Debug.Log(6);
				return false;
			}
			if(IsInWorld((eastBounds + westBounds)/2, southBounds) != -1){ // S middle
				Debug.Log(7);
				return false;
			}
			if(IsInWorld(westBounds, (northBounds + southBounds)/2) != -1){ // W middle
				Debug.Log(8);
				return false;
			}
			return true;
		}
		


		public int IsInWorld(int x, int y){
			for(int i = 0; i < roomArray.Count; i++){
				if(roomArray[i].IsInRoom(x,y)){
					return i;
				}
			}
			return -1;
		}
	
	}






//---------------------------------------------------------------------------------------------






	// Use this for initialization
	void Start () {
		World myWorld = new World();
	}


}
