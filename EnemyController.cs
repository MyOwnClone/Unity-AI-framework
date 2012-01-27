using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class EnemyController : MonoBehaviour {
	private List<PathNode> path = null;
	
	public PathNode actualTargetNode = null;
	public PathNode actualStartNode = null;
	
	private float actX = 0;
	private float actY = 0;
	
	private int nodeCounter = 0;
	
	public int startIndex = -1;
	
	public PathNode innerStart = null;
	
	private PathNode target = null;
	
	public PathNode start = null, end = null;
	
	public Vector3 startNodePosition = new Vector3(0f, 0f, 0f);
	public Vector3 endNodePosition = new Vector3(0f, 0f, 0f);
	
	private float dx, dy, sx, sy, err, x1, y1, x0, y0;
	
	private bool haveToReset = false;
	
	//public UnityEngine.Object explosion = null;
	
	public UnityEngine.Object parent = null;
	
	public bool pathfindingRequested = false;
	
	public bool waypointsPrepared = false;
	public bool pathForPreparedWaypointsNotFound = false;
	
	public int id = -1;
	
	public int enemiesInSight = 0;
	
	public List<int> enemiesInSightIdList = new List<int>();
	
	public List<EnemyController> enemiesInSightList = new List<EnemyController>();
	
	private int raycastingCounter = 0;
		
	private bool enemyFound = false;
	
	//private bool playerKilled = false;
	
	// Use this for initialization
	void Start () {
		//targetCounterTarget = UnityEngine.Random.Range(5, 15);
	}
	
	List<GameObject> GetGameObjectsOnTheWay(Vector3 start, Vector3 end)
	{
		List<GameObject> result = new List<GameObject>();
		
		QuadTree qt = GameObject.Find("QuadTreeGenerator").GetComponent<QuadTree>();
		
		float dx = end.x - start.x;
		float dy = end.y - start.y;
		
		float lastX = start.x, lastY = y1;
		
		for (float x = start.x; x < end.x; x++) {
		    float y = y1 + (dy) * (x - x1)/(dx);
			
			if (Mathf.Abs(lastX - x) > 1 || Mathf.Abs(lastY - y) > 1)
			{
				QuadTreeItem qti = qt.GetQuadTreeItemFor(new Vector3(x, 0.0f, y), 1);	
					
				result.AddRange(qti.GameObjects);
				
				lastX = x;
				lastY = y;	
			}			
		}	
		
		return result;
	}
	
	public void DoRayCast ()
	{
		/*if (enemyFound && raycastingCounter < 50)
		{
			DrawHelper.DrawCube(new Vector3(actX, 0.0f, actY), new Vector3(2.0f, 2.0f, 2.0f), Color.yellow);
			raycastingCounter++;
			
			foreach (EnemyController ec in enemiesInSightList)
				Debug.DrawLine (this.transform.position, ec.transform.position);
			
			return;
		}
		else
			enemyFound = false;*/
		
		
		QuadTree qt = GameObject.Find("QuadTreeGenerator").GetComponent<QuadTree>();
		
		//List<QuadTreeItem> qtis = qt.GetQuadTreeItemFor(new Vector3(actX, 0.0f, actY), new Vector3(2.5f, 2.5f, 2.5f), 2);
		//QuadTreeItem qti1 = qt.GetQuadTreeItemFor(new Vector3(actX + this.transform.localScale.x/2.0f, 0.0f, actY + this.transform.localScale.z/2.0f), 3);
		//QuadTreeItem qti2 = qt.GetQuadTreeItemFor(new Vector3(actX + this.transform.localScale.x/2.0f, 0.0f, actY - this.transform.localScale.z/2.0f), 3);
		//QuadTreeItem qti3 = qt.GetQuadTreeItemFor(new Vector3(actX - this.transform.localScale.x/2.0f, 0.0f, actY - this.transform.localScale.z/2.0f), 3);
		//QuadTreeItem qti4 = qt.GetQuadTreeItemFor(new Vector3(actX - this.transform.localScale.x/2.0f, 0.0f, actY + this.transform.localScale.z/2.0f), 3);
		
		/*List<GameObject> gos = new List<GameObject>();
		
		foreach (QuadTreeItem qti in qtis)		
			gos.AddRange(qti.GameObjects);*/
		//gos.AddRange(qti1.GameObjects);
		//gos.AddRange(qti2.GameObjects);
		//gos.AddRange(qti3.GameObjects);
		//gos.AddRange(qti4.GameObjects);
		
		List<GameObject> gos = new List<GameObject>(GameObject.FindGameObjectsWithTag ("wall"));//qt.GetQuadTreeItemFor(new Vector3(actX, 0.0f, actY), 1).GameObjects;
		
		List<GameObject> enemies = EnemyGenerator.enemies;
		
		enemiesInSight = 0;
		
		enemiesInSightIdList.Clear();
		enemiesInSightList.Clear();
		
		foreach (GameObject enemyGo1 in enemies)		
		{					
			bool obstacleFound = false;
		
			double distance = Vector3.Distance(enemyGo1.transform.position, this.transform.position);				
		
			if (distance > 15.0f)
				continue;
		
			EnemyController ec1 = enemyGo1.GetComponent<EnemyController>();
		
			/*List<QuadTreeItem> qtis2 = qt.GetQuadTreeItemFor(new Vector3(ec1.actX, 0.0f, ec1.actY), new Vector3(2.5f, 2.5f, 2.5f), 2);
			List<GameObject> gos2 = new List<GameObject>();//qt.GetQuadTreeItemFor(new Vector3(ec1.actX, 0.0f, ec1.actY), 3).GameObjects;//GetGameObjectsOnTheWay(ec1.transform.position, this.transform.position);
			
			foreach (QuadTreeItem qti in qtis2)		
				gos2.AddRange(qti.GameObjects);
			
			List<GameObject> globalGos = new List<GameObject>();
			globalGos.AddRange(gos);
			globalGos.AddRange(gos2);*/
			
			if (this.id == ec1.id)
				continue;
		
			Vector3 lineDir		= ec1.transform.position - this.transform.position;
			float halfLength	= Vector3.Distance(ec1.transform.position, this.transform.position) / 2.0f; 
			Vector3 midpoint	= new Vector3((ec1.transform.position.x + this.transform.position.x)/2.0f, /*(ec1.transform.position.y + this.transform.position.y)/2.0f*/0.3f, (ec1.transform.position.z + this.transform.position.z)/2.0f);
			
			foreach (GameObject go in gos)
			{
				Vector3 goPosition 	= new Vector3(go.transform.position.x, 0.5f, go.transform.position.z);
				
				double distance2 = Vector3.Distance(go.transform.position, this.transform.position);	
				if (distance2 > 15)
					continue;
				
				Vector3 goScale 	= go.transform.localScale;	
				DrawHelper.DrawCube(goPosition, goScale, Color.red);				
			
				if (OOBCollisionDetection.IsRayIntersectingBox(ec1.transform.position, this.transform.position, goPosition, goScale))
				{
					obstacleFound = true;
					break;
				}
			}
		
			if (!obstacleFound)
			{					
				Debug.DrawLine(ec1.transform.position , this.transform.position);
				enemiesInSight++;		
				enemiesInSightIdList.Add(ec1.id);
				enemiesInSightList.Add(ec1);
				
				enemyFound = true;
				raycastingCounter = 0;
			}
		}
	}

	void ResetMovement ()
	{
		x0 = actualStartNode.Position.x;
		y0 = actualStartNode.Position.z;
		
		x1 = actualTargetNode.Position.x;
		y1 = actualTargetNode.Position.z;
				
		dx = Mathf.Abs(x1-x0);
		dy = Mathf.Abs(y1-y0) ;
		
		sx = 0;
		sy = 0;
		
		if (x0 < x1)
			sx = 1;
		else 
			sx = -1;
						
		if (y0 < y1) 
			sy = 1; 
		else 
			sy = -1;
		
		err = dx - dy;
	}
	
	//returns -1 when to the left, 1 to the right, and 0 for forward/backward
	float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
		var perp = Vector3.Cross(fwd, targetDir);
		var dir  = Vector3.Dot(perp, up);
		
		if (dir > 0.0f) {
			return 1.0f;
		} else if (dir < 0.0f) {
			return -1.0f;
		} else {
			return 0.0f;
		}
	}
	
	float ContAngle(Vector3 fwd, Vector3 targetDir, Vector3 upDir) {
		var angle = Vector3.Angle(fwd, targetDir);
		
		//The AngleDir function is the one from the other thread.
		if (AngleDir(fwd, targetDir, upDir) == -1) {
			return 360 - angle;
		} else {
			return angle;
		}
	}

	void RotateByPath ()
	{
		this.transform.eulerAngles = new Vector3(
		    this.transform.eulerAngles.x,
		    ContAngle(this.transform.forward, actualTargetNode.Position - actualStartNode.Position, new Vector3(0.0f, 1.0f, 0.0f)),
		    this.transform.eulerAngles.z
		);
	}

	public void DestroySelf ()
	{
		//((GameObject) explosion).transform.position = player.transform.position;
		
		//((GameObject) explosion).GetComponentInChildren<Detonator>().transform.position = this.transform.position;
		//((GameObject) explosion).GetComponentInChildren<Detonator>().Explode();
		
		//this.transform.position = player.transform.position;
		//((GameObject) this.parent).transform.position = player.transform.position;
		//((GameObject) this.parent).GetComponentInChildren<MeshRenderer>().enabled = false;
		
		//EnemyGenerator.enemies.Remove((GameObject)this.parent);
		
		//Destroy(parent, 0.2f);
	}
	
	public void WaypointSystemChangedCallback()
	{		
		WaypointManager wg = GameObject.Find ("WaypointManager").GetComponent<WaypointManager>();
		var sources = wg.pathNodes;	
		int startIndex = -1;
		int endIndex = -1;
						
		if (actualStartNode == null || !actualStartNode.nodeValid)
		{
			startIndex = PathfindingManager.Closest(sources, startNodePosition);
			actualStartNode = sources[startIndex];
		}
		if (actualTargetNode == null || !actualTargetNode.nodeValid)
		{
			endIndex = PathfindingManager.Closest(sources, endNodePosition);
			actualTargetNode = sources[endIndex];
		}
		
		bool found = false;
		
		if (path != null)
		{
			//foreach (PathNode pn in path)
			for (int i = 0; i < path.Count; i++)
			{
				PathNode pn = path[i];
				
				if (pn == null || !pn.nodeEnabled || !pn.nodeValid)
				{
					found = true;
					break;
				}
			}
			if (found)
				path.Clear();
		}		
	}
	
	private bool PrepareStartEndWaypoints (ref PathNode start, ref PathNode end)
	{			
		GameObject wg = GameObject.Find("WaypointManager");
		
		if (wg == null)
		{
			return false;
		}
		
		var sources = wg.GetComponent<WaypointManager>().pathNodes;			
		if (sources == null || sources.Count == 0)
			return false;
		
		int counter = 0;						
		//Debug.Log (this.name + ": Generating start and end waypoints");
		do {
			int index1 = UnityEngine.Random.Range(0, sources.Count-1);
			int index2 = UnityEngine.Random.Range(0, sources.Count-1);				
			
			if (startIndex != -1)
				start = sources[startIndex];
			else
				start = sources[index1];
			
			if (target == null)
				end = sources[index2];
			else
				end = target;
			
			counter++;
		} while ((start.connections.Count < 3 || end.connections.Count < 2) && counter <= 3/* && Vector3.Distance(start.position, end.position) > 10f*/);			
		
		if (counter == 2 && (start.connections.Count < 3 || end.connections.Count < 3))
		{
			//Debug.Log ("Waypoints invalid");
			start = null;
			end = null;
			
			return false;
		}
		
		//Debug.Log ("start and end generated");
		
		if (innerStart != null)
		{
			start = innerStart;	
		}
		
		//Debug.Log ("Pathfinder query");
		return true;
	}
	
	
	// Update is called once per frame
	void Update () {	
		GameObject go = GameObject.Find("PathfindingManager");	
		
		if (path != null && path.Count > 1 && ((actualStartNode != null && actualTargetNode != null) && (actualStartNode.position != actualTargetNode.position)))
		{
			if (actualStartNode == null && actualTargetNode == null && path.Count >= 2)
			{
				actualStartNode = path[0];
				actualTargetNode = path[1];
				
				startNodePosition = actualStartNode.position;
				endNodePosition = actualTargetNode.position;
				
				this.transform.position = actualStartNode.Position;	
				nodeCounter = 0;
				actX = (int) actualTargetNode.Position.x;
				actY = (int) actualTargetNode.Position.z;
				
				RotateByPath ();
				
				haveToReset = true;
				
				//Debug.Log ("actStartNode = null, actTargetNode = null, path exists");
			}
			else if (actualTargetNode != null && Vector3.Distance(new Vector3(actX, this.transform.position.y, actY), actualTargetNode.Position) <= 0.6f)
			{
				actX = (float) actualTargetNode.Position.x;
				actY = (float) actualTargetNode.Position.z;
				
				nodeCounter++;
				
				//Debug.Log ("assigning new target node");
				
				if (nodeCounter + 1 < path.Count)
				{
					actualStartNode = actualTargetNode;
					actualTargetNode = path[nodeCounter+1];
					
					startNodePosition = actualStartNode.position;
					endNodePosition = actualTargetNode.position;
					
					RotateByPath ();
					
					haveToReset = true;
					//Debug.Log ("new target node assigned");
				}
				else
				{					
					innerStart = actualTargetNode;
										
					actualStartNode = null;
					actualTargetNode = null;
					
					start = null;
					end = null;
					
					path.Clear();
					
					//Debug.Log ("no more nodes, path cleared");
				}					
			}
			else if (actualTargetNode != null) {	
				//Debug.Log ("actTargetNode != null, bresenham");
				
				if (haveToReset)
				{
					ResetMovement();
					
					haveToReset = false;
				}
								
				float e2 = 2 * err;
				
				if (e2 > -dy)
				{
				   err = err - dy;
				   actX = actX + (sx*0.1f);//*(Time.deltaTime*3f));
				}
				
				if (e2 < dx)
				{
					err = err + dx;
					actY = actY + (sy*0.1f);//*(Time.deltaTime*3f));
				}
				
				this.transform.position = new Vector3(actX, this.transform.position.y, actY);
			}
			else
			{
				actualStartNode = null;
				actualTargetNode = null;
				
				start = null;
				end = null;
			}
		}
		else
		{						
			PathNode newStart = null;
			PathNode newEnd = null;
		
			if (!waypointsPrepared && PrepareStartEndWaypoints (ref newStart, ref newEnd))
			{
				start = newStart;
				end = newEnd;
				
				pathForPreparedWaypointsNotFound = false;
				
				waypointsPrepared = true;
			}
			
			if (pathForPreparedWaypointsNotFound && PrepareStartEndWaypoints (ref newStart, ref newEnd))
			{
				start = newStart;
				end = newEnd;
				
				pathForPreparedWaypointsNotFound = false;
				
				waypointsPrepared = true;
			}
			
			//FpsCounter.pathfindingsCounter++;
			
			if (waypointsPrepared && !pathfindingRequested)
			{
				//path = go.GetComponent<PathfindingManager>().GetPath(start, end);
				
				bool result = false;
				
				path = go.GetComponent<PathfindingManager>().RequestPath(new PathfindingRequest(id, start, end), ref result);
				
				if (!result)
				{
					//Debug.Log (this.name + ": Request not answered");
				}
				else if (path != null && path.Count > 1 )
				{		
					//Debug.Log ("Path found");
					//Debug.Log (this.name + ": Pathfinder query: found");
					actualStartNode = path[0];
					actualTargetNode = path[1];
					startNodePosition = actualStartNode.position;
					endNodePosition = actualTargetNode.position;
					
					this.transform.position = new Vector3(actualStartNode.Position.x, 0.5f, actualStartNode.Position.z );
					
					nodeCounter = 0;
					actX = (int) actualStartNode.Position.x;
					actY = (int) actualStartNode.Position.z;
					
					haveToReset = true;
					
					waypointsPrepared = false;
					
					RotateByPath ();
				}
				else
				{							
					pathForPreparedWaypointsNotFound = true;
				}
			}
		}
		
	}
}
