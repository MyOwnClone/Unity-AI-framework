using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class PathNodeTester : MonoBehaviour
{
    public static List<PathNode> sources;
    //public GameObject start;
    //public GameObject end;
    /*public Color nodeColor = new Color(0.05f, 0.3f, 0.05f, 0.1f);
    public Color pulseColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    public Color pathColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);*/
    public bool reset;
    
    public bool gridCreated;
    int startIndex;
    int endIndex;
    
    int lastEndIndex;
    int lastStartIndex;
	
	bool playerPositionSet = false;
    
    //bool donePath = false;
    public List<PathNode> solvedPath = new List<PathNode>();

    public void InitGrid()
	{
		if(gridCreated)
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
			Vector3 newPos = sources[PathNode.startNode]./*transform.*/position + new Vector3(0.0f, 1.0f, 0.0f);
			
			//if (!float.IsNaN(newPos.x) && !float.IsNaN(newPos.y) && !float.IsNaN(newPos.z) )
			//{
				player.transform.position = newPos;	
				
				playerPositionSet = true;
			//}
		}
	}
	
	/*void OnGUI()
	{
		if(!gridCreated)
			GUI.Label(new Rect(Screen.width / 2, (Screen.height / 2) + 50, 200, 30), "Generating AI waypoints", "box");
	}*/
    
    public void Awake()
    {    
        InitGrid();        
    }
    
    /*public void PulsePoint(int index)
    {
        if(AStarHelper.Invalid(sources[index]))
            return;
        DrawHelper.DrawCube(sources[index].Position, Vector3.one * 2.0f, pulseColor);
    }*/
    
    
    public void Draw(int startPoint, int endPoint, Color inColor)
    {
        Debug.DrawLine(sources[startPoint].Position, sources[endPoint].Position, inColor);
    }
    
    public static int Closest(List<PathNode> inNodes, Vector3 toPoint)
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
    }
	
	/*public static List<PathNode> GetSources()
	{
		return sources;
	}*/
    
	public List<PathNode> GetPath(/*GameObject*/PathNode start, /*GameObject*/PathNode end)
	{		
		if(start == null || end == null)
        {
			if (sources != null && sources.Count >= 2)
			{			
				start 	= sources[PathNode.startNode];//.GetInnerObject();
				end 	= sources[PathNode.endNode];//.GetInnerObject();			
			}
			
			if (start == null || end == null)
			{
	            Debug.LogWarning("Need 'start' and or 'end' defined!");
	            enabled = false;
	            return null;
			}
        }
		
		startIndex = Closest(sources, start/*.transform*/.position);
    
        endIndex = Closest(sources, end/*.transform*/.position);
		
		return AStarHelper.Calculate(sources[startIndex], sources[endIndex]);
	}
    
    public void Update()
    {
		InitGrid();
		
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
