using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

	public class EnemyGenerator : MonoBehaviour
	{
		public int numberToGenerate = 0;
	
		public UnityEngine.Object enemyPrefab = null;
	
		//public UnityEngine.Object explosionPrefab = null;
		
		private bool isGenerated = false;
	
		public bool raycastingEnabled = false;
	
		public static float startTime = -1;
	
		//public int timeSpan = 20;
		
		void Start()
		{			
			enemies = new List<GameObject>();
		}
	
		public static List<GameObject> enemies;
	
		public void WaypointSystemChangedCallback()
		{
			if (enemies != null && enemies.Count > 0)
			{
				foreach (GameObject go in enemies)
				{
					EnemyController ec = go.GetComponent<EnemyController>();
					ec.WaypointSystemChangedCallback();
				}
			}
		}
	
		bool Generate ()
		{
			//GameObject player = GameObject.FindGameObjectWithTag("Player");
			//Debug.Log ("Generating enemies");
		
			GameObject wg = GameObject.Find ("WaypointManager");
		
			if (wg == null)
			{
				Debug.Log ("WaypointGenerator not found");	
			}
		
			var sources = wg.GetComponent<WaypointManager>().pathNodes;
		
			if (sources == null || sources.Count == 0)
				return false;
		
			int index1 = 0;
		
			int counter = 0;
		
			do {
				index1 = UnityEngine.Random.Range(0, sources.Count-1);
			} while (sources[index1]/*.GetComponent<PathNode>()*/.connections.Count >= 3 /*&& Vector3.Distance(PathNodeTester.sources[index1].position, player.transform.position) < 8*/);
				
		
				for (int i = 0; i < numberToGenerate; i++)
				{
					//GameObject go = new GameObject("enemy"+i); /*GameObject.CreatePrimitive(PrimitiveType.Sphere)*/;
					
					/*GameObject enemyBot = GameObject.FindGameObjectWithTag("EnemyBot");
				
					MeshFilter[] filters = enemyBot.GetComponentsInChildren<MeshFilter>();
				
					int counter = 0;
				
					foreach (var filter in filters)
					{
						var mesh = filter.mesh;
					
						GameObject child = new GameObject("child" + counter);
					
						child.AddComponent<MeshFilter>();
						child.GetComponent<MeshFilter>().mesh = mesh;
					
						child.transform.parent = go.transform;
					
						counter++;
					}*/
				
					GameObject go = (GameObject) Instantiate(/*GameObject.FindGameObjectWithTag("EnemyBot")*/enemyPrefab, new Vector3(0.0f, 0.5f, 0.0f), Quaternion.identity);
				
					//go.GetComponent<MeshFilter>().mesh = enemyBot.GetComponent<MeshFilter>().sharedMesh;
				
					go.name = "enemy_" + counter;
					go.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
					//go.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
					go.transform.position = sources[index1]./*GetComponent<PathNode>().*/Position + new Vector3(0f, 0.5f, 0f);
				
					go.transform.eulerAngles = new Vector3(270, 0, 0);
					
					go.AddComponent<EnemyController>();
				
					//GameObject explosion = (GameObject) Instantiate(explosionPrefab, new Vector3(0.0f, 0.5f, 0.0f), Quaternion.identity);
				
					//explosion.transform.parent = go.transform;
				
					//go.GetComponent<EnemyController>().explosion = explosion;
					go.GetComponent<EnemyController>().parent = go;
					go.GetComponent<EnemyController>().id = i;
					go.GetComponent<BoxCollider>().enabled = false;
					//go.GetComponent<EnemyController>().startIndex = index1;
			
					enemies.Add(go);
					counter++;
				}
			return true;
		}
	
		/*void OnGUI()
		{
			if (startTime != -1)
			{
				int togo = (int) startTime - (int)Time.timeSinceLevelLoad;
			
				GUI.Label(new Rect(1, 60, 200, 30), "Next wave in: " + togo, "box");
			}
		}*/
		
		void Update()
		{
			/*if (Time.timeSinceLevelLoad >= startTime)
			{
				if (!Generate())
					return;
			
				startTime = Time.timeSinceLevelLoad + timeSpan;
			}*/
		
			if (raycastingEnabled)
			{
				if (enemies != null && enemies.Count > 0)
				{
					foreach (GameObject go in enemies)	
						go.GetComponent<EnemyController>().DoRayCast();
				}	
			}
		
			if (!isGenerated && Generator.generatorDone == true)
			{
				if (!Generate())
					return;
				
				isGenerated = true;
			
				/*if (startTime == -1)
					startTime = Time.timeSinceLevelLoad + timeSpan;*/
			}
		}
	}

