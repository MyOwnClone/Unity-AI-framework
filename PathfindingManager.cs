using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using System.Diagnostics;

public class PathfindingRequest
{
	public PathNode Start
	{
		get {
			return start;	
		}
		
		set {
			start = value;	
		}
	}
	
	public PathNode End
	{
		get {
			return end;	
		}
		
		set {
			end = value;	
		}
	}
	
	public int Id
	{
		get {
			return id;	
		}
		
		set {
			id = value;	
		}
	}
	
	PathNode start;
	PathNode end;
	int id;
	
	public PathfindingRequest(int p_id, PathNode p_start, PathNode p_end)
	{
		this.start = p_start;
		this.end = p_end;
		this.id = p_id;
	}
	
	public PathfindingRequest(PathfindingRequest _ref)
	{
		this.start = _ref.start;
		this.end = _ref.end;
		this.id = _ref.id;	
	}
}

public class PathfindingManager : MonoBehaviour
{
    //public static List<PathNode> sources;
    //public GameObject start;
    //public GameObject end;
    /*public Color nodeColor = new Color(0.05f, 0.3f, 0.05f, 0.1f);
    public Color pulseColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    public Color pathColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);*/
    public bool reset;
    
	private List<PathfindingRequest> requests = new List<PathfindingRequest>();
	
    public bool gridCreated;
	
	public int maxPathFindingPerSecond = 20;
	
    int startIndex;
    int endIndex;
    
    int lastEndIndex;
    int lastStartIndex;
	
	//bool playerPositionSet = false;
    
    //bool donePath = false;
    public List<PathNode> solvedPath = new List<PathNode>();

    public void InitGrid()
	{
		/*if(gridCreated)
            return;
		
		GameObject go = GameObject.FindGameObjectWithTag("LevelGenerator");
		
		int cols = go.GetComponent<Generator>().cols;
		int rows = go.GetComponent<Generator>().rows;
		
        sources = PathNode.CreateGrid(new Vector3(0, 0.5f, 0), Vector3.one * 2.0f, new int[] { cols, rows}, 0.0f);
		
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		
		if (sources != null)
		{			
        	gridCreated = true;
		}
		
		if (!playerPositionSet && sources != null)
		{	
			Vector3 newPos = sources[PathNode.startNode].position + new Vector3(0.0f, 1.0f, 0.0f);			
			
			player.transform.position = newPos;	
			
			playerPositionSet = true;
		}*/
	}
	
	/*void OnGUI()
	{
		if(!gridCreated)
			GUI.Label(new Rect(Screen.width / 2, (Screen.height / 2) + 50, 200, 30), "Generating AI waypoints", "box");
	}*/
    
    public void Awake()
    {    
        //InitGrid();        
    }
    
    /*public void PulsePoint(int index)
    {
        if(AStarHelper.Invalid(sources[index]))
            return;
        DrawHelper.DrawCube(sources[index].Position, Vector3.one * 2.0f, pulseColor);
    }*/
    
    
    public void Draw(int startPoint, int endPoint, Color inColor)
    {
		WaypointManager wg = GameObject.Find ("WaypointManager").GetComponent<WaypointManager>();
		var sources = wg.pathNodes;		
		
        Debug.DrawLine(sources[startPoint]./*GetComponent<PathNode>().*/Position, sources[endPoint]/*.GetComponent<PathNode>()*/.Position, inColor);
    }
	
	public static int Closest(List</*GameObject*/PathNode> inNodes, Vector3 toPoint)
    {
        int closestIndex = 0;
        float minDist = float.MaxValue;
        for(int i = 0; i < inNodes.Count; i++)
        {
            if(AStarHelper.Invalid(inNodes[i]/*.GetComponent<PathNode>()*/))
                continue;
            float thisDist = Vector3.Distance(toPoint, inNodes[i]/*.GetComponent<PathNode>()*/.Position);
            if(thisDist > minDist)
                continue;
            
            minDist = thisDist;
            closestIndex = i;
        }
		
        if (minDist != float.MaxValue)
        	return closestIndex;
		else
			return -1;
    }
    
    /*public static int Closest(List<PathNode> inNodes, Vector3 toPoint)
    {
        int closestIndex = 0;
        float minDist = float.MaxValue;
        for(int i = 0; i < inNodes.Count; i++)
        {
            if(AStarHelper.Invalid(inNodes[i]))
                continue;
            float thisDist = Vector3.Distance(toPoint, inNodes[i].Position);
            if(thisDist > minDist)
                continue;
            
            minDist = thisDist;
            closestIndex = i;
        }
		
        if (minDist != float.MaxValue)
        	return closestIndex;
		else
			return -1;
    }*/
	
	/*public static List<PathNode> GetSources()
	{
		return sources;
	}*/
    
	PathfindingRequest FindRequest(PathfindingRequest req)
	{
		foreach (PathfindingRequest pr in requests)	
			if (req.Id == pr.Id)
				return req;
		
		return null;
	}
	
	public List<PathNode> RequestPath(PathfindingRequest req, ref bool result)
	{		
		List<PathNode> pathResult = null;
		
		//float time1 = 0;
		//float time2 = 0;
		
		//DateTime dt1 = DateTime.Now;
		//DateTime dt2 = DateTime.Now;
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		
		if (FindRequest(req) != null && FpsCounter.pathfindingsCounter <= maxPathFindingPerSecond)
		{
			FpsCounter.pathfindingsCounter++;
			
			requests.Remove(req);
			
			result = true;
			
			sw.Start();		
			pathResult = GetPath(req.Start, req.End);	
			sw.Stop();
		}
		else if (requests.Count == 0 && FpsCounter.pathfindingsCounter <= maxPathFindingPerSecond)
		{
			FpsCounter.pathfindingsCounter++;
			
			result = true;
			
			sw.Start();
			pathResult = GetPath(req.Start, req.End);	
			sw.Stop();
		}
		else
		{
			result = false;
			
			requests.Add(req);
			
			pathResult = null;
		}
		
		if (result)
		{
			//Debug.Log ("Pathfinding time: " + (sw.ElapsedMilliseconds) + " ms");
			FpsCounter.pathfindingTime = sw.ElapsedMilliseconds;
		}
		
		return pathResult;
	}
	
	public List<PathNode> GetPath(PathNode start, PathNode end)
	{	
		WaypointManager wg = GameObject.Find ("WaypointManager").GetComponent<WaypointManager>();
		var sources = wg.pathNodes;		
		
		if(start == null || end == null)
        {
			if (sources != null && sources.Count >= 2)
			{			
				start 	= sources[PathNode.startNode]/*.GetComponent<PathNode>()*/;//.GetInnerObject();
				end 	= sources[PathNode.endNode]/*.GetComponent<PathNode>()*/;//.GetInnerObject();			
			}
			
			if (start == null || end == null)
			{
	            Debug.LogWarning("Need 'start' and or 'end' defined!");
	            enabled = false;
	            return null;
			}
        }
		
		startIndex = Closest(sources, start.position);
    
        endIndex = Closest(sources, end.position);
		
		return AStarHelper.Calculate(sources[startIndex]/*.GetComponent<PathNode>()*/, sources[endIndex]/*.GetComponent<PathNode>()*/);
	}
    
    public void Update()
    {
		//InitGrid();
		
        /*if(reset)
        {
            donePath = false;
            solvedPath.Clear();
            reset = false;
        }
        
        if(start == null || end == null)
        {
			if (sources != null && sources.Count >= 2)
			{			
				start 	= sources[PathNode.startNode].GetInnerObject();
				end 	= sources[PathNode.endNode].GetInnerObject();			
			}
			
			if (start == null || end == null)
			{
	            Debug.LogWarning("Need 'start' and or 'end' defined!");
	            enabled = false;
	            return;
			}
        }
        
        startIndex = Closest(sources, start.transform.position);
    
        endIndex = Closest(sources, end.transform.position);
        
        
        if(startIndex != lastStartIndex || endIndex != lastEndIndex)
        {
            reset = true;
            lastStartIndex = startIndex;
            lastEndIndex = endIndex;
            return;
        }
        
        for(int i = 0; i < sources.Count; i++)
        {
            if(AStarHelper.Invalid(sources[i]))
                continue;
            sources[i].nodeColor = nodeColor;
        }
        
        PulsePoint(lastStartIndex);
        PulsePoint(lastEndIndex);
        
        
        if(!donePath)
        {
                        
            solvedPath = AStarHelper.Calculate(sources[lastStartIndex], sources[lastEndIndex]);
            
			//GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
			
			//enemy.GetComponent<EnemyController>().path = solvedPath;
			//enemy.E
			
            donePath = true;
        }
        
        // Invalid path
        if(solvedPath == null || solvedPath.Count < 1)
        {
            Debug.LogWarning("Invalid path!");
            reset = true;
            enabled = false;
            return;
        }
        
        
        // Draw path 
        for(int i = 0; i < solvedPath.Count - 1; i++)
        {
            if(AStarHelper.Invalid(solvedPath[i]) || AStarHelper.Invalid(solvedPath[i + 1]))
            {
                reset = true;
                
                return;
            }
            Debug.DrawLine(solvedPath[i].Position + new Vector3(0, 3, 0), solvedPath[i + 1].Position + new Vector3(0, 3, 0), Color.cyan * new Color(0.0f, 0.0f, 1.0f, 0.5f)); 
        }*/   
    }

}
