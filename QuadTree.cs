using System;
using System.Collections.Generic;
using UnityEngine;
	
	public class ObjectInfo 
	{
		public Vector3 scale = new Vector3(0f, 0f, 0f);
		public Vector3 position = new Vector3(0f, 0f, 0f);
		public string name = string.Empty;
		public bool moved = false;
		public Vector3 oldPosition = new Vector3(0f, 0f, 0f);
	
		public ObjectInfo(Vector3 position, Vector3 scale)
		{	
			this.position = position;
			this.scale = scale;	
		}
	
		public ObjectInfo(Vector3 position, Vector3 scale, string name)
		{	
			this.position = position;
			this.scale = scale;	
			this.name = name;
		}
	
		public ObjectInfo(ObjectInfo objRef)
		{
			this.position = objRef.position;
			this.scale = objRef.scale;	
			this.name = objRef.name;	
		}
	}

	public class QuadTree : MonoBehaviour
	{
		private GameObject root = null;
		
		public float CenterX = 0.0f;
		public float CenterY = 0.0f;
		public float Width = 0.0f;
		public float TargetSize = 0.0f;
	
		private List<ObjectInfo> oldObstacles = new List<ObjectInfo>();
			
		public List<ObjectInfo> differentObjects = new List<ObjectInfo>();
	
		public List<QuadTreeItem> qtisToRegenerate = new List<QuadTreeItem>();
	
		//public QuadTreeItem Root
		public GameObject Root
		{
			get
			{
				return root;	
			}
		}
	
		public static bool generationDone = false;
	
		public static bool waypointsNeedRegeneration = false;
		
		List<GameObject> quadTree = new List<GameObject>();
	
		public bool IsGameObjectIn(GameObject box1, GameObject box2)
		{
			Vector3 size = box2.transform.localScale;
			Vector3 position = box2.transform.position;
		
			return OOBCollisionDetection.AreBoxexOverlapping(box1, size, position);
		}
	
		public bool IsGameObjectIn(GameObject bigger, Vector3 smallerPos)
		{
			Vector3 size = new Vector3(Width, 1, Width);
		
			return OOBCollisionDetection.AreBoxexOverlapping(bigger, size, smallerPos);
		}
	
		public List<GameObject> GetObjectsInside(Vector3 vec)
		{
			GameObject act = root;	
		
			List<GameObject> result = new List<GameObject>();
		
			while (true)
			{
				Vector3 actPosition = act.GetComponent<QuadTreeItem>().Position;
			
				if (act.GetComponent<QuadTreeItem>().Children.Count == 0 && (IsGameObjectIn(act, vec)/* || act.GetComponent<QuadTreeItem>().GameObjects.Count != 0 */))
				{
					result.AddRange(act.GetComponent<QuadTreeItem>().GameObjects);
					break;				
				}
			
				if (act.GetComponent<QuadTreeItem>().Children.Count == 0)
				{
					break;
				}					
				
				if (vec.x <= actPosition.x && vec.z <= actPosition.z)
				{
					act = act.GetComponent<QuadTreeItem>().Children[0];
				}
				else if (vec.x <= actPosition.x && vec.z >= actPosition.z)
				{
					act = act.GetComponent<QuadTreeItem>().Children[3];
				}	
				else if (vec.x >= actPosition.x && vec.z >= actPosition.z)
				{
					act = act.GetComponent<QuadTreeItem>().Children[2];
				}
				else if (vec.x >= actPosition.x && vec.z <= actPosition.z)
				{
					act = act.GetComponent<QuadTreeItem>().Children[1];
				}
				else
				{
					break;
				}
			}
		
			return result;
		}
	
		public List<QuadTreeItem> GetQuadTreeItemFor(Vector3 vec, Vector3 size, int maxDepth)
		{
			var result = new List<QuadTreeItem>();
		
			Vector3 leftFront 	= new Vector3(vec.x - size.x/2.0f, vec.y, vec.z - size.z/2.0f);
			Vector3 rightFront 	= new Vector3(vec.x + size.x/2.0f, vec.y, vec.z - size.z/2.0f);
			
			Vector3 leftBack 	= new Vector3(vec.x - size.x/2.0f, vec.y, vec.z + size.z/2.0f);
			Vector3 rightBack 	= new Vector3(vec.x + size.x/2.0f, vec.y, vec.z + size.z/2.0f);
		
			result.Add(GetQuadTreeItemFor(leftFront, 	maxDepth));
			result.Add(GetQuadTreeItemFor(rightFront, 	maxDepth));
			result.Add(GetQuadTreeItemFor(leftBack, 	maxDepth));
			result.Add(GetQuadTreeItemFor(rightBack, 	maxDepth));
		
			return result;
		}
	
		public QuadTreeItem GetQuadTreeItemFor(Vector3 vec, int maxDepth)
		{
			GameObject act = root;	
		
			QuadTreeItem result = null;
		
			int depth = 0;
		
			while (true)
			{
				Vector3 actPosition = act.GetComponent<QuadTreeItem>().Position;
			
				if (act.GetComponent<QuadTreeItem>().Children.Count == 0 && (IsGameObjectIn(act, vec)))
				{
					result = act.GetComponent<QuadTreeItem>();
					break;				
				}
			
				if (act.GetComponent<QuadTreeItem>().Children.Count == 0)
				{
					break;
				}	
			
				if (maxDepth != 0 && depth == maxDepth && IsGameObjectIn(act, vec))
				{
					result = act.GetComponent<QuadTreeItem>();
					break;	
				}
				
				if (vec.x <= actPosition.x && vec.z <= actPosition.z)
				{
					act = act.GetComponent<QuadTreeItem>().Children[0];
				}
				else if (vec.x <= actPosition.x && vec.z >= actPosition.z)
				{
					act = act.GetComponent<QuadTreeItem>().Children[3];
				}	
				else if (vec.x >= actPosition.x && vec.z >= actPosition.z)
				{
					act = act.GetComponent<QuadTreeItem>().Children[2];
				}
				else if (vec.x >= actPosition.x && vec.z <= actPosition.z)
				{
					act = act.GetComponent<QuadTreeItem>().Children[1];
				}
				else
				{
					break;
				}
			
				depth++;
			}
		
			return result;
		}
	
		void DisposeOldQuadTree(GameObject paramRoot, bool keepRoot)
		{		
			if (paramRoot != null && paramRoot.GetComponent<QuadTreeItem>().Children.Count == 4)
			{
				for (int i = 0; i < 4; i++)
				{
					DisposeOldQuadTree(paramRoot.GetComponent<QuadTreeItem>().Children[i], false);
				}
			
				paramRoot.GetComponent<QuadTreeItem>().Children.Clear();
			}	
			if (paramRoot != null && !keepRoot)
			{
				GameObject.DestroyImmediate(paramRoot);
			}
		}
		
		public List<GameObject> DetermineGameObjectsInside(GameObject item)
		{
			GameObject[] list = GameObject.FindGameObjectsWithTag("wall");
			
			List<GameObject> result = new List<GameObject>();
			
			foreach (GameObject go in list)
			{
				if (IsGameObjectIn(item, go))
				{
					result.Add(go);
				}
			}
			
			return result;
		}
		
		void Start()
		{
		}
	
		private int detectionCounter = 0;
	
		bool DetectChangeInObstacles()
		{
			bool result = true;
		
			if (detectionCounter < 50)
			{
				result = false;
				detectionCounter++;
			}
			else
			{
				//Debug.Log ("Times up");
				detectionCounter = 0;
			}
		
			if (result)
			{
				differentObjects.Clear();
			
				var newObstacles = new List<ObjectInfo>();
			
				var obstacles = GameObject.FindGameObjectsWithTag("wall");
				foreach (GameObject go in obstacles)
				{
					newObstacles.Add(new ObjectInfo(go.transform.position, go.transform.localScale, go.name));
				}
			
				if (newObstacles.Count != oldObstacles.Count)
				{
					Debug.Log ("Obstacle count differs");	
				
					if (oldObstacles.Count > newObstacles.Count)
					{
						Debug.Log ("Old > new");
						foreach (ObjectInfo oi1 in oldObstacles)
						{
							bool found = false;
						
							foreach (ObjectInfo oi2 in newObstacles)
							{	
								if (oi2.position == oi1.position)
									found = true;
							}
						
							if (!found)
								differentObjects.Add(oi1);
						}
					}
					else
					{
						Debug.Log ("Old < new");
						foreach (ObjectInfo oi1 in newObstacles)
						{
							bool found = false;
						
							foreach (ObjectInfo oi2 in oldObstacles)
							{	
								if (oi2.position == oi1.position)
									found = true;
							}
						
							if (!found)
								differentObjects.Add(oi1);
						}
					}
				
					result = true;
				}
				else
					result = false;
				
				int counter = 0;
				
				if (!result)
				{
					for (; counter < newObstacles.Count; counter++)
					{
						ObjectInfo oldObject = oldObstacles[counter];
						ObjectInfo newObject = newObstacles[counter];
					
						newObject.moved = false;
					
						if (oldObject.position != newObject.position)
						{
							newObject.moved = true;
							newObject.oldPosition = oldObject.position;
							result = true;
							differentObjects.Add(newObject);
						}
						else if (oldObject.scale != newObject.scale)
						{
							result = true;
							differentObjects.Add(newObject);
						}
					}
				}
			
				//Debug.Log ("change in obstacles: " + result + " map generator done: " + Generator.generatorDone + " quadtree generation done: " + generationDone );
			}
		
			return result;
		}
	
		public static bool rebuildingQuadTree = false;
		
		void UpdateQuadTree()
		{
			Debug.Log("Different objs count: " + differentObjects.Count);
			foreach (ObjectInfo oi in differentObjects) 
			{				
				List<QuadTreeItem> result = GetQuadTreeItemFor(new Vector3(oi.position.x, 1.0f, oi.position.z), oi.scale, 2);
				result.AddRange(GetQuadTreeItemFor(new Vector3(oi.oldPosition.x, 1.0f, oi.oldPosition.z), oi.scale, 2));
			
				foreach (QuadTreeItem qti in result)
				{
					qti.GameObjects.Clear();
				
					DisposeOldQuadTree(qti.insideGameObject, true);
				
					Init (qti.insideGameObject, qti.Depth);
				
					qtisToRegenerate.Add (qti);	
				}
			}
		}
	
		void Update()
		{
			if (Generator.generatorDone && !generationDone)
			{
				Init (null, 0);
				generationDone = true;
			}
			else if (DetectChangeInObstacles() && Generator.generatorDone && generationDone == true)
			{			
				UpdateQuadTree();
				waypointsNeedRegeneration = true;
			
				var obstacles = GameObject.FindGameObjectsWithTag("wall");
			
				oldObstacles.Clear();
				foreach (GameObject go in obstacles)
				{
					oldObstacles.Add(new ObjectInfo(go.transform.position, go.transform.localScale, go.name));
				}
			}			
		}
	
		static int itemCounter = 0;

		GameObject PrepareChild (GameObject parent, Vector3 position, Vector3 size, int itemCounter, int depth)
		{
			GameObject child = null;
		
			child = GameObject.CreatePrimitive(PrimitiveType.Cube);
			child.transform.position = position;
			child.AddComponent<QuadTreeItem>();
			child.name = "quadtree_child" + depth + "_" + itemCounter;
			child.GetComponent<MeshRenderer>().enabled = false;
			child.GetComponent<QuadTreeItem>().Position = position;
			child.GetComponent<QuadTreeItem>().Size = size;
			child.GetComponent<QuadTreeItem>().Parent = parent;
			child.GetComponent<QuadTreeItem>().Depth = depth;
			child.GetComponent<QuadTreeItem>().insideGameObject = child;
			child.GetComponent<BoxCollider>().enabled = false;
			child.transform.localScale = size;
			child.GetComponent<QuadTreeItem>().GameObjects.AddRange(DetermineGameObjectsInside(child));
		
			return child;
		}
	
		public void Init(GameObject startItem, int depth)
		{
			if (startItem == null)
			{			
				var obstacles = GameObject.FindGameObjectsWithTag("wall");
			
				oldObstacles.Clear();
				foreach (GameObject go in obstacles)
				{
					oldObstacles.Add(new ObjectInfo(go.transform.position, go.transform.localScale, go.name));
				}				
			
				root = startItem = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				root.name = "quadtree_root";
				root.AddComponent<QuadTreeItem>();
				root.GetComponent<MeshRenderer>().enabled = false;
				root.AddComponent<BoxCollider>();
				root.GetComponent<BoxCollider>().size = new Vector3(Width, 1, Width);
				root.GetComponent<BoxCollider>().enabled = false;
				root.GetComponent<QuadTreeItem>().insideGameObject = root;
				
				root.GetComponent<QuadTreeItem>().Position = new Vector3(CenterX, 0, CenterY);
				root.GetComponent<QuadTreeItem>().Size = new Vector3(Width, 1, Width);
				root.transform.position = new Vector3(CenterX, 0, CenterY);
				
				root.GetComponent<QuadTreeItem>().GameObjects.AddRange( DetermineGameObjectsInside(root));
				
				quadTree.Add(root);	
				
				float newWidth = startItem.GetComponent<QuadTreeItem>().Size.x;// / 2.0f;
				
				Vector3 pos1 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x - (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - (newWidth/4));
				Vector3 size = new Vector3(newWidth/2, newWidth/2, newWidth/2);
				
				Vector3 pos2 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - (newWidth/4));
				
				Vector3 pos3 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z + (newWidth/4));
				
				Vector3 pos4 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x - (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z + (newWidth/4));
			
				GameObject child1, child2, child3, child4;
				
				child1 = PrepareChild (root, pos1, size, itemCounter, depth);				
				itemCounter++;
				child2 = PrepareChild (root, pos2, size, itemCounter, depth);				
				itemCounter++;
				child3 = PrepareChild (root, pos3, size, itemCounter, depth);
				itemCounter++;
				child4 = PrepareChild (root, pos4, size, itemCounter, depth);
				itemCounter++;
			
				root.GetComponent<QuadTreeItem>().Children.Add(child1);
				root.GetComponent<QuadTreeItem>().Children.Add(child2);
				root.GetComponent<QuadTreeItem>().Children.Add(child3);
				root.GetComponent<QuadTreeItem>().Children.Add(child4);
				
				Init(child1, depth+1);
				Init(child2, depth+1);
				Init(child3, depth+1);
				Init(child4, depth+1);
			
				root.GetComponent<QuadTreeItem>().GameObjects.AddRange(child1.GetComponent<QuadTreeItem>().GameObjects);
				root.GetComponent<QuadTreeItem>().GameObjects.AddRange(child2.GetComponent<QuadTreeItem>().GameObjects);
				root.GetComponent<QuadTreeItem>().GameObjects.AddRange(child3.GetComponent<QuadTreeItem>().GameObjects);
				root.GetComponent<QuadTreeItem>().GameObjects.AddRange(child4.GetComponent<QuadTreeItem>().GameObjects);
			}
			else
			{
				float newWidth = startItem.GetComponent<QuadTreeItem>().Size.x;
				
				if (newWidth < TargetSize)
					return;
				
				Vector3 pos1 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x - (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - (newWidth/4));
				Vector3 size = new Vector3(newWidth/2, newWidth/2, newWidth/2);
				
				Vector3 pos2 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - (newWidth/4));
				
				Vector3 pos3 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z + (newWidth/4));
				
				Vector3 pos4 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x - (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z + (newWidth/4));
				
				GameObject child1, child2, child3, child4;
				
				if (pos1.x < Width / 2 && pos1.z < Width / 2)
				{					
					child1 = PrepareChild (startItem, pos1, size, itemCounter, depth);		
				
					startItem.GetComponent<QuadTreeItem>().Children.Add(child1);
					Init(child1, depth + 1);
					
					startItem.GetComponent<QuadTreeItem>().GameObjects.AddRange(child1.GetComponent<QuadTreeItem>().GameObjects);
				
					itemCounter++;
				}
			
				if (pos2.x < Width / 2 && pos2.z < Width / 2)
				{
					child2 = PrepareChild (startItem, pos2, size, itemCounter, depth);	
					
					startItem.GetComponent<QuadTreeItem>().Children.Add(child2);
					Init(child2, depth + 1);
				
					startItem.GetComponent<QuadTreeItem>().GameObjects.AddRange(child2.GetComponent<QuadTreeItem>().GameObjects);
				
					itemCounter++;
				}
				
				if (pos3.x < Width / 2 && pos3.z < Width / 2)
				{
					child3 = PrepareChild (startItem, pos3, size, itemCounter, depth);	
				
					startItem.GetComponent<QuadTreeItem>().Children.Add(child3);
					Init(child3, depth + 1);
				
					startItem.GetComponent<QuadTreeItem>().GameObjects.AddRange(child3.GetComponent<QuadTreeItem>().GameObjects);
				
					itemCounter++;
				}	
			
				if (pos4.x < Width / 2 && pos4.z < Width / 2)
				{				
					child4 = PrepareChild (startItem, pos4, size, itemCounter, depth);	
				
					startItem.GetComponent<QuadTreeItem>().Children.Add(child4);
					Init(child4, depth + 1);
				
					startItem.GetComponent<QuadTreeItem>().GameObjects.AddRange(child4.GetComponent<QuadTreeItem>().GameObjects);
				
					itemCounter++;
				}	
			}
		}
	}
