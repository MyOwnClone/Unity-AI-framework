using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode : MonoBehaviour, IPathNode<PathNode>
{
    public List<PathNode> connections;
	public List<int> connectionIndices;
    	
	public GameObject innerObj = null;
	
	public bool nodeEnabled = false;
	
	public static int startNode = -1;
	public static int endNode = -1;
	
	public int xPos = -1;
	public int yPos = -1;
	
	public Vector3 position;
	
	private static int nodeCounter = 0;
    
    public bool Invalid
    {
        get { return (this == null); }
    }
    
    public List<PathNode> Connections
    {
        get { return connections; }
    }
    
    public Vector3 Position
    {
		get
        {
            return position;
        }
		set
		{
			position = value;	
		}
    }
    
    public void Update()
    {
		if (!nodeEnabled)
			return;
		
        DrawHelper.DrawCube(transform.position/*Position*/, Vector3.one, Color.red );
        if(connections == null)
            return;
        for(int i = 0; i < connections.Count; i++)
        {
            if(connections[i] == null)
                continue;
            Debug.DrawLine(transform.position/*Position*/, connections[i].Position, Color.red);
        }
    }
    
    public void Awake()
    {
        if(connections == null)
		{
            connections = new List<PathNode>();
        	connectionIndices = new List<int>();
		}
    }
	
	public PathNode()
	{
		if(connections == null)
		{
            connections = new List<PathNode>();		
			connectionIndices = new List<int>();
		}
	}
    
    public static GameObject Spawn(Vector3 inPosition, bool nodeEnabled)
    {

        GameObject obj = null;
		obj = new GameObject("PathNode_" + nodeCounter);
		//obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//obj.name = "PathNode_" + nodeCounter;
		
		/*PathNode newNode = new PathNode();//*/obj.AddComponent<PathNode>();
		
		obj.transform.position = inPosition;
		//newNode.position = inPosition;
		
		/*newNode*/obj.GetComponent<PathNode>().innerObj = obj;
		
		obj.GetComponent<PathNode>().nodeEnabled = nodeEnabled;
		obj.GetComponent<PathNode>().Position = inPosition;
		
		nodeCounter++;
		
        return obj;
    }
	
	/*public GameObject GetInnerObject()
	{
		return innerObj;	
	}*/
        
    public static List<PathNode> CreateGrid(Vector3 center, Vector3 spacing, int[] dim, float randomSpace)
    {
        GameObject groupObject = new GameObject("grid");
        Random.seed = 1337;
        int xCount = dim[0];
        int yCount = dim[1];
        float xWidth = spacing.x * xCount;
        float yWidth = spacing.z * yCount;
		
		GameObject go = GameObject.FindGameObjectWithTag("LevelGenerator");
		
		int cols = go.GetComponent<Generator>().cols;
		int rows = go.GetComponent<Generator>().rows;
        
		float startX 	= cols;
		float startZ 	= rows;
		
        List<PathNode> result = new List<PathNode>();
		
		if (Generator.map == null || Generator.generatorDone == false)
			return null;
        
		int counter = 0;
		
        for(int col = 0; col < xCount; col++)
        {            
            for(int row = 0; row < yCount; row++)
            {
				bool nodeEnabled = false;
				
				if (Generator.map[col][row] == 0)
				{
					nodeEnabled = true;
					endNode = counter;
					
					if (startNode == -1)
						startNode = counter;
				}
				else
				{
					nodeEnabled = false;
				}
				
                /*GameObject newNode = Spawn(new Vector3(startX - (col * 2), center.y, startZ - (row * 2)), nodeEnabled);
				
				newNode.nodeEnabled = nodeEnabled;
				
				newNode.xPos = col;
				newNode.yPos = row;
				
                result.Add(newNode);
				counter++;*/
            
            }
        }			
        
        for(int x = 0; x < xCount; x++)
        {       
            for(int y = 0; y < yCount; y++)
            {
				if (Generator.map[x][y] != 0)
					continue;
				
                int thisIndex = (x * yCount) + y;
                List<int> connectedIndicies = new List<int>();
				
				if (thisIndex >= result.Count)
					continue;
				
                PathNode thisNode = result[thisIndex];
                if(AStarHelper.Invalid(thisNode)) continue;
				
				int x1 = x, y1 = y;
				                
				if(x != 0 && Generator.map[x1-1][y] == 0)
                    connectedIndicies.Add(((x - 1) * yCount) + y);
				
                if(x != xCount - 1 && Generator.map[x1+1][y1] == 0)
                    connectedIndicies.Add(((x + 1) * yCount) + y);
				
                if(y != 0 && Generator.map[x1][y1-1] == 0)
                    connectedIndicies.Add((x * yCount) + (y - 1));
				
                if(y != yCount - 1 && Generator.map[x1][y1+1] == 0)
                    connectedIndicies.Add((x * yCount) + (y + 1));
                
                if(x != 0 && y != 0 && Generator.map[x1-1][y1-1] == 0)
                    connectedIndicies.Add(((x - 1) * yCount) + (y - 1));
                
                if(x != xCount - 1 && y != yCount - 1 && Generator.map[x1+1][y1+1] == 0)
                    connectedIndicies.Add(((x + 1) * yCount) + (y + 1));
                
                if(x != 0 && y != yCount - 1 && Generator.map[x1-1][y1+1] == 0)
                    connectedIndicies.Add(((x - 1) * yCount) + (y + 1));
                
                if(x != xCount - 1 && y != 0 && Generator.map[x1+1][y1-1] == 0)
                    connectedIndicies.Add(((x + 1) * yCount) + (y - 1));
				
                for(int i = 0; i < connectedIndicies.Count; i++)
                {
					int index = connectedIndicies[i];
					
					if (index >= result.Count)
						continue;
					
                    PathNode thisConnection = result[index];
                    if(AStarHelper.Invalid(thisConnection))
                        continue;
                    thisNode.Connections.Add(thisConnection);
                }
                
            }
        }
        
        return result;        
    }
}
