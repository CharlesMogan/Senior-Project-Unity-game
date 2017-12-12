//rooms can be made.









using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Assertions;



using UnityEngine;

public class WorldMaker : MonoBehaviour {
	public static readonly float globalScaler = 1f;
	public static readonly int fill = 32;
	public static readonly int globalElevation = 0;
	public static readonly int wallHeight = 12;
	public static readonly int minimumEdgeOverlapToBuildDoor = 7;
	public static readonly int roomsToBuild = 140;
	public static readonly int numberOfGenerations = 5;
	public static readonly int minEnemiesToSpawn = 10;
	public static readonly int maxEnemiesToSpawn = 11;
	public static readonly int minRoomSize = 55;
	public static readonly int maxRoomSize = 80;
	public static readonly int cellsToClear = 9;


	public class Cell{
		int xLocationInRoom;
		int yLocationInRoom;
		int absoluteXLocation;
		int absoluteyLocation;
		bool isOn;
		bool isOuterWall;
		bool isDoor;
		bool isTrigger = false;
		GameObject myCube;
		Room myRoom;
		public Cell(bool isOn, bool isOuterWall, bool isDoor, int xInRoom, int yInRoom, int xOffset, int yOffset, Room myRoom){
			this.isOn = isOn;
			this.isOuterWall = isOuterWall; 
			this.isDoor = isDoor;
			this.xLocationInRoom = xInRoom;
			this.yLocationInRoom = yInRoom;
			this.absoluteXLocation = xInRoom + xOffset;
			this.absoluteyLocation = yInRoom + yOffset;
			this.myRoom = myRoom;
		}

		public int XInRoom{
			get{return xLocationInRoom;}
		}

		public int YInRoom{
			get{return yLocationInRoom;}
		}

		public int AbsoluteXLocation{
			get{return absoluteXLocation;}
		}

		public int AbsoluteyLocation{
			get{return absoluteyLocation;}
		}

		public bool IsOn{
			get{return isOn;}

			set{isOn = value;}
		}
		
		public bool IsTrigger{
			get{return isTrigger;}

			set{isTrigger = value;}
		}

		public bool IsOuterWall{
			get{return isOuterWall;}

			set{isOuterWall = value;}
		}

		public bool IsDoor{
			get{return isDoor;}

			set{isDoor = value;}
		}





		public void Draw(){   //could be refactored by subclassing diffrent types of cells
			Destroy(myCube);
			if(isDoor && isOn){
				myCube = Instantiate(Resources.Load("DoorCuber") as GameObject, new Vector3(absoluteXLocation*globalScaler,globalElevation + (wallHeight / 2f)-.5f,absoluteyLocation*globalScaler), Quaternion.identity);
			}else if(isTrigger){
				myCube = Instantiate(Resources.Load("DoorTriggerCuber") as GameObject, new Vector3(absoluteXLocation*globalScaler,globalElevation + (wallHeight / 2f)-.5f,absoluteyLocation*globalScaler), Quaternion.identity);
				DownDoor downDoorScript = myCube.GetComponent<DownDoor>();
				downDoorScript.SendMessage("WhoIsMyRoom",myRoom,SendMessageOptions.RequireReceiver);
			}else if(isOuterWall){
				myCube = Instantiate(Resources.Load("OuterCuber") as GameObject, new Vector3(absoluteXLocation*globalScaler,globalElevation + (wallHeight / 2f)-.5f,absoluteyLocation*globalScaler), Quaternion.identity);
			}else if(isOn){
				myCube = Instantiate(Resources.Load("Cuber") as GameObject, new Vector3(absoluteXLocation*globalScaler,globalElevation + (wallHeight / 2f)-.5f,absoluteyLocation*globalScaler), Quaternion.identity);
			}
			if(isOn || isTrigger){
				Transform myCubeTransform = myCube.GetComponent<Transform>();
				myCubeTransform.localScale = new Vector3(globalScaler, wallHeight, globalScaler);
			}

		}		
	}

//--------------------------------------------------------------------









	public class Room{
		
		
		World myWorld;
		GameObject gameManager;
       	GameManager gameManagerScript;
		public GameObject cube;
		GameObject groundplane;
		Transform groundplaneTransform;
		int xOffset;
		int yOffset;
		int xDimension;
		int yDimension;
		int roomSeed;

		int northBounds;
		int eastBounds;
		int southBounds;
		int westBounds;
		Cell[,] room;
		List<Cell> northDoors;
		List<Cell> eastDoors;
		List<Cell> southDoors;
		List<Cell> westDoors;
		bool isCleared;
		bool inCombat;
		bool bossRoom;

		public Room(int xDimension, int yDimension, int xLocation, int yLocation, World myWorld, int roomSeed, bool bossRoom){
			gameManager = GameObject.FindWithTag("GameController");
       		gameManagerScript = gameManager.GetComponent<GameManager>();
       		this.xOffset = xLocation;
       		this.yOffset = yLocation;
			this.xDimension = xDimension;
			this.yDimension = yDimension;
			this.myWorld = myWorld;
			this.roomSeed = roomSeed;
			northBounds = yDimension+yOffset -1;
			eastBounds = xDimension+xOffset -1;
			southBounds = yOffset;
			westBounds = xOffset;
			northDoors = new List<Cell>();
			eastDoors = new List<Cell>();
			southDoors = new List<Cell>();
			westDoors = new List<Cell>();
			isCleared = false;
			inCombat = false;
			this.bossRoom = bossRoom;
			



			room = new Cell[xDimension,yDimension];
			
			MakeOuterWalls();
			
		}

		public void EnteredRoom(){
			if(!isCleared && !inCombat){
				inCombat = true;
				myWorld.LockDoorsInAllRoomsBesides(this);
				gameManagerScript.EnteredCombatInRoom(this);
				if(!bossRoom){
					SpawnEnemies();
				}else{
					SpawnBoss();
				}
			}
		}

		public bool IsMooreNeighborhoodCleared(int x, int y){
			return(getMooreNeighborhood(x,y) == 0 && room[x,y].IsOn == false);
		}

		

		public void SpawnBoss(){
			
			int xPlacement = 0;
			int yPlacement = 0;
			while(!IsMooreNeighborhoodCleared(xPlacement,yPlacement)){
				xPlacement = gameManagerScript.NextRandom(1,xDimension);
				yPlacement = gameManagerScript.NextRandom(1,yDimension);
			}
			GameObject enemyToSpawn = Resources.Load("Boss") as GameObject;
			GameObject enemy = Instantiate(enemyToSpawn, new Vector3(room[xPlacement,yPlacement].AbsoluteXLocation*globalScaler,globalElevation-.5f,room[xPlacement,yPlacement].AbsoluteyLocation*globalScaler), Quaternion.identity);
			Health enemyHealthScript = enemy.GetComponent<Health>();
			enemyHealthScript.SendMessage("WhoIsMyRoom",this,SendMessageOptions.RequireReceiver);
			for(int i = -1; i <= 1; i++){
				for(int j = -1; j <= 1; j++){
					room[xPlacement,yPlacement].IsOn = true;
				}
			}


		}

		public void SpawnEnemies(){
			int livingEnemies = gameManagerScript.NextRandom(minEnemiesToSpawn,maxEnemiesToSpawn);
			for(int i = 0;i < livingEnemies; i++){
				int xPlacement = 0;
				int yPlacement = 0;
				while(room[xPlacement,yPlacement].IsOn){
					xPlacement = gameManagerScript.NextRandom(1,xDimension);
					yPlacement = gameManagerScript.NextRandom(1,yDimension);
				}


				int randomNum = gameManagerScript.NextRandom(1,7);
				GameObject enemyToSpawn;
				if(randomNum == 1){
					enemyToSpawn = Resources.Load("OctalTurret") as GameObject;
				}else if(randomNum == 2){
					enemyToSpawn = Resources.Load("FollowEnemy") as GameObject;
				}else if(randomNum == 3){
					enemyToSpawn = Resources.Load("ZigZagEnemy") as GameObject;
				}else if(randomNum == 4){
					enemyToSpawn = Resources.Load("FollowEnemy") as GameObject;
				}else if(randomNum == 5){
					enemyToSpawn = Resources.Load("FollowEnemyDoubleShot") as GameObject;
				}else{
					enemyToSpawn = Resources.Load("FollowEnemyTripleSpreadShot") as GameObject;
				}
				
				
				room[xPlacement,yPlacement].IsOn = true;
				GameObject enemy = Instantiate(enemyToSpawn, new Vector3(room[xPlacement,yPlacement].AbsoluteXLocation*globalScaler,globalElevation-.5f,room[xPlacement,yPlacement].AbsoluteyLocation*globalScaler), Quaternion.identity);
			}


		}

		public void SpawnChasers(int chasersToSpawn){
			SpawnEnemy(chasersToSpawn,"FollowEnemy");
		}


		public void SpawnTurrets(int turretsToSpawn){
			SpawnEnemy(turretsToSpawn,"OctalTurret");
		}


		void SpawnEnemy(int numberToSpawn, string enemyToSpawn){
			int xPlacement = 0;
			int yPlacement = 0;
			for(int i = 0; i <= numberToSpawn; i++){
				while(room[xPlacement,yPlacement].IsOn){
					xPlacement = gameManagerScript.NextRandom(1,xDimension);
					yPlacement = gameManagerScript.NextRandom(1,yDimension);
				}
				room[xPlacement,yPlacement].IsOn = true;
				GameObject enemy = Instantiate(Resources.Load(enemyToSpawn) as GameObject, new Vector3(room[xPlacement,yPlacement].AbsoluteXLocation*globalScaler,globalElevation-.5f,room[xPlacement,yPlacement].AbsoluteyLocation*globalScaler), Quaternion.identity);
				EnemyHealth enemyHealthScript = enemy.GetComponent<EnemyHealth>();
				enemyHealthScript.SendMessage("WhoIsMyRoom",this,SendMessageOptions.RequireReceiver);
			}
		}

		public bool IsCleared{
			get{return isCleared;}

			set{isCleared = value;}
		}


		// public void EnemyDied(Vector3 deathLocation){
		// 	livingEnemies--;
		// 	Debug.Log("the number of living enemies is " + livingEnemies);

		// 	if(livingEnemies == 0){
		// 		EndCombat(deathLocation);
		// 	}
		// }


		public void EndCombat(Vector3 locationToSpawnPickup){
			IsCleared = true;
			inCombat = false;
			int randomNum = gameManagerScript.NextRandom(1,8);
			if(randomNum == 1){
			Debug.Log("youpicked up a " + "fire rate +");
			Instantiate(Resources.Load("bulletFireRateUpPickup") as GameObject, locationToSpawnPickup, Quaternion.identity);
			}else if(randomNum == 2){
				Debug.Log("youpicked up a " + "bullet lifetime +");
				Instantiate(Resources.Load("BulletLifetimeUpPickup") as GameObject, locationToSpawnPickup, Quaternion.identity);
			}else if(randomNum == 3){
				Debug.Log("youpicked up a " + "bullet speed +");
				Instantiate(Resources.Load("BulletSpeedUpPickup") as GameObject, locationToSpawnPickup, Quaternion.identity);
			}else if(randomNum == 4){
				Debug.Log("youpicked up a " + "charge delay -");
				Instantiate(Resources.Load("LaserChargeDelayDownPickup") as GameObject, locationToSpawnPickup, Quaternion.identity);
			}else if(randomNum == 5){
				Debug.Log("youpicked up a " + "laserDamage +");
				Instantiate(Resources.Load("LaserDamageUpPickup") as GameObject, locationToSpawnPickup, Quaternion.identity);
			}else if(randomNum == 6){
				Debug.Log("youpicked up a " + "maxHealthUp +");
				Instantiate(Resources.Load("MaxHealthUpPickup") as GameObject, locationToSpawnPickup, Quaternion.identity);
			}else if(randomNum == 7){
				Debug.Log("youpicked up a " + "speed +");
				Instantiate(Resources.Load("SpeedUpPickup") as GameObject, locationToSpawnPickup, Quaternion.identity);
			
			}else{Assert.IsTrue(false);}
			myWorld.UnlockAllDoors();
		}


		public void LockDoors(){
			foreach(Cell door in northDoors){
				door.IsOn = true;
				door.Draw();
			}
			foreach(Cell door in eastDoors){
				door.IsOn = true;
				door.Draw();
			}
			foreach(Cell door in southDoors){
				door.IsOn = true;
				door.Draw();
			}
			foreach(Cell door in westDoors){
				door.IsOn = true;
				door.Draw();
			}
		}

		public void UnlockDoors(){
			foreach(Cell door in northDoors){
				door.IsOn = false;
				door.Draw();
			}
			foreach(Cell door in eastDoors){
				door.IsOn = false;
				door.Draw();
			}
			foreach(Cell door in southDoors){
				door.IsOn = false;
				door.Draw();
			}
			foreach(Cell door in westDoors){
				door.IsOn = false;
				door.Draw();
			}
		}

		void MakeOuterWalls(){
			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j += (yDimension -1)){
					room[i,j]= new Cell(true,true,false,i,j,xOffset,yOffset,this); 
				}
			}
			for (int i = 0; i < xDimension; i+=(xDimension -1)){
				for (int j = 1; j < yDimension; j ++){
					room[i,j]= new Cell(true,true,false,i,j,xOffset,yOffset,this);   
				}
			}

		}

		public void Fill(){
			System.Random randomNumGenerator = new System.Random(gameManagerScript.Seed + roomSeed); 
			for (int i = 1; i < xDimension-1; i++){
				for (int j = 1; j < yDimension-1; j++){
					int ranNum = randomNumGenerator.Next(0,100);
					if(ranNum < fill){
						room[i,j]= new Cell(true,false,false,i,j,xOffset,yOffset,this);
					}else{
						room[i,j]= new Cell(false,false,false,i,j,xOffset,yOffset,this);
					}
				}
			}

			for(int i = 0; i < numberOfGenerations; i++){  
				NextGeneration();
			}
			LastGeneration();
			

			
		}

		public void ClearDoors(){
			
			bool inGap = false;
			foreach(Cell door in northDoors){
				room[door.XInRoom,door.YInRoom-1].IsTrigger = true;
				int i = door.YInRoom-1;
				while(i > door.YInRoom - cellsToClear || !inGap){
				//for(int i = door.YInRoom-1; i > door.YInRoom - cellsToClear; i--){
					if(room[door.XInRoom,i].IsOn){
						room[door.XInRoom,i].IsOn = false;
						inGap = false;
					}else{
						inGap = true;
					}
					i--;
				}
				inGap = false;
			}



			foreach(Cell door in eastDoors){
				room[door.XInRoom-1,door.YInRoom].IsTrigger = true;
				int i = door.XInRoom-1;
				while( i > door.XInRoom - cellsToClear || !inGap){
				//for(int i = door.XInRoom-1; i > door.XInRoom - cellsToClear; i--){
					if(room[i,door.YInRoom].IsOn){
						room[i,door.YInRoom].IsOn = false;
						inGap = false;
					}else{
						inGap = true;
					}
					i--;
				}
				inGap = false;
			}
			foreach(Cell door in southDoors){
				room[door.XInRoom,door.YInRoom+1].IsTrigger = true;
				int i = door.YInRoom+1;
				while( i < door.YInRoom + cellsToClear || !inGap){
				//for(int i = door.YInRoom+1; i < door.YInRoom + cellsToClear; i++){
					if(room[door.XInRoom,i].IsOn){
						room[door.XInRoom,i].IsOn = false;
						inGap = false;
					}else{
						inGap = true;
					}
					i++;
				}
				inGap = false;
			}

			foreach(Cell door in westDoors){
				room[door.XInRoom+1,door.YInRoom].IsTrigger = true;
				int i = door.XInRoom+1;
				while( i < door.XInRoom + cellsToClear || !inGap){
				//for(int i = door.XInRoom+1; i < door.XInRoom + cellsToClear; i++){
					if(room[i,door.YInRoom].IsOn){
						room[i,door.YInRoom].IsOn = false;
						inGap = false;
					}else{
						inGap = true;
					}
					i++;
				}
				inGap = false;
			}


		}




		public void Draw(){
			float xToDrawAt = (eastBounds + westBounds) / 2f;
			float yToDrawAt = (northBounds + southBounds) / 2f;
			groundplane = Instantiate(Resources.Load("GroundPlane2") as GameObject, new Vector3(xToDrawAt*globalScaler,globalElevation-1,yToDrawAt*globalScaler), Quaternion.identity);   //xToDrawAt+.5f,-21,yToDrawAt+.5f
			Transform groundplaneTransform = groundplane.GetComponent<Transform>();
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
			try{
				room[x - xOffset,y - yOffset].IsDoor = true;       //has a small chance of crashing? race conditions?
				room[x - xOffset,y - yOffset].IsOn = false;
			} catch(System.NullReferenceException e){
                Assert.IsTrue(false);
            	Debug.Log("bad shit going down at" + (x - xOffset) +" "+ (y - yOffset) );
        	}
		}

		public void AddDoors(World world){      // adds doors to the north and east walls of the room, and tells the south and west walls of adjacent rooms where to build.

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
					if(upperBoundsOfBorder - lowerBoundsOfBorder > minimumEdgeOverlapToBuildDoor){
						Debug.Log("Door crasher 1");
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
				if(upperBoundsOfBorder - lowerBoundsOfBorder > minimumEdgeOverlapToBuildDoor){
					Debug.Log("Door crasher 2");
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
				if(indexOfRoomBeingTracked == -1 && borderingRoom != -1){
					lowerBoundsOfBorder = i;
					indexOfRoomBeingTracked = borderingRoom;
				}	
				if(indexOfRoomBeingTracked != borderingRoom){   // the room being bordered changes
					upperBoundsOfBorder = i -1;
					xToBuildAt = (upperBoundsOfBorder + lowerBoundsOfBorder) / 2;
					if(upperBoundsOfBorder - lowerBoundsOfBorder > 4){
						Debug.Log("Door crasher 3");
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
					Debug.Log("Door crasher 4");
					Debug.Log("build at"+ xToBuildAt+" " + (northBounds+1));
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


		void NextGeneration(){
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


		void LastGeneration(){
			bool[,] tempRoom = new bool[xDimension,yDimension];
			

			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j++){

					tempRoom[i,j] = room[i,j].IsOn;
				}
			}


			for (int i = 0; i < xDimension; i++){
				for (int j = 0; j < yDimension; j++){
					tempRoom[i,j] = B1S12345678(i,j);
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



		bool B1S12345678(int x, int y){                             //conway's rules
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors == 1){
				return true;
			}
			return room[x,y].IsOn;
		}



		bool B345678S345678(int x, int y){   
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors < 3){
				return false;
			}else{
				return true;
			}

		}

		bool B35678S35678(int x, int y){   
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors < 3 || neighbors == 4){
				return false;
			}else{
				return true;
			}

		}

		bool B45678S45678(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors < 4){
				return false;
			}else{
				return true;
			}

		}

		bool B4678S4678(int x, int y){					//this ones no good
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors < 4 || neighbors == 5){
				return false;
			}else{
				return true;
			}

		}

		bool B4578S4578(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors < 4 || neighbors == 6){
				return false;
			}else{
				return true;
			}

		}

		bool B4568S4568(int x, int y){
			int neighbors = getMooreNeighborhood(x,y);
			if(neighbors < 4 || neighbors == 7){
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
			get{return new int[4]{northBounds,eastBounds,southBounds,westBounds};}
		}
		


		int getMooreNeighborhood(int x, int y){
			int sum = 0;
			for(int i = -1; i <= 1; i++){
				for(int j = -1; j <= 1; j++){
					if(x+i>-1 && y+j>-1 && x+i < xDimension && y+j < yDimension){
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






	public class World{
		List<Room> roomArray;

		public World(){


			


			GameObject gameManager = GameObject.FindWithTag("GameController");
       		GameManager gameManagerScript = gameManager.GetComponent<GameManager>();	




			roomArray = new List<Room>();
			Room firstRoom = new Room(50,50,0,0,this, 0, false);
			firstRoom.IsCleared = true;
			roomArray.Add(firstRoom);

			
			//for(int i = 0; i < 500; i++){
			while(roomArray.Count < roomsToBuild){
				int sideToBuildOn = gameManagerScript.NextRandom(0,4);
				int whichRoomToBuldNextTo = gameManagerScript.NextRandom(0,roomArray.Count);
				int ranRoomSize = gameManagerScript.NextRandom(minRoomSize,maxRoomSize);
       			Room[] tempArray = roomArray.ToArray();
       			int[] neighborRoomBounds = tempArray[whichRoomToBuldNextTo].NESWBounds;
       			int xLocation = 0;        //location for the lower left corner of the room
       			int yLocation = 0;
  				if(sideToBuildOn == 0){   //north
  					xLocation = neighborRoomBounds[3];
					yLocation = neighborRoomBounds[0] + 1;
					if(IsSafeToBuildV2(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation)){
						if(roomArray.Count == roomsToBuild -1){
							roomArray.Add(new Room(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation,this,roomArray.Count, true));
						}else{
							roomArray.Add(new Room(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation,this,roomArray.Count, false));
							Debug.Log("bossRoomMade");
						}
					}else{
						Debug.Log("room Collision sucsessfullyy detected");
					}		
				}else if(sideToBuildOn == 1){  //east
					xLocation = neighborRoomBounds[1] + 1;
					yLocation = neighborRoomBounds[2];
					if(IsSafeToBuildV2(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation)){
						if(roomArray.Count == roomsToBuild -1){
							roomArray.Add(new Room(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation,this,roomArray.Count, true));
						}else{
							roomArray.Add(new Room(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation,this,roomArray.Count, false));
						}
					}else{
						Debug.Log("room Collision sucsessfullyy detected");
					}
				}else if(sideToBuildOn == 2){   // south
					xLocation = neighborRoomBounds[3];
					yLocation =  neighborRoomBounds[2]-ranRoomSize;
					
					if(IsSafeToBuildV2(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation)){
						if(roomArray.Count == roomsToBuild -1){
							roomArray.Add(new Room(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation,this,roomArray.Count, true));
						}else{
							roomArray.Add(new Room(roomArray[whichRoomToBuldNextTo].Width,ranRoomSize,xLocation,yLocation,this,roomArray.Count, false));
						}
					}else{
						Debug.Log("room Collision sucsessfullyy detected");
					}
				}else if(sideToBuildOn == 3){   //west
					xLocation = neighborRoomBounds[3] - ranRoomSize;
					yLocation = neighborRoomBounds[2];
					
					if(IsSafeToBuildV2(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation)){
						if(roomArray.Count == roomsToBuild -1){
							roomArray.Add(new Room(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation,this,roomArray.Count, true));
						}else{
							roomArray.Add(new Room(ranRoomSize,roomArray[whichRoomToBuldNextTo].Height,xLocation,yLocation,this,roomArray.Count, false));
						}
					}else{
						Debug.Log("room Collision sucsessfullyy detected");
					}
				}else{Assert.IsTrue(false);}

			}

		
			// for(int i = 0; i < roomArray.Count; i++){
			//  	Debug.Log("room " + i + " has a NESW bounds of " + roomArray[i].NESWBounds[0] + " " + roomArray[i].NESWBounds[1] + " " + roomArray[i].NESWBounds[2] + " " + roomArray[i].NESWBounds[3]);
			// }  

			AddDoors();
			Fill();
			ClearDoors();
			Draw();

		}

		public void UnlockAllDoors(){
			foreach(Room room in roomArray){
				room.UnlockDoors();

			}
		}

		public void LockDoorsInAllRoomsBesides(Room enteredRoom){
			foreach(Room room in roomArray){
				if(room != enteredRoom){
					room.LockDoors();
				}

			}
		}


		void Fill(){
			



			// foreach(Room room in roomArray){
			// 	room.Fill();
			// }


			//------------------------------

			Parallel.ForEach(roomArray, room =>
			{
				room.Fill();
			});



			//-----------------------------


		}

		void ClearDoors(){
			// foreach(Room room in roomArray){
			// 	room.ClearDoors();
			// }

			Parallel.ForEach(roomArray, room =>
			{
				room.ClearDoors();
			});
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
			Debug.Log("Door crasher 5");
			roomArray[RoomIndex].AddDoorAtAbsoluteLocation(x,y);
		}

		bool IsSafeToBuild(int xDimension, int yDimension, int xLocation, int yLocation){  ///this should be 100% safe for rooms where min size > 1/2x maxsize
			Assert.IsTrue(xDimension*2 >= yDimension);
			Assert.IsTrue(yDimension*2 >= xDimension);
			int northBounds = yDimension+yLocation-1;
			int eastBounds = xDimension+xLocation-1;
			int southBounds = yLocation;
			int westBounds = xLocation;

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



		bool IsSafeToBuildV2(int xDimension, int yDimension, int xLocation, int yLocation){  ///this should be 100% safe
			//Assert.IsTrue(xDimension*2 >= yDimension);
			//Assert.IsTrue(yDimension*2 >= xDimension);
			int northBounds = yDimension+yLocation-1;
			int eastBounds = xDimension+xLocation-1;
			int southBounds = yLocation;
			int westBounds = xLocation;
			
            


            foreach (Room room in roomArray){
				int[] roombounds = room.NESWBounds;
				if(
                ((eastBounds <= roombounds[1] && eastBounds >= roombounds[3]) ||  (westBounds <= roombounds[1] && westBounds >= roombounds[3]) || (eastBounds >= roombounds[1] && westBounds <= roombounds[3]))
				&&((northBounds <= roombounds[0] && northBounds >= roombounds[2]) || (southBounds <= roombounds[0] && southBounds >= roombounds[2]) || (northBounds >= roombounds[0] && southBounds <= roombounds[2]))
                ){
					return false;
				}
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
	// void Start () {
	// 	World myWorld = new World();
	// }


}
