using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class Generator : MonoBehaviour {
	public GameObject wallPrefab = null;
	private UnityEngine.GameObject[] walls = null;
	
	public int cols = 50;
	public int rows = 50;
	
	public int rooms = 50;
	public static List<List<int>> map = null;
	
	public static bool generatorDone = false;
	
	private bool playerSet = false;
	
	public GameObject textMesh = null;
	
	public static DateTime start = DateTime.Now;
	
	// Use this for initialization
	void Start () {		
		start = DateTime.Now;
	}
	
	/*void DrawLine(int x1, int y1, int x2, int y2)
	{
		int dx = x2 - x1;
		int dy = y2 - y1;
		
		int y = 0;
		
		if (dx == 0)
			dx = 1;
		
		if (x1 < 0 && x2 < 0 && y1 < 0 && y2 < 0)
			return;
		
		if (x1 > cols - 1 && x2 > cols - 1 && y1 > rows - 1 && y2 > rows - 1)
			return;
		
		for (int x = x1; x <= x2; x++) {
			y = y1 + (dy) * (x - x1)/(dx);
		        
			map[x][y] = 0;
		}	
	}*/
	
	void DrawLine(int x0, int y0, int x1, int y1)
	{
		int dx = Math.Abs(x1-x0);
		int dy = Math.Abs(y1-y0) ;
		
		int sx = 0, sy = 0;
		
		if (x0 < x1)
			sx = 1;
		else 
			sx = -1;
						
		if (y0 < y1) 
			sy = 1; 
		else 
			sy = -1;
						
		int err = dx-dy;
	 	int e2 = 0;
			
	   	while (true)
		{
			map[x0][y0] = 0;
			if (x0 == x1 && y0 == y1) 
				break;
			
			e2 = 2*err;
			if (e2 > -dy)
			{
				err = err - dy;
				x0 = x0 + sx;
			}
			if (e2 <  dx)
			{
				err = err + dx;
				y0 = y0 + sy;
			}
		}
	}
		
	
	bool DrawRoom(int x, int y, int width, int height/*, bool firstRow, bool firstCol*/)
	{
		if (x < 1 || y < 1)
		{
			//Debug.Log("Refused: x|y < 1");
			return false;
		}
		
		if (x + width >= cols)
		{
			//Debug.Log("Refused: x + width >= cols");
			return false;
		}
		
		if (y + height >= rows)
		{
			//Debug.Log("Refused: x + height >= rows");
			return false;
		}
		
		if (height < 3 || width < 3)
		{
			//Debug.Log("Refused: height < 3 || width < 3");
			return false;
		}
		
		for (int col = x; col < x + width; col++)
		{
			for (int row = y; row < y + height; row++)
			{				
				map[row][col] = 0;		
			}
		}	
		
		if (x > 2)
		{
			if (map[y+(height/2)][x-2] == 0)
			{
				map[y+(height/2)][x-1] = 0;
			}
		}
		
		if (y > 2)
		{
			if (map[y-2][x+(width/2)] == 0)
			{
				map[y-1][x+(width/2)] = 0;
			}
		}
				
		return true;
	}
	
	bool GenerateMap()
	{
		/*if (map == null)
		{
			map = new  List<List<int>>(); 				
			lastCol = lastRow = 0;
		}
		
		for (; lastRow < rows; lastRow++)
		{
			List<int> arr = null;
			
			if (map.Count > lastRow)
			{
				arr = map[lastRow];
			}
			else
			{
				arr = new List<int>();
								
				map.Add(arr);
			}						
			
			for (; lastCol < cols; lastCol++)
			{
				arr.Add(1);	
			}			
			
			lastCol = 0;
			
			if (lastRow < rows)
			{
				lastRow++;
				return false;
			}
		}*/
		
		map = new List<List<int>>();
		for (int col = 0; col < cols; col++)
		{
			map.Add (new List<int>());
			map[col] = new List<int>();
			
			for (int row = 0; row < rows; row++)
			{
				map[col].Add(1);
			}
		}
		
		
#if true
		int /*x,*/ y, xRange = 0, yRange = 0, width, height;
		
		List<int> roomXList = new List<int>();
		List<int> roomYList = new List<int>();
		
		//Debug.Log("Rooms to generate: " + rooms);
						
		/*int xDivide = 4;
		int yDivide = 4;*/
		
		int xStep = 5;
		int yStep = 5;
		
		int xCounter = 1;
		int yCounter = 1;
		
		while (true)
		{
			while (true)
			{
				int rand = UnityEngine.Random.Range(1, 100);
				
				int offset = 0;
				
				if (rand > 50)
				{
					offset += xStep;
				}
				else
				{
					offset += 2*xStep;
				}
				
				if (xCounter + offset > cols-1)
				{
					if (offset == xStep)
						break;
					else
					{
						if (xCounter + xStep <= cols-1)
							offset = xStep;
						else
							break;
					}
				}
				if (yCounter + yStep > rows-1)
					break;
				
				DrawRoom(xCounter, yCounter, offset, yStep/*, xCounter == 1, yCounter == 1*/);
				
				roomXList.Add(xCounter);
				roomYList.Add(yCounter);
				
				xCounter += offset+1;
				
				if (xCounter >= cols-1)
					break;
			}
			
			xCounter = 1;
			yCounter += yStep + 1;
			
			if (xCounter > cols || yCounter > rows)
				break;
		}
		
		return true;
#endif
	}
	
	void OnGUI()
	{
		if (!generatorDone)
		{			
			GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 30), "Generating maze and AI waypoints", "box");
		}
	}
	
	void SaveToFile()
	{
		string fileName = Application.persistentDataPath + "/" + "grid";
		var fileWriter = File.CreateText(fileName);
		
		fileWriter.WriteLine(cols);
		fileWriter.WriteLine(rows);
		
		
		for (int col = 0; col < cols; col++)
		{
			
			for (int row = 0; row < rows; row++)
			{
				if (map[col][row] == 0)
					fileWriter.WriteLine(0);
				else
					fileWriter.WriteLine(1);
			}
		}
		
		fileWriter.Close();
	}
	
	bool LoadFromFile()
	{
		string fileName = Application.persistentDataPath + "/" + "grid";
		StreamReader fileReader = null;
		
		try {
			fileReader = File.OpenText(fileName);	
		}
		catch (IOException e)
		{
			return false;	
		}
		
		string str1 = fileReader.ReadLine();
		
		if (str1 == null)
			return false;
		
		int actCols = Int32.Parse(str1);
		int actRows = Int32.Parse (fileReader.ReadLine());
		
		if (actCols != cols || actRows != rows)
			return false;
		
		map = new List<List<int>>();
		for (int col = 0; col < cols; col++)
		{
			map.Add (new List<int>());
			map[col] = new List<int>();
			
			for (int row = 0; row < rows; row++)
			{
				string act = fileReader.ReadLine();
				
				if (act == "0")
					map[col].Add(0);
				else
					map[col].Add(1);
			}
		}
		
		fileReader.Close();
		
		return true;
	}
	
	bool SaveFileExists()
	{
		string fileName = Application.persistentDataPath + "/" + "grid";
		
		return File.Exists(fileName);
	}
	
	// Update is called once per frame
	void Update () {
		if (!generatorDone)
		{	
			bool result = LoadFromFile();
			
			if (result)
			{
				Debug.Log ("Map succesfully loaded");	
			}
			else
			{
				if (!GenerateMap())
					return;
			}
			
			//SaveToFile();
						
			float startX 	= cols;
			float startZ 	= rows;		
			
			walls = new UnityEngine.GameObject[2500];
			
			int counter = 0;
			
			for (int col = 0; col < cols; col++)
			{
				for (int row = 0; row < rows; row++)
				{	
					Vector3 position = new Vector3(startX - (col * 2), 1f, startZ - (row * 2));	
					
					if (map[col][row] == 0 && col > (cols / 2.0f) && (row > rows / 2.0f) && !playerSet)
					{
						GameObject.Find ("Player").transform.position = position + new Vector3(0.0f, 3.0f, 0.0f);
						playerSet = true;
					}
					
					if (map[col][row] == 0)
						continue;
					
					walls[counter] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					walls[counter].name = "wall" + counter;
					walls[counter].transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
					walls[counter].transform.position = position;
					//walls[counter].AddComponent<BoxCollider>();
					walls[counter].GetComponent<BoxCollider>().size = new Vector3(1.0f, 1.0f, 1.0f);
					
					walls[counter].GetComponent<MeshRenderer>().enabled = false;
					/*walls[counter].GetComponent<MeshFilter>().mesh = null;*/
					walls[counter].tag = "wall";
									
					counter++;				
				}
			}
			
			var mf = wallPrefab.GetComponent<MeshFilter>();
					
			var mesh = mf.mesh;
					
			Debug.Log("Original mesh vertex count: " + mesh.vertexCount);
			
			var globalVertexCount = mesh.vertexCount * counter;
			var globalNormalCount = mesh.normals.Length * counter;
			var globalUVCount = mesh.uv.Length * counter;
			var globalTriCount = mesh.triangles.Length * counter;
			
			Vector3[] vertices = new Vector3[globalVertexCount];
			Vector3[] normals = new Vector3[globalNormalCount];
			Vector2[] uvs = new Vector2[globalUVCount];
			int[] tris = new int[globalTriCount];
			
			var actualVertexCount = 0;
			var actualNormalCount = 0;
			var actualUVCount = 0;
			var actualTriCount = 0;
			
			for (int col = 0; col < cols; col++)
			{
				for (int row = 0; row < rows; row++)
				{
					if (map[col][row] == 0)
						continue;
					
					Vector3 position = new Vector3(startX - (col * 2), 0.5f, startZ - (row * 2));
					
					for (int i = 0; i < mesh.vertexCount; i++)
					{
						vertices[i + actualVertexCount] = mesh.vertices[i];
						vertices[i + actualVertexCount].Scale(new Vector3(2, 2, 2));
						vertices[i + actualVertexCount] += position;
					}
					
					for (int i = 0; i < mesh.normals.Length; i++)
						normals[i+actualNormalCount] = mesh.normals[i];
					
					for (int i = 0; i < mesh.uv.Length; i++)
						uvs[i+actualUVCount] = mesh.uv[i];
					
					for (int i = 0; i < mesh.triangles.Length; i++)
						tris[i+actualTriCount] = mesh.triangles[i] + actualVertexCount;
					
					actualVertexCount += mesh.vertexCount;
					actualNormalCount += mesh.normals.Length;
					actualUVCount += mesh.uv.Length;
					actualTriCount += mesh.triangles.Length;
				}
			}
			
			MeshFilter newMf = GetComponent<MeshFilter>();
			Mesh newMesh = new Mesh();
			
			Debug.Log("vertex count: " + vertices.Length + ", actualVertexCount: " + actualVertexCount);
			
			newMesh.vertices = vertices;
			newMesh.uv = uvs;
			newMesh.normals = normals;
			newMesh.triangles = tris;
			
			newMf.mesh = newMesh;
			
			var maze = new GameObject("maze");
			maze.AddComponent<MeshFilter>();
			maze.AddComponent<MeshRenderer>();
			
			maze.GetComponent<MeshFilter>().mesh = newMesh;
			maze.GetComponent<MeshRenderer>().materials = wallPrefab.GetComponent<MeshRenderer>().materials;
			
			maze.transform.position = new Vector3(0f, 0.6f, 0f);	
			
			generatorDone = true;	
		}
	}
}
