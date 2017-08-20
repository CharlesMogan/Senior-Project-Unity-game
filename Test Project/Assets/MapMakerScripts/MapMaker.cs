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
		RandomFillMap();

		for(int i = 1; i <= smoothingPasses; i++){
			SmoothMap();
		}
	}


	void RandomFillMap(){
		if(randomSeed){
			seed = DateTime.Now.Ticks.ToString();  // not stock
		}
		System.Random randomNumbers = new System.Random(seed.GetHashCode());      

		for(int x = 0; x < width; x++){										//initalizes the map randomly based on fill%
			for(int y = 0; y < height; y++){
				if(randomNumbers.Next(0,100) < fillPercent || x == 0 || y == 0 || x == width-1 || y == height-1){ // makes sure all edges are walls
					map[x,y] = 1; 
				}
			}
		}

	}


	void SmoothMap(){								//properly I should be using a temp map but in practice I seem to get better resaults without
		int [,] tempMap = new int [width,height];
		for(int x = 0; x < width; x++){	
			for(int y = 0; y < height; y++){
				int numberOfAdjacentWalls = GetSurroundingWallCountAtPosition(x,y);
				if(numberOfAdjacentWalls > 4){
					tempMap[x,y] = 1;
				}else if(numberOfAdjacentWalls < 3){
					tempMap[x,y] = 0;
				}
			}
		}
		map = tempMap;	
	}

	int GetSurroundingWallCountAtPosition(int x, int y){
		int numberOfWalls = 0;
		for(int adjacentTileX = x-1; adjacentTileX <= x + 1; adjacentTileX++){	//goes through all neighbours 
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



//draws the map in a location, i dont think ill need this later
	void OnDrawGizmos(){     // look into this gizmos thing
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

	}




}


	