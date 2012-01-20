using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WaypointGenerator : MonoBehaviour
{
	public float CenterX = 0;
	public float CenterY = 0;
	
	public float Width = 0;
	
	public float Step = 0;
	
	/*public WaypointGenerator ()
	{
		
	}*/
	
	//private List<List<PathNode>> matrix = new List<List<PathNode>>();
	
	private PathNode[][] matrix = null;
	
	public static bool GenerationDone = false;
	
	private List<GameObject> pathNodes = new List<GameObject>();
	
	private bool FileLoaded = false;
	private bool LoadingCorrupted = false;
	
	private int timeFinished = 0;
	
	void Init()
	{
		
	}
	
	float MinimalDistance(Vector3 tested, List<GameObject> objs)
	{
		float min = float.MaxValue;
		
		foreach (GameObject go in objs)
		{
			float distance = Vector3.Distance(tested, go.transform.position);
			
			if (min > distance)
				min = distance;
		}
		
		return min;
	}
	
	bool SaveToFile()
	{
		string fileName = Application.persistentDataPath + "/" + "waypoints";
		var fileWriter = File.CreateText(fileName);
		fileWriter.WriteLine(Width);
		//fileWriter.WriteLine(Width * Width);
		
		for (int x = 0; x < Width; x += 1)
		{							
			for (int y = 0; y < Width; y += 1)
			{
				/*if ( || matrix[x][y].nodeEnabled == false)
				{
					fileWriter.WriteLine("NULL");
				}
				else*/ if (matrix[x][y] != null && matrix[x][y].nodeEnabled == true)
				{
					Vector3 pos = matrix[x][y]./*GetComponent<PathNode>().*/Position;
					//List<int> indices = matrix[x][y].GetComponent<PathNode>().connectionIndices;
					
					fileWriter.WriteLine(x + "," + y);
					fileWriter.WriteLine(pos.x + "," + pos.y + "," + pos.z);
					/*fileWriter.WriteLine(indices.Count);
					
					foreach (int index in indices)
						fileWriter.WriteLine(index);*/
				}
			}
		}
		
		fileWriter.Close();
		
		return true;
	}
	
	bool SaveFileExist()
	{
		string fileName = Application.persistentDataPath + "/" + "waypoints";
		
		if (File.Exists(fileName))
			return true;
		else
			return false;
	}
	
	void OnGUI()
	{
		if (!FileLoaded && SaveFileExist() && !GenerationDone)
		{			
			GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 50, 300, 30), "Loading AI waypoints from file", "box");
		}
		
		GUI.Label(new Rect(1, 40, 200, 28), "Time to load level: " + timeFinished + "s", "box");
	}
	
	bool LoadFile()
	{
		string fileName = Application.persistentDataPath + "/" + "waypoints";
		
		StreamReader fileReader = null;
		
		try {
			fileReader = File.OpenText(fileName);
		}
		catch (IOException e)
		{
			return false;	
		}
		
		string width = fileReader.ReadLine();
		if (Width != float.Parse(width))
			return false;
		
		matrix = new PathNode[(int) Width][];
			
		for (int x = 0; x < Width; x += 1)
		{			
			matrix[x] = new PathNode[(int)Width];
			
			for (int y = 0; y < Width; y += 1)
			{
				matrix[x][y] = null;
			}
		}
		
		int counter = 0;
		
		int i = 0;
		
		/*for (int x = 0; x < Width; x += 1)
		{			
			for (int y = 0; y < Width; y += 1)*/
		
			while (true)
			{
				string tmp = fileReader.ReadLine();
					
				if (tmp == null)
					break;
			
				/*int y = i / (int)Width;
				int x = i % (int) Width;*/
				
				string[] arr2 = tmp.Split(new char[]{ ','});
			
				if (arr2.Length != 2)
					return false;
			
				int x = Int32.Parse(arr2[0]);
				int y = Int32.Parse(arr2[1]);
			
				/*if (tmp == "NULL")
				{
					matrix[x][y] = null;	
				}
				else
				{*/
					Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
					
					tmp = fileReader.ReadLine();
					
					string[] arr = tmp.Split(new char[] {',' });
					
					if (arr.Length != 3)
						return false;
					
					position.x = float.Parse(arr[0]);
					position.y = float.Parse(arr[1]);
					position.z = float.Parse(arr[2]);
					
					matrix[x][y] = PathNode.Spawn(position, true).GetComponent<PathNode>();
			
					if (x > 0 && matrix[x-1][y] != null && matrix[x-1][y].nodeEnabled)
					{
						matrix[x][y].connections.Add(matrix[x-1][y]);
						matrix[x][y].connectionIndices.Add((x-1) + (y * (int)Width));
					}
					if (x < Width-1 && matrix[x+1][y] != null && matrix[x+1][y].nodeEnabled)
					{							
						matrix[x][y].connections.Add(matrix[x+1][y]);
						matrix[x][y].connectionIndices.Add((x+1) + (y * (int)Width));
					}
					if (y > 0 && matrix[x][y-1] != null && matrix[x][y-1].nodeEnabled)
					{
						matrix[x][y].connections.Add(matrix[x][y-1]);
						matrix[x][y].connectionIndices.Add((x) + ((y-1) * (int)Width));
					}
					if (y < Width-1 && matrix[x][y+1] != null && matrix[x][y+1].nodeEnabled)
					{
						matrix[x][y].connections.Add(matrix[x][y+1]);
						matrix[x][y].connectionIndices.Add((x) + ((y+1) * (int)Width));
					}
					
					if (x > 0 && y > 0 && matrix[x-1][y-1] != null && matrix[x-1][y-1].nodeEnabled)
					{
						matrix[x][y].connections.Add(matrix[x-1][y-1]);
						matrix[x][y].connectionIndices.Add((x-1) + ((y-1) * (int)Width));
					}
					if (x < Width+1 && y > 0 && matrix[x+1][y-1] != null && matrix[x+1][y-1].nodeEnabled)
					{
						matrix[x][y].connections.Add(matrix[x+1][y-1]);
						matrix[x][y].connectionIndices.Add((x+1) + ((y-1) * (int)Width));
					}
					if (x < Width+1 && y < Width+1 && matrix[x+1][y+1] != null && matrix[x+1][y+1].nodeEnabled)
					{
						matrix[x][y].connections.Add(matrix[x+1][y+1]);
						matrix[x][y].connectionIndices.Add((x+1) + ((y+1) * (int)Width));
					}
					if (x > 0 && y < Width-1 && matrix[x-1][y+1] != null && matrix[x-1][y+1].nodeEnabled)
					{
						matrix[x][y].connections.Add(matrix[x-1][y+1]);
						matrix[x][y].connectionIndices.Add((x-1) + ((y+1) * (int)Width));
					}
			
					counter++;
				/*}*/
			
				i++;
			}
		/*}*/
		
		Debug.Log ("PathNode count: " + counter);
		
		/*for (int x = 0; x < Width; x += 1)
		{			
			for (int y = 0; y < Width; y += 1)
			{
				if (matrix[x][y] == null)
					continue;
				
				if (x > 0 && matrix[x-1][y] != null && matrix[x-1][y].nodeEnabled)
				{
					matrix[x][y].connections.Add(matrix[x-1][y]);
					matrix[x][y].connectionIndices.Add((x-1) + (y * (int)Width));
				}
				if (x < Width-1 && matrix[x+1][y] != null && matrix[x+1][y].nodeEnabled)
				{							
					matrix[x][y].connections.Add(matrix[x+1][y]);
					matrix[x][y].connectionIndices.Add((x+1) + (y * (int)Width));
				}
				if (y > 0 && matrix[x][y-1] != null && matrix[x][y-1].nodeEnabled)
				{
					matrix[x][y].connections.Add(matrix[x][y-1]);
					matrix[x][y].connectionIndices.Add((x) + ((y-1) * (int)Width));
				}
				if (y < Width-1 && matrix[x][y+1] != null && matrix[x][y+1].nodeEnabled)
				{
					matrix[x][y].connections.Add(matrix[x][y+1]);
					matrix[x][y].connectionIndices.Add((x) + ((y+1) * (int)Width));
				}
				
				if (x > 0 && y > 0 && matrix[x-1][y-1] != null && matrix[x-1][y-1].nodeEnabled)
				{
					matrix[x][y].connections.Add(matrix[x-1][y-1]);
					matrix[x][y].connectionIndices.Add((x-1) + ((y-1) * (int)Width));
				}
				if (x < Width+1 && y > 0 && matrix[x+1][y-1] != null && matrix[x+1][y-1].nodeEnabled)
				{
					matrix[x][y].connections.Add(matrix[x+1][y-1]);
					matrix[x][y].connectionIndices.Add((x+1) + ((y-1) * (int)Width));
				}
				if (x < Width+1 && y < Width+1 && matrix[x+1][y+1] != null && matrix[x+1][y+1].nodeEnabled)
				{
					matrix[x][y].connections.Add(matrix[x+1][y+1]);
					matrix[x][y].connectionIndices.Add((x+1) + ((y+1) * (int)Width));
				}
				if (x > 0 && y < Width-1 && matrix[x-1][y+1] != null && matrix[x-1][y+1].nodeEnabled)
				{
					matrix[x][y].connections.Add(matrix[x-1][y+1]);
					matrix[x][y].connectionIndices.Add((x-1) + ((y+1) * (int)Width));
				}
			}
		}*/
		
		fileReader.Close();
		
		return true;
	}
	
	bool IsInside(List<GameObject> gos, Vector3 actPosition)
	{
		foreach (GameObject go in gos)
		{		
			Vector3 scale = go.transform.localScale;
			
			float leftBackX = go.transform.position.x - scale.x / 2.0f;
			float rightBackX = go.transform.position.x + scale.x / 2.0f;
			float leftBackZ = go.transform.position.z - scale.z / 2.0f;
			float rightBackZ = go.transform.position.z - scale.z / 2.0f;
			
			float leftFrontX = go.transform.position.x - scale.x / 2.0f;
			float rightFrontX = go.transform.position.x + scale.x / 2.0f;
			float leftFrontZ = go.transform.position.z + scale.z / 2.0f;
			float rightFrontZ = go.transform.position.z + scale.z / 2.0f;
		
			if (actPosition.x >= leftBackX && actPosition.x <= rightBackX && actPosition.z >= leftBackZ && actPosition.z <= leftFrontZ)
				return true;
		}
		
		return false;
	}
	
	Vector3 GetNearestVertex(GameObject go, Vector3 actPosition, out double minOut)
	{
		Vector3 scale = go.transform.localScale;
		
		float leftBackX = go.transform.position.x - scale.x / 2.0f;
		float rightBackX = go.transform.position.x + scale.x / 2.0f;
		float leftBackZ = go.transform.position.z - scale.z / 2.0f;
		float rightBackZ = go.transform.position.z - scale.z / 2.0f;
		
		float leftFrontX = go.transform.position.x - scale.x / 2.0f;
		float rightFrontX = go.transform.position.x + scale.x / 2.0f;
		float leftFrontZ = go.transform.position.z + scale.z / 2.0f;
		float rightFrontZ = go.transform.position.z + scale.z / 2.0f;
		
		var lst = new List<Vector3>();
		
		lst.Add(new Vector3(leftBackX, 0, leftBackZ));
		lst.Add(new Vector3(rightBackX, 0, rightBackZ));
		lst.Add(new Vector3(leftFrontX, 0, leftFrontZ));
		lst.Add(new Vector3(rightFrontX, 0, rightFrontZ));
		
		double min = double.MaxValue;
		Vector3 vMin = new Vector3(0, 0, 0);
		
		foreach (Vector3 vc in lst)
		{
			double distance = Vector3.Distance(vc, actPosition);	
			
			if (distance < min)
			{
				min = distance;
				vMin = vc;
			}
		}
		
		minOut = min;
		return vMin;
	}
	
	GameObject GetClosestObject(QuadTreeItem qi, Vector3 actPosition)
	{
		double min = double.MaxValue;
		GameObject closest = null;
		
		foreach (GameObject go in qi.GameObjects)
		{
			if (go.name == "Capsule")
			{
				Debug.Log("Capsule");	
			}
			
			double distance = Vector3.Distance(go.transform.position, actPosition);	
			
			/*double minOut = 0;
			
			Vector3 vc = GetNearestVertex(go, actPosition, out minOut);*/
			
			/*if (minOut < distance)
			{
				distance = minOut;
			}*/
			
			if (distance < min)
			{
				min = distance;	
				closest = go;
			}
		}
		
		return closest;
	}
	
	void Update()
	{
		if (!LoadingCorrupted && Generator.generatorDone && !FileLoaded && SaveFileExist() && !GenerationDone)
		{	
			FileLoaded = LoadFile();
			
			if (FileLoaded == false)
				LoadingCorrupted = true;
			else
			{
				GenerationDone = true;
				Debug.Log ("Waypoints succesfully loaded from file");
				SaveToFile();
				timeFinished = (DateTime.Now.Subtract(Generator.start)).Seconds;
			}
		}
		else if (!GenerationDone && QuadTree.generationDone && Generator.generatorDone /*&& (!SaveFileExist() || LoadingCorrupted)*/)
		{
			Debug.Log("Waypoint generation");
			
			QuadTree qt = GameObject.Find("QuadTreeGenerator").GetComponent<QuadTree>();
			
			float startX = CenterX - Width / 2.0f;
			float startY = CenterY - Width / 2.0f;
			
			float endX = CenterX + Width / 2.0f;
			float endY = CenterY + Width / 2.0f;
			
			
			matrix = new PathNode[(int) Width][];
			
			for (int x = 0; x < Width; x += 1)
			{				
				matrix[x] = new PathNode[(int)Width];
				
				for (int y = 0; y < Width; y += 1)
				{	
					matrix[x][y] = null;
				}
			}
			
			int x1, y1;
			float xf;
			float yf;
			
			for (xf = startX, x1 = 0; xf < endX && x1 < Width; xf += Step, x1++)
			{
				for (yf = startY, y1 = 0; yf < endY && y1 < Width; yf += Step, y1++)
				{
					Vector3 position = new Vector3(xf, 0.5f, yf);
					
					List<GameObject> result = qt.GetObjectsInside(position);
					
					QuadTreeItem qi = qt.GetQuadTreeItemFor(position);
					
					if (qi == null || qi.GameObjects == null)
						continue;
					
					GameObject closest = GetClosestObject(qi, position);
					
					if (IsInside(qi.GameObjects, position))
						continue;
					
					//if (result.Count == 0)
					if (closest == null || (closest != null && Vector3.Distance(closest.transform.position, position) > 1.0f))
					{
						//var tmp = PathNode.Spawn(position, true);	
						
						matrix[x1][y1] = PathNode.Spawn(position, true).GetComponent<PathNode>();
						
						if (x1 > 0 && matrix[x1-1][y1] != null && matrix[x1-1][y1].nodeEnabled)
						{
							matrix[x1][y1].connections.Add(matrix[x1-1][y1]);
							matrix[x1][y1].connectionIndices.Add((x1-1) + (y1 * (int)Width));
						}
						if (x1 < Width-1 && matrix[x1+1][y1] != null && matrix[x1+1][y1].nodeEnabled)
						{							
							matrix[x1][y1].connections.Add(matrix[x1+1][y1]);
							matrix[x1][y1].connectionIndices.Add((x1+1) + (y1 * (int)Width));
						}
						if (y1 > 0 && matrix[x1][y1-1] != null && matrix[x1][y1-1].nodeEnabled)
						{
							matrix[x1][y1].connections.Add(matrix[x1][y1-1]);
							matrix[x1][y1].connectionIndices.Add((x1) + ((y1-1) * (int)Width));
						}
						if (y1 < Width-1 && matrix[x1][y1+1] != null && matrix[x1][y1+1].nodeEnabled)
						{
							matrix[x1][y1].connections.Add(matrix[x1][y1+1]);
							matrix[x1][y1].connectionIndices.Add((x1) + ((y1+1) * (int)Width));
						}
						
						if (x1 > 0 && y1 > 0 && matrix[x1-1][y1-1] != null && matrix[x1-1][y1-1].nodeEnabled)
						{
							matrix[x1][y1].connections.Add(matrix[x1-1][y1-1]);
							matrix[x1][y1].connectionIndices.Add((x1-1) + ((y1-1) * (int)Width));
						}
						if (x1 < Width+1 && y1 > 0 && matrix[x1+1][y1-1] != null && matrix[x1+1][y1-1].nodeEnabled)
						{
							matrix[x1][y1].connections.Add(matrix[x1+1][y1-1]);
							matrix[x1][y1].connectionIndices.Add((x1+1) + ((y1-1) * (int)Width));
						}
						if (x1 < Width+1 && y1 < Width+1 && matrix[x1+1][y1+1] != null && matrix[x1+1][y1+1].nodeEnabled)
						{
							matrix[x1][y1].connections.Add(matrix[x1+1][y1+1]);
							matrix[x1][y1].connectionIndices.Add((x1+1) + ((y1+1) * (int)Width));
						}
						if (x1 > 0 && y1 < Width-1 && matrix[x1-1][y1+1] != null && matrix[x1-1][y1+1].nodeEnabled)
						{
							matrix[x1][y1].connections.Add(matrix[x1-1][y1+1]);
							matrix[x1][y1].connectionIndices.Add((x1-1) + ((y1+1) * (int)Width));
						}
						
						//pathNodes.Add(tmp);
					}
					else
					{
						//Debug.Log ("Nope");	
						matrix[x1][y1] = null;
					}
					
					//Debug.Log ("Result count = " + result.Count);
				}
			}
			
			//SaveToFile();
			
			GenerationDone = true;
			
			timeFinished = (DateTime.Now.Subtract(Generator.start)).Seconds;
		}
	}
}

