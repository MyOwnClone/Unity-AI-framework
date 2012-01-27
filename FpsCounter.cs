using UnityEngine;
using System.Collections;

public class FpsCounter : MonoBehaviour {
    public float updateInterval = 0.5F;
    private float lastInterval;
    private int frames = 0;
    private float fps;
	
	private int displayCounter = 0;
	public int displayCounterTarget = 5;
	
	public static int pathfindingsCounter = 0;
	
	public static int pathfindingsMax = int.MinValue;
	
	private int maxPFSFps = 0;
	private int lowFPS = int.MaxValue;
	
	public bool first = true;
	public bool firstFps = true;
	public bool firstMPF = true;
	
	public static long pathfindingTime = 0;
	public long maxPathFindingTime = 0;
	
    void Start() {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
	
    void OnGUI() {
        GUI.Label(new Rect(1, 1, 600, 28), "FPS: " + fps.ToString() + " lowest FPS: " + lowFPS + " PFS: " + pathfindingsCounter + " Max PFS: " + pathfindingsMax + " FPS with Max PFS: " + maxPFSFps + " MAX PF TIME: " + maxPathFindingTime, "box");
    }
	
    void Update() {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + 1) {
            fps = frames;
            frames = 0;
            lastInterval = timeNow;
			
			displayCounter++;
			
			if (displayCounter >= displayCounterTarget)
			{				
				displayCounter = 0;
			}
			
			if (pathfindingsMax < pathfindingsCounter)
			{
				if (!first)
				{
					pathfindingsMax = pathfindingsCounter;
					maxPFSFps = (int)fps;
				}
				else
					first = false;
			}
			
			if (lowFPS > fps)
			{
				if (!firstFps)
				{	
					lowFPS = (int) fps;	
				}
				else
					firstFps = false;
			}
			
			if (maxPathFindingTime < pathfindingTime)
			{
				if (!firstMPF)
				{	
					maxPathFindingTime = pathfindingTime;
				}
				else
					firstMPF = false;
			}
			
			pathfindingsCounter = 0;
        }
    }
}