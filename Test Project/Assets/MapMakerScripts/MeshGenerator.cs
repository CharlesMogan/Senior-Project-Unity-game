using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//	1   2
//
//  4   3

//	1   2
//
//  8   4
public class MeshGenerator : MonoBehaviour {

	public SquareGrid squareGrid;
	public float wallHeight;
	public MeshFilter walls;
	List<Vector3> vertices;
	List<int> triangles;
	List<List<int>> outlines = new List<List<int>>();
	Dictionary<int,List<Triangle>> triangleDictionary = new Dictionary<int,List<Triangle>>();
	HashSet<int>  checkedVertices = new HashSet<int>();




	public void MakeMesh(int [,] map, float squareSize){
		triangleDictionary.Clear();
		outlines.Clear();
		checkedVertices.Clear();

		squareGrid = new SquareGrid(map, squareSize);
		vertices = new List<Vector3>();
		triangles = new List<int>();
		for(int x = 0; x < squareGrid.squares.GetLength(0); x++){	
			for(int y = 0; y < squareGrid.squares.GetLength(1); y++){
			TriangulateSquare(squareGrid.squares[x,y]);	
			}
		}
	

		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();  //maybe not nessary

		MakeWallMesh();
	}


	void MakeWallMesh(){

		CalculateMeshOutlines();

		List<Vector3> wallVertices = new List<Vector3>();
		List<int> wallTriangles = new List<int>();
		Mesh wallMesh = new Mesh();	
	

		foreach(List<int> outline in outlines){
			for(int i = 0; i < outline.Count -1; i++){
				int startingIndex = wallVertices.Count;
				wallVertices.Add(vertices[outline[i]]); //left
				wallVertices.Add(vertices[outline[i+1]]); //right
				wallVertices.Add(vertices[outline[i]] - Vector3.up * wallHeight); // bottom left
				wallVertices.Add(vertices[outline[i+1]] - Vector3.up * wallHeight); //bottom right

				wallTriangles.Add(startingIndex + 0);
				wallTriangles.Add(startingIndex + 2);
				wallTriangles.Add(startingIndex + 3);

				wallTriangles.Add(startingIndex + 3);
				wallTriangles.Add(startingIndex + 1);
				wallTriangles.Add(startingIndex + 0);
			}
		}
		wallMesh.vertices = wallVertices.ToArray();
		wallMesh.triangles = wallTriangles.ToArray();
		walls.mesh = wallMesh;
	}





	void TriangulateSquare(Square square){
		switch(square.configuration){
			case 0:
				break;
			case 1:
				MeshFromPoints(square.centerLeft, square.centerBottom, square.bottomLeft);     //fixed
				break;
			case 2:
				MeshFromPoints(square.bottomRight, square.centerBottom, square.centerRight);   //fixed
				break;
			case 4:
				MeshFromPoints(square.topRight, square.centerRight, square.centerTop);         //fixed
				break;
			case 8:
				MeshFromPoints(square.topLeft, square.centerTop, square.centerLeft);
				break;

			case 3:
				MeshFromPoints(square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
				break;
			case 6:
				MeshFromPoints(square.centerTop, square.topRight, square.bottomRight, square.centerBottom);
				break;
			case 9:
				MeshFromPoints(square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
				break;
			case 12:
				MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerLeft);
				break;
				

			case 5:
				MeshFromPoints(square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft, square.centerLeft);
				break;
			case 10:
				MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft);
				break;


			case 7:
				MeshFromPoints(square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft);
				break;
			case 11:
				MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft);
				break;
			case 13:
				MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft);
				break;
			case 14:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
				break;


			case 15:																						
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
				checkedVertices.Add(square.topLeft.vertexIndex);				//cant be edge walls
				checkedVertices.Add(square.topRight.vertexIndex);
				checkedVertices.Add(square.bottomRight.vertexIndex);
				checkedVertices.Add(square.bottomLeft.vertexIndex);
				break;
		}

	}





	void MeshFromPoints(params Node[] points){//--------------------------------------------------------------------------------------------11.27
		AssignVertices(points);
		if(points.Length >= 3){
			MakeTriangle(points[0], points[1], points[2]);
		}
		if (points.Length >= 4){
			MakeTriangle(points[0], points[2], points[3]);
		}
		if (points.Length >= 5){
			MakeTriangle(points[0], points[3], points[4]);
		}
		if (points.Length >= 6){
			MakeTriangle(points[0], points[4], points[5]);
		}
	}




	void AssignVertices(Node[] points){					//figrue out what this does vid 3 13:00
			for(int i = 0; i < points.Length; i++){
				if(points[i].vertexIndex == -1){
					points[i].vertexIndex = vertices.Count;
					vertices.Add(points[i].position);
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
		public int configuration;

		public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft){
			this.topLeft = topLeft;
			this.topRight = topRight;
			this.bottomLeft = bottomLeft;
			this.bottomRight = bottomRight;



			centerTop = topLeft.right;
			centerRight = bottomRight.above;
			centerBottom = bottomLeft.right;
			centerLeft = bottomLeft.above;


			if(topLeft.isActive){
				configuration+=8;
			}

			if(topRight.isActive){
				configuration+=4;
			}

			if(bottomRight.isActive){
				configuration+=2;
			}

			if(bottomLeft.isActive){
				configuration+=1;
			}

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


	void MakeTriangle(Node edgeA, Node edgeB, Node edgeC){	//not 100% sure edges	
		triangles.Add(edgeA.vertexIndex);
		triangles.Add(edgeB.vertexIndex);
		triangles.Add(edgeC.vertexIndex);

		Triangle triangle = new Triangle(edgeA.vertexIndex, edgeB.vertexIndex, edgeC.vertexIndex);
		AddToDictionary(triangle.vertexIndexA, triangle);
		AddToDictionary(triangle.vertexIndexB, triangle);
		AddToDictionary(triangle.vertexIndexC, triangle);
	}


	void AddToDictionary(int aKey, Triangle aTriangle){
		if(triangleDictionary.ContainsKey(aKey)){
			triangleDictionary[aKey].Add(aTriangle);
		}else{
			List<Triangle> triangleList = new List<Triangle>();
			triangleList.Add(aTriangle);
			triangleDictionary.Add(aKey,triangleList);
		}
	}


	bool IsOutlineEdge(int vertexA, int vertexB){   //works by geting a list of all triangles to which vertex a belongs and a list of all triangles to which vertex b belongs, if they only have one common triangle your golden
		int sharedTriangles = 0;
		List<Triangle > trianglesWithA = triangleDictionary[vertexA];

		for(int i = 0; i < trianglesWithA.Count; i++){
			if(trianglesWithA[i].Contains(vertexB)){
				sharedTriangles++;
				if(sharedTriangles > 1){
					break;
				}
			}
		}
		return (sharedTriangles == 1);
	}


	void CalculateMeshOutlines(){
		for(int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++){
			if(!checkedVertices.Contains(vertexIndex)){
				int newOutlineVertex = GetOutlineVertex(vertexIndex);
				if(newOutlineVertex != -1){
					checkedVertices.Add(vertexIndex);

					List<int> newOutline = new List<int>();
					newOutline.Add(vertexIndex);
					outlines.Add(newOutline);
					FollowOutline(newOutlineVertex, outlines.Count-1);
					outlines[outlines.Count-1].Add(vertexIndex); 

				}
			}
		}
	}


	int GetOutlineVertex(int vertexIndex){												//this seems wacky reevaluate in morning hours in vid 4 at 1800
		List<Triangle > trianglesWithVertex = triangleDictionary[vertexIndex];
		for(int i = 0; i < trianglesWithVertex.Count; i++){
			Triangle triangle = trianglesWithVertex[i];
			for(int j = 0; j < 3; j++){  //number of verticies per triangle
				int vertexB = triangle[j];
				if(vertexB != vertexIndex && !checkedVertices.Contains(vertexB)){
					if(IsOutlineEdge(vertexIndex, vertexB)){
						return vertexB;
					}
				}

			}
		}
		return -1;
	}

	

	void FollowOutline(int vertexIndex, int outlineIndex){
		outlines[outlineIndex].Add(vertexIndex);
		checkedVertices.Add(vertexIndex);
		int nextVertexIndex = GetOutlineVertex(vertexIndex);

		if(nextVertexIndex != -1){          //not reached end of outline
			FollowOutline(nextVertexIndex, outlineIndex);
		}
	}


	struct Triangle{
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		int[] vertices;

		public Triangle (int a, int b, int c){
			vertexIndexA = a;
			vertexIndexB = b;
			vertexIndexC = c;
		
			vertices = new int[3];
			vertices[0] = a;
			vertices[1] = b;
			vertices[2] = c;
		}

		public int this[int i]{
			get{
				return vertices[i];
			}
		}

		public bool Contains(int vertexIndex){
			return (vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC);
		}

	}




//just to test
/*	void OnDrawGizmos() {
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
*/
}
