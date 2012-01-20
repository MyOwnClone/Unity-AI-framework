using System;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeItem : MonoBehaviour
	{
		public List<GameObject> GameObjects = new List<GameObject>();
		public List</*QuadTreeItem*/GameObject> Children = new List</*QuadTreeItem*/GameObject>();
		
		public enum PositionEnum
		{
			LEFT_UP,
			RIGHT_UP,
			LEFT_DOWN,
			RIGHT_DOWN,
		};
	
		public GameObject TextMeshPrefab = null;
	
		public int Level = 0;
		
		public GameObject Parent = null;
		
		public Vector3 Position = new Vector3(0.0f, 0.0f, 0.0f);
		public Vector3 Size = new Vector3(0.0f, 0.0f, 0.0f);
		
		/*public List<GameObject> GameObjects
		{
			get
			{
				return gameObjects;	
			}
		}
	
		public GameObject[] ChildrenArray
		{
			get
			{
				return GameObjects.ToArray();	
			}
		}
		
		public List<GameObject> Children
		{
			get
			{
				return children;	
			}
		}
	
		public int ChildrenCount {
			get
			{
				return children.Count;
			}
		}*/
		
		public QuadTreeItem() {}
		
		void Update()
		{
			//DrawHelper.DrawCube(Position, Size, Color.red);
		}
	}
