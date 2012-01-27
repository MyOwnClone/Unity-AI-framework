#define DEBUG_WAYPOINT
#undef DEBUG_WAYPOINT

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if (DEBUG_WAYPOINT)
public class PathNode : MonoBehaviour, IPathNode<PathNode>
#else
public class PathNode : IPathNode<PathNode>	
#endif
{
    public List<PathNode> connections;
    	
	public GameObject innerObj = null;
	
	public bool nodeEnabled = false;
	
	public static int startNode = -1;
	public static int endNode = -1;
	
	public int xPos = -1;
	public int yPos = -1;
	
	public bool nodeValid = true;
	
	public Vector3 position;
	
	private static int nodeCounter = 0;
	
	public int Id {
		get;
		set;
	}
	
	public HashSet<int> ConnectionSet = new HashSet<int>();
    
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
	
	public void AddConnection(PathNode other)
	{
		if (!ConnectionSet.Contains(other.Id))
			connections.Add(other);
	}

#if (DEBUG_WAYPOINT)
    public void Update()
    {
		if (!nodeEnabled)
			return;
		
        DrawHelper.DrawCube(transform.position, Vector3.one, Color.red );
        if(connections == null)
            return;
        for(int i = 0; i < connections.Count; i++)
        {
            if(connections[i] == null)
                continue;
            Debug.DrawLine(transform.position, connections[i].Position, Color.red);
        }
    }
    
    public void Awake()
    {
        if(connections == null)
		{
            connections = new List<PathNode>();
		}
    }
#else
	public string name = string.Empty;
	
#endif
	
	public PathNode()
	{
		if(connections == null)
		{
            connections = new List<PathNode>();		
		}
	}
    
    public static PathNode Spawn(Vector3 inPosition, bool nodeEnabled, string suffix)
    {
#if (DEBUG_WAYPOINT)
        GameObject obj = null;
		obj = new GameObject("PathNode_" + nodeCounter + " " + suffix);
		obj.name = "PathNode_" + nodeCounter + " " + inPosition.x + ", " + inPosition.z + " " + suffix + ", enabled = " + nodeEnabled;
		obj.AddComponent<PathNode>();
		obj.transform.position = inPosition;obj.AddComponent<PathNode>();
		obj.GetComponent<PathNode>().innerObj = obj;
		
		obj.GetComponent<PathNode>().nodeEnabled = nodeEnabled;
		obj.GetComponent<PathNode>().nodeValid = true;
		obj.GetComponent<PathNode>().Position = inPosition;
		obj.GetComponent<PathNode>().Id = nodeCounter;
		
		nodeCounter++;
		
		return obj.GetComponent<PathNode>();
#else				
		PathNode newNode = new PathNode();
		newNode.name = "PathNode_" + nodeCounter + " " + inPosition.x + ", " + inPosition.z + " " + suffix + ", enabled = " + nodeEnabled;
		newNode.position = inPosition;
		newNode.nodeEnabled = nodeEnabled;
		newNode.nodeValid = true;
		
		nodeCounter++;
        return newNode;
#endif
    }
}
