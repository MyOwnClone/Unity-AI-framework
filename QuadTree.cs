using System;
using System.Collections.Generic;
using UnityEngine;
	
	public class ObjectInfo 
	{
		public Vector3 scale = new Vector3(0f, 0f, 0f);
		public Vector3 position = new Vector3(0f, 0f, 0f);
	
		public ObjectInfo(Vector3 position, Vector3 scale)
		{	
			this.position = position;
			this.scale = scale;	
		}
	}

	public class QuadTree : MonoBehaviour
	{
		//private QuadTreeItem root = null;
		private GameObject root = null;
		
		public float CenterX = 0.0f;
		public float CenterY = 0.0f;
		public float Width = 0.0f;
		public float TargetSize = 0.0f;
	
		//private List<GameObject> allObstacles = new List<GameObject>();	// tag == wall
		private List<ObjectInfo> oldObstacles = new List<ObjectInfo>();
		
		private DateTime lastTimeChecked = DateTime.Now;
	
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
		
		/*public bool IsGameObjectIn(GameObject bigger, GameObject smaller)
		{
			return IsGameObjectIn(bigger, smaller.transform.position);		
		}
	
		public bool IsGameObjectIn(GameObject bigger, Vector3 smallerPos)
		{
			// bigger edges
			Vector3 biggerPos = bigger.transform.position;
			Vector3 biggerColliderSize = bigger.GetComponent<QuadTreeItem>().Size;
		
			Vector3 potencialBigger = bigger.transform.localScale;
		
			if (potencialBigger.x > biggerColliderSize.x && potencialBigger.y > biggerColliderSize.y && potencialBigger.z > biggerColliderSize.z)
				biggerColliderSize = potencialBigger;
			
			float leftFrontDownX = biggerPos.x - biggerColliderSize.x / 2.0f;
			float leftFrontDownY = biggerPos.y - biggerColliderSize.y / 2.0f;
			float leftFrontDownZ = biggerPos.z - biggerColliderSize.z / 2.0f;
			
			float rightFrontDownX = biggerPos.x + biggerColliderSize.x / 2.0f;
			float rightFrontDownY = biggerPos.y - biggerColliderSize.y / 2.0f;
			float rightFrontDownZ = biggerPos.z - biggerColliderSize.z / 2.0f;
						
			float leftBackDownX = biggerPos.x - biggerColliderSize.x / 2.0f;
			float leftBackDownY = biggerPos.y - biggerColliderSize.y / 2.0f;
			float leftBackDownZ = biggerPos.z + biggerColliderSize.z / 2.0f;
			
			float rightBackDownX = biggerPos.x + biggerColliderSize.x / 2.0f;
			float rightBackDownY = biggerPos.y - biggerColliderSize.y / 2.0f;
			float rightBackDownZ = biggerPos.z + biggerColliderSize.z / 2.0f;
			
			//Vector3 smallerPos = smaller.transform.position;
			
			if (smallerPos.x >= leftFrontDownX && smallerPos.x <= rightFrontDownX
				&& smallerPos.z >= rightFrontDownZ && smallerPos.z <= rightBackDownZ)
			{
				return true;	
			}
			else
			{
				return false;
			}
		}*/
	
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
	
		public QuadTreeItem GetQuadTreeItemFor(Vector3 vec)
		{
			GameObject act = root;	
		
			QuadTreeItem result = null;
		
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
	
		void DisposeOldQuadTree(GameObject root)
		{		
			if (root != null &&root.GetComponent<QuadTreeItem>().Children.Count == 4)
			{
				for (int i = 0; i < 4; i++)
				{
					DisposeOldQuadTree(root.GetComponent<QuadTreeItem>().Children[i]);
				}
				GameObject.DestroyImmediate(root);
			}	
			else if (root != null)
			{
				GameObject.DestroyImmediate(root);
			}
		}
		
		public List<GameObject> DetermineGameObjectsInside(GameObject item)
		{
			GameObject[] list = GameObject.FindGameObjectsWithTag("wall");
			
			List<GameObject> result = new List<GameObject>();
			
			foreach (GameObject go in list)
			{
				//if (go.GetComponent<BoxCollider>().size item.GetComponent<BoxCollider>().size	
				if (IsGameObjectIn(item, go))
				{
					result.Add(go);
				
					//go.GetComponent<WallController>().StringToDisplay = item.GetComponent<QuadTreeItem>().name;
					//go.GetComponent<WallController>().QuadTreeItems.Add(item.GetComponent<QuadTreeItem>());
				}
			}
			
			return result;
		}
		
		void Start()
		{
			//Init (null, 40, 0);	
		}
	
		private int detectionCounter = 0;
	
		bool DetectChangeInObstacles()
		{
			bool result = true;
		
			if (detectionCounter < 500)
			{
				result = false;
				detectionCounter++;
			}
			else
			{
				Debug.Log ("Times up");
				detectionCounter = 0;
			}
		
			if (result)
			{
				var newObstacles = new List<ObjectInfo>();
			
				var obstacles = GameObject.FindGameObjectsWithTag("wall");
				foreach (GameObject go in obstacles)
				{
					newObstacles.Add(new ObjectInfo(go.transform.position, go.transform.localScale));
				}
			
				if (newObstacles.Count != oldObstacles.Count)
				{
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
					
						if ((oldObject.position != newObject.position) || (oldObject.scale != newObject.scale))
						{
							return true;
						}
					}
				}
			
				Debug.Log ("change in obstacles: " + result + " map generator done: " + Generator.generatorDone + " quadtree generation done: " + generationDone );
			}
		
			return result;
		}
	
		public static bool rebuildingQuadTree = false;
		
		void Update()
		{
			if (Generator.generatorDone && !generationDone)
			{
				Init (null, 40, 0);
				generationDone = true;
			}
			else if (DetectChangeInObstacles() && Generator.generatorDone && generationDone == true)
			{
				Debug.Log("Rebuilding QuadTree");
				rebuildingQuadTree = true;
				DisposeOldQuadTree(root);
				Init (null, 40, 0);
				rebuildingQuadTree = false;
				waypointsNeedRegeneration = true;
			}			
		}
	
		public void Init(/*QuadTreeItem*/GameObject startItem, int level, int depth)
		{
			if (startItem == null)
			{
				//allObstacles = new List<GameObject>(GameObject.FindGameObjectsWithTag("wall"));
			
				var obstacles = GameObject.FindGameObjectsWithTag("wall");
				foreach (GameObject go in obstacles)
				{
					oldObstacles.Add(new ObjectInfo(go.transform.position, go.transform.localScale));
				}				
			
				root = startItem = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				root.name = "quadtree_root";
				root.AddComponent<QuadTreeItem>();
				root.GetComponent<MeshRenderer>().enabled = false;
				root.AddComponent<BoxCollider>();
				root.GetComponent<BoxCollider>().size = new Vector3(Width, 1, Width);
				root.GetComponent<BoxCollider>().enabled = false;
				
				root.GetComponent<QuadTreeItem>().Position = new Vector3(CenterX, 0, CenterY);
				root.GetComponent<QuadTreeItem>().Size = new Vector3(Width, 1, Width);
				root.transform.position = new Vector3(CenterX, 0, CenterY);
				
				root.GetComponent<QuadTreeItem>().GameObjects.AddRange( DetermineGameObjectsInside(root));
				
				quadTree.Add(root);
			
			
				
				float newWidth = startItem.GetComponent<QuadTreeItem>().Size.x;// / 2.0f;
				
				Vector3 pos1 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x - (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - (newWidth/4));
				Vector3 size1 = new Vector3(newWidth/2, level, newWidth/2);
				
				Vector3 pos2 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - (newWidth/4));
				Vector3 size2 = new Vector3(newWidth/2, level, newWidth/2);
				
				Vector3 pos3 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z + (newWidth/4));
				Vector3 size3= new Vector3(newWidth/2, level, newWidth/2);
				
				Vector3 pos4 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x - (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z + (newWidth/4));
				Vector3 size4= new Vector3(newWidth/2, level, newWidth/2);
			
				/*Vector3 pos4 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + newWidth, 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z);
				Vector3 size4 = new Vector3(newWidth, level, newWidth);*/
				
				/*Debug.Log ("Pos: " + pos1.x + ", " + pos1.y + ", " + pos1.z + ", size: " + size1.x + ", " + size1.y + ", " + size1.z);
				Debug.Log ("Pos: " + pos2.x + ", " + pos2.y + ", " + pos2.z + ", size: " + size2.x + ", " + size2.y + ", " + size2.z);
				Debug.Log ("Pos: " + pos3.x + ", " + pos3.y + ", " + pos3.z + ", size: " + size3.x + ", " + size3.y + ", " + size3.z);
				Debug.Log ("Pos: " + pos4.x + ", " + pos4.y + ", " + pos4.z + ", size: " + size4.x + ", " + size4.y + ", " + size4.z);*/
			
				GameObject child1, child2, child3, child4;
				
				child1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				child1.transform.position = pos1;
				child1.AddComponent<QuadTreeItem>();
				child1.name = "quadtree_child" + depth;
				child1.GetComponent<MeshRenderer>().enabled = false;
				child1.GetComponent<QuadTreeItem>().Position = pos1;
				child1.GetComponent<QuadTreeItem>().Size = size1;
				child1.GetComponent<QuadTreeItem>().Parent = root;
				//child1.AddComponent<BoxCollider>();
				//child1.GetComponent<BoxCollider>().size = size1;
				child1.GetComponent<BoxCollider>().enabled = false;
				child1.transform.localScale = size1;
				child1.GetComponent<QuadTreeItem>().GameObjects.AddRange(DetermineGameObjectsInside(child1));
				
				child2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				child2.transform.position = pos2;
				child2.AddComponent<QuadTreeItem>();
				child2.name = "quadtree_child" + depth;
				child2.GetComponent<MeshRenderer>().enabled = false;
				child2.GetComponent<QuadTreeItem>().Position = pos2;
				child2.GetComponent<QuadTreeItem>().Size = size2;
				child2.GetComponent<QuadTreeItem>().Parent = root;
				//child2.AddComponent<BoxCollider>();
				//child2.GetComponent<BoxCollider>().size = size2;
				child2.GetComponent<BoxCollider>().enabled = false;
				child2.transform.localScale = size2;
				child2.GetComponent<QuadTreeItem>().GameObjects.AddRange(DetermineGameObjectsInside(child2));
				
				child3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				child3.transform.position = pos3;
				child3.AddComponent<QuadTreeItem>();
				child3.name = "quadtree_child" + depth;
				child3.GetComponent<MeshRenderer>().enabled = false;
				child3.GetComponent<QuadTreeItem>().Position = pos3;
				child3.GetComponent<QuadTreeItem>().Size = size3;
				child3.GetComponent<QuadTreeItem>().Parent = root;
				//child3.AddComponent<BoxCollider>();
				//child3.GetComponent<BoxCollider>().size = size3;
				child3.GetComponent<BoxCollider>().enabled = false;
				child3.transform.localScale = size3;
				child3.GetComponent<QuadTreeItem>().GameObjects.AddRange(DetermineGameObjectsInside(child3));
				
				child4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				child4.transform.position = pos4;
				child4.AddComponent<QuadTreeItem>();
				child4.name = "quadtree_child" + depth;
				child4.GetComponent<MeshRenderer>().enabled = false;
				child4.GetComponent<QuadTreeItem>().Position = pos4;
				child4.GetComponent<QuadTreeItem>().Size = size4;
				child4.GetComponent<QuadTreeItem>().Parent = root;
				child4.transform.localScale = size4;
				//child4.AddComponent<BoxCollider>();
				//child4.GetComponent<BoxCollider>().size = size4;
				child4.GetComponent<BoxCollider>().enabled = false;
				child4.GetComponent<QuadTreeItem>().GameObjects.AddRange(DetermineGameObjectsInside(child4));
				
				root.GetComponent<QuadTreeItem>().Children.Add(child1);
				root.GetComponent<QuadTreeItem>().Children.Add(child2);
				root.GetComponent<QuadTreeItem>().Children.Add(child3);
				root.GetComponent<QuadTreeItem>().Children.Add(child4);
				
				Init(child1, level-10, depth+1);
				Init(child2, level-10, depth+1);
				Init(child3, level-10, depth+1);
				Init(child4, level-10, depth+1);
			}
			else
			{
				float newWidth = startItem.GetComponent<QuadTreeItem>().Size.x/* / 2.0f*/;
				
				if (newWidth < TargetSize)
					return;
			
				//float newWidth = startItem.GetComponent<QuadTreeItem>().Size.x;// / 2.0f;
				
				Vector3 pos1 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x - (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - (newWidth/4));
				Vector3 size1 = new Vector3(newWidth/2, level, newWidth/2);
				
				Vector3 pos2 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - (newWidth/4));
				Vector3 size2 = new Vector3(newWidth/2, level, newWidth/2);
				
				Vector3 pos3 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z + (newWidth/4));
				Vector3 size3= new Vector3(newWidth/2, level, newWidth/2);
				
				Vector3 pos4 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x - (newWidth/4), 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z + (newWidth/4));
				Vector3 size4= new Vector3(newWidth/2, level, newWidth/2);
				
				/*Vector3 pos1 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x - newWidth, 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - newWidth);
				Vector3 size1 = new Vector3(newWidth, level, newWidth);
				
				Vector3 pos2 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x, 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z - newWidth);
				Vector3 size2 = new Vector3(newWidth, level, newWidth);
				
				Vector3 pos3 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x, 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z + newWidth);
				Vector3 size3 = new Vector3(newWidth, level, newWidth);
				
				Vector3 pos4 = new Vector3(startItem.GetComponent<QuadTreeItem>().Position.x + newWidth, 1.0f,startItem.GetComponent<QuadTreeItem>().Position.z);
				Vector3 size4 = new Vector3(newWidth, level, newWidth);*/
			
				/*Debug.Log ("Pos: " + pos1.x + ", " + pos1.y + ", " + pos1.z + ", size: " + size1.x + ", " + size1.y + ", " + size1.z);
				Debug.Log ("Pos: " + pos2.x + ", " + pos2.y + ", " + pos2.z + ", size: " + size2.x + ", " + size2.y + ", " + size2.z);
				Debug.Log ("Pos: " + pos3.x + ", " + pos3.y + ", " + pos3.z + ", size: " + size3.x + ", " + size3.y + ", " + size3.z);
				Debug.Log ("Pos: " + pos4.x + ", " + pos4.y + ", " + pos4.z + ", size: " + size4.x + ", " + size4.y + ", " + size4.z);*/
				
				GameObject child1, child2, child3, child4;
				
				if (pos1.x < Width / 2 && pos1.z < Width / 2)
				{
					child1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
					child1.transform.position = pos1;
					child1.AddComponent<QuadTreeItem>();
					child1.name = "quadtree_child" + depth;
					child1.GetComponent<MeshRenderer>().enabled = false;
					child1.GetComponent<QuadTreeItem>().Position = pos1;
					child1.GetComponent<QuadTreeItem>().Size = size1;
					child1.GetComponent<QuadTreeItem>().Parent = startItem;
					child1.transform.localScale = size1;
					//child1.AddComponent<BoxCollider>();
					//child1.GetComponent<BoxCollider>().size = size1;
					child1.GetComponent<BoxCollider>().enabled = false;
					child1.GetComponent<QuadTreeItem>().GameObjects.AddRange(DetermineGameObjectsInside(child1));
									
					startItem.GetComponent<QuadTreeItem>().Children.Add(child1);
					Init(child1, level-10, depth + 1);
				}
			
				if (pos2.x < Width / 2 && pos2.z < Width / 2)
				{
					child2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
					child2.transform.position = pos2;
					child2.AddComponent<QuadTreeItem>();
					child2.name = "quadtree_child" + depth;
					child2.GetComponent<MeshRenderer>().enabled = false;
					child2.GetComponent<QuadTreeItem>().Position = pos2;
					child2.GetComponent<QuadTreeItem>().Size = size2;
					child2.GetComponent<QuadTreeItem>().Parent = startItem;
					child2.transform.localScale = size2;
					//child2.AddComponent<BoxCollider>();
					child2.GetComponent<BoxCollider>().enabled = false;
					child2.GetComponent<QuadTreeItem>().GameObjects.AddRange(DetermineGameObjectsInside(child2));
					
					startItem.GetComponent<QuadTreeItem>().Children.Add(child2);
					Init(child2, level-10, depth + 1);
				}
				
				if (pos3.x < Width / 2 && pos3.z < Width / 2)
				{
					child3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
					child3.transform.position = pos3;
					child3.AddComponent<QuadTreeItem>();
					child3.name = "quadtree_child" + depth;
					child3.GetComponent<MeshRenderer>().enabled = false;
					child3.GetComponent<QuadTreeItem>().Position = pos3;
					child3.GetComponent<QuadTreeItem>().Size = size3;
					child3.GetComponent<QuadTreeItem>().Parent = startItem;
					child3.transform.localScale = size3;
					//child3.AddComponent<BoxCollider>();
					//child3.GetComponent<BoxCollider>().size = size3;
					child3.GetComponent<BoxCollider>().enabled = false;
					child3.GetComponent<QuadTreeItem>().GameObjects.AddRange(DetermineGameObjectsInside(child3));
				
					startItem.GetComponent<QuadTreeItem>().Children.Add(child3);
					Init(child3, level-10, depth + 1);
				}	
			
				if (pos4.x < Width / 2 && pos4.z < Width / 2)
				{
					child4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
					child4.transform.position = pos4;
					child4.AddComponent<QuadTreeItem>();
					child4.name = "quadtree_child" + depth;
					child4.GetComponent<MeshRenderer>().enabled = false;
					child4.GetComponent<QuadTreeItem>().Position = pos4;
					child4.GetComponent<QuadTreeItem>().Size = size4;
					child4.GetComponent<QuadTreeItem>().Parent = startItem;
					child4.transform.localScale = size4;
					//child4.AddComponent<BoxCollider>();
					//child4.GetComponent<BoxCollider>().size = size4;
					child4.GetComponent<BoxCollider>().enabled = false;
					child4.GetComponent<QuadTreeItem>().GameObjects.AddRange(DetermineGameObjectsInside(child4));
				
					startItem.GetComponent<QuadTreeItem>().Children.Add(child4);
					Init(child4, level-10, depth + 1);
				}
				
			}
		}
		
		public QuadTree ()
		{
		}
	}
