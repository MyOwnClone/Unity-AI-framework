using UnityEngine;
using System.Collections;

public class FpsCounter : MonoBehaviour {
    public float updateInterval = 0.5F;
    private float lastInterval;
    private int frames = 0;
    private float fps;
	
    void Start() {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
	
    void OnGUI() {
        GUI.Label(new Rect(1, 1, 100, 28), "FPS: " + fps.ToString(), "box");
    }
	
    void Update() {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + 1) {
            fps = frames;
            frames = 0;
            lastInterval = timeNow;
        }
    }
}