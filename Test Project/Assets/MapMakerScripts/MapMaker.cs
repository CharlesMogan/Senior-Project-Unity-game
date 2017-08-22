using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//https://www.youtube.com/watch?v=v7yyZZjF1z4&t=231s
// not doing smothing passes on the same map like in the tatorial as that causes jankyness.



public class MapMaker : MonoBehaviour {




	[Range(0,100)]
	public int fillPercent;
	public int width;
	public int height;
	public string seed;
	public bool randomSeed;
	public int smoothingPasses;
	public float squareSize;
	[Range(0,25)]
	public int borderSize;

	[Range(0,250)]
	public int wallThresholdSize;

	[Range(0,250)]
	public int roomThresholdSize;
	


	private int [,] map;


	void Start(){
		MakeMap();
	}


	void Update(){
		if(Input.GetButtonDown("Fire1")){
			MakeMap();
		}
	}


	void MakeMap(){
		map = new int [width,height];
		RandomFillMapV3();

		for(int i = 1; i <= smoothingPasses; i++){
			SmoothMapV2();
		}
		//from tatorial 5
		removeSmallMapRegions();

		//stuff from the second tutroial 
		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.MakeMesh(map,squareSize);
	}


	void RandomFillMapV1(){
		if(randomSeed){
			seed = DateTime.Now.Ticks.ToString();  // not stock
		}
		System.Random randomNumbers = new System.Random(seed.GetHashCode());      

		for(int x = 0; x < width; x++){										//initalizes the map randomly based on fill%
			for(int y = 0; y < height; y++){
				if(randomNumbers.Next(0,100) < fillPercent || x == 0|| y ==0 || x == width-1 || y == height-1){ // makes sure all edges are walls
					map[x,y] = 1; 
				}
			}
		}
	}

	void RandomFillMapV2(){
		if(randomSeed){
			seed = DateTime.Now.Ticks.ToString();  // not stock
		}
		System.Random randomNumbers = new System.Random(seed.GetHashCode());      

		for(int x = 0; x < width; x++){										//initalizes the map randomly based on fill%
			for(int y = 0; y < height; y++){
				if(randomNumbers.Next(0,100) < fillPercent || x == 0 || y == 0 || x == width-1 || y == height-1 || x == 1 || y == 1 || x == width-2 || y == height-2){ // makes sure all edges are walls
					map[x,y] = 1; 
				}
			}
		}
	}



	void RandomFillMapV3(){   //this may not be a good solution always makes the wall one less thick at the top alternitive is at 3:00 in vid 4 use with random fill map v1 --------nvm it is not a prob with random fill map v3 but with the drawing somewhere down the line.
		if(randomSeed){
			seed = DateTime.Now.Ticks.ToString();  // not stock
		}
		System.Random randomNumbers = new System.Random(seed.GetHashCode());      

		for(int x = 0; x < width; x++){										//initalizes the map randomly based on fill%
			for(int y = 0; y < height; y++){
				if(randomNumbers.Next(0,100) < fillPercent || x < borderSize || y < borderSize || x >= width-borderSize || y >= height-borderSize){ // makes sure all edges are walls
					map[x,y] = 1; 
				}
			}
		}
	}

	void SmoothMapV1(){								//properly I should be using a temp map but in practice I seem to get better resaults without
		for(int x = 0; x < width; x++){	
			for(int y = 0; y < height; y++){
				int numberOfAdjacentWalls = GetSurroundingWallCountAtPosition(x,y);
				if(numberOfAdjacentWalls > 4){
					map[x,y] = 1;
				}else if(numberOfAdjacentWalls < 4){
					map[x,y] = 0;
				}
			}
		}	
	}

	void SmoothMapV2(){								//temp map version
		int [,] tempMap = new int [width,height];
		for(int x = 0; x < width; x++){	
			for(int y = 0; y < height; y++){
				int numberOfAdjacentWalls = GetSurroundingWallCountAtPosition(x,y);
				if(numberOfAdjacentWalls > 4){
					tempMap[x,y] = 1;
				}else if(numberOfAdjacentWalls < 2){
					tempMap[x,y] = 0;
				}
			}
		}
		map = tempMap;	
	}

	int GetSurroundingWallCountAtPosition(int x, int y){
		int numberOfWalls = 0;
		for(int adjacentTileX = x-1; adjacentTileX <= x + 1; adjacentTileX++){	//goes through all neighbors 
			for(int adjacentTileY = y-1; adjacentTileY <= y + 1; adjacentTileY++){
				if(adjacentTileX >= 0 && adjacentTileY >= 0 && adjacentTileX < width && adjacentTileY <	 height){
					if(x != adjacentTileX || y != adjacentTileY){
						numberOfWalls += map[adjacentTileX,adjacentTileY]; 
					}
				}else{
					numberOfWalls++;  /////////should cause more walls to form around edges not sure if needed.
				}


			}
		}
		return numberOfWalls;
	}




	struct Coordinate{
		public int x;
		public int y;

		public Coordinate(int x, int y){
			this.x = x;
			this.y = y;
		}
	}


	List<Coordinate> GetRegonTiles(int startingX, int startingY){
		List<Coordinate> tiles = new List<Coordinate>();
		int[,] mapFlags = new int[width,height];
		Queue<Coordinate> queue = new Queue<Coordinate>();
		queue.Enqueue(new Coordinate(startingX,startingY));
		mapFlags [startingX,startingY] = 1;
		int tileType = map[startingX,startingY];

		while(queue.Count > 0){ // maybe good
			Coordinate tile = queue.Dequeue();
			tiles.Add(tile);

			for(int x = tile.x - 1; x <= tile.x + 1; x++){
				for(int y = tile.y - 1; y <= tile.y + 1; y++){
					if(IsInMapRange(x,y) && (y == tile.y || x == tile.x)){
						if (mapFlags[x,y] == 0 && map[x,y] == tileType){
							mapFlags[x,y] = 1;
							queue.Enqueue(new Coordinate(x,y));
						}
					}
				}
			}
		}
		return tiles;
	}

	bool IsInMapRange(int x, int y){
		return(x >= 0 && x < width && y >= 0 && y < height);
	}


	List<List<Coordinate>> GetRegions(int tileType){
		List<List<Coordinate>> regions = new List<List<Coordinate>>();
		int[,] mapFlags = new int[width,height];
		for(int x = 0; x < width; x++){	
			for(int y = 0; y < height; y++){
				if(mapFlags[x,y] == 0 && map[x,y] == tileType){
					List<Coordinate> newRegion = GetRegonTiles(x,y);
					regions.Add(newRegion);
					foreach(Coordinate tile in newRegion){
						mapFlags[tile.x,tile.y] = 1;
					}
				}
			}
		}
		return regions;
	}

	void removeSmallMapRegions(){
		List<List<Coordinate>> wallRegions = GetRegions(1);
		foreach(List<Coordinate> wallRegion in wallRegions){
			if(wallRegion.Count < wallThresholdSize){
				foreach(Coordinate tile in wallRegion){
					map[tile.x,tile.y] = 0;
				}
			}
		}


		List<List<Coordinate>> roomRegions = GetRegions(0);
		foreach(List<Coordinate> roomRegion in roomRegions){
			if(roomRegion.Count < roomThresholdSize){
				foreach(Coordinate tile in roomRegion){
					map[tile.x,tile.y] = 1;
				}
			}
		}
	}


//draws the map in a location, i dont think ill need this later ------ I was right this is just for testing
	/*void OnDrawGizmos(){     // look into this gizmos thing
		if(map != null){
			for(int x = 0; x < width; x++){
				for(int y = 0; y < height; y++){
					if(map[x,y] == 1){
							Gizmos.color = Color.black;
						}else{
							Gizmos.color = Color.white;
						}
					Vector3 position = new Vector3(-width/2 + x + 0.5f, 0, -height/2 + y + 0.5f);
					Gizmos.DrawCube(position, Vector3.one);
				}
			}
		}

	}*/




}


	