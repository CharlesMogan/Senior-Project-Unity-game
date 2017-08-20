using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	public SquareGrid squareGrid;
	public void MakeMesh(int [,] map, float squareSize){
		squareGrid = new SquareGrid(map, squareSize);
	}



	//just to test
	void OnDrawGizmos() {
		if (squareGrid != null){
			for(int x = 0; x < squareGrid.squares.GetLength(0); x++){	
				for(int y = 0; y < squareGrid.squares.GetLength(1); y++){
					 
					if(squareGrid.squares[x,y].topLeft.isActive){
						Gizmos.color = Color.black;	
					}else{
						Gizmos.color = Color.white;
					}
					Gizmos.DrawCube(squareGrid.squares[x,y].topLeft.position, Vector3.one * .4f);
				
					if(squareGrid.squares[x,y].topRight.isActive){
						Gizmos.color = Color.black;	
					}else{
						Gizmos.color = Color.white;
					}
					Gizmos.DrawCube(squareGrid.squares[x,y].topRight.position, Vector3.one * .4f);
				
					if(squareGrid.squares[x,y].bottomRight.isActive){
						Gizmos.color = Color.black;	
					}else{
						Gizmos.color = Color.white;
					}
					Gizmos.DrawCube(squareGrid.squares[x,y].bottomRight.position, Vector3.one * .4f);
					
					if(squareGrid.squares[x,y].bottomLeft.isActive){
						Gizmos.color = Color.black;	
					}else{
						Gizmos.color = Color.white;
					}
					Gizmos.DrawCube(squareGrid.squares[x,y].bottomLeft.position, Vector3.one * .4f);
				

					Gizmos.color = Color.gray;
					Gizmos.DrawCube(squareGrid.squares[x,y].centerTop.position, Vector3.one * .15f);
					Gizmos.DrawCube(squareGrid.squares[x,y].centerRight.position, Vector3.one * .15f);
					Gizmos.DrawCube(squareGrid.squares[x,y].centerBottom.position, Vector3.one * .15f);
					Gizmos.DrawCube(squareGrid.squares[x,y].centerLeft.position, Vector3.one * .15f);

				}
			}

		}
	}




	public class SquareGrid{
			public Square[,] squares;
			
			public SquareGrid(int[,] map, float squareSize){
				int widthSquares = map.GetLength(0);
				int heightSquares = map.GetLength(1);
				float mapWidth = widthSquares * squareSize;
				float mapHeight = heightSquares * squareSize;

				ControlNode[,] controlNodes = new ControlNode[widthSquares,heightSquares];

				for(int x = 0; x < widthSquares; x++){	
					for(int y = 0; y < heightSquares; y++){
						Vector3 position = new Vector3(-mapWidth/2 + x*squareSize + squareSize/2, 0, -mapHeight/2 + y*squareSize + squareSize/2); //15:14 vid 2 figure out what the hell is going on here in the morning
						controlNodes[x,y] = new ControlNode(position,map[x,y] == 1,squareSize);
					}
				}
				squares = new Square[widthSquares - 1, heightSquares - 1];  //there will always be  one less square then there is node
				for(int x = 0; x < widthSquares-1; x++){	
					for(int y = 0; y < heightSquares-1; y++){
						squares[x,y] = new Square(controlNodes[x,y+1], controlNodes[x+1,y+1], controlNodes[x+1,y], controlNodes[x,y]);
					}
				}
			}
		}




	public class Square{
		public ControlNode topLeft, topRight, bottomRight, bottomLeft;
		public Node centerTop, centerRight, centerBottom, centerLeft; 
		
		public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft){
			this.topLeft = topLeft;
			this.topRight = topRight;
			this.bottomLeft = bottomLeft;
			this.bottomRight = bottomRight;



			centerTop = topLeft.right;
			centerRight = bottomRight.above;
			centerBottom = bottomLeft.right;
			centerLeft = bottomLeft.above;
		}
	}



	public class Node{
		public Vector3 position;
		public int vertexIndex = -1;
		
		public Node(Vector3 position){
			this.position = position;
		}
	}

	public class ControlNode : Node{
		public bool isActive;
		public Node above, right;
		
		public ControlNode(Vector3 position, bool isActive, float squareSize) : base(position){
			this.isActive = isActive;
			above = new Node(position + Vector3.forward * squareSize/2f);
			right = new Node(position + Vector3.right * squareSize/2f);
		}
	}
}
