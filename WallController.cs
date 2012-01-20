using System;
using System.Collections.Generic;
using UnityEngine;

	public class WallController : MonoBehaviour
	{
		public List<QuadTreeItem> QuadTreeItems = new List<QuadTreeItem>();
		
		//public GameObject TextMeshPrefab = null;
	
		//public UnityEngine.Object text = null;
	
		public string StringToDisplay = string.Empty;
	
		public GameObject parent = null;
		
		GameObject wallTxt = null;
	
		/*public WallController ()
		{
			
		}*/
	
		void Start()
		{
			/*text =  Instantiate(TextMeshPrefab, this.transform.position, Quaternion.identity);
			parent.GetComponent<TextMesh>(). = text;*/
			wallTxt = new GameObject("TextField");
			wallTxt.transform.position = parent.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
			wallTxt.AddComponent<TextMesh>();
			wallTxt.AddComponent<MeshRenderer>();
			var meshRender = wallTxt.GetComponent<MeshRenderer>();
			var material = meshRender.material;
			meshRender.material = (Material) Resources.Load("Arial");
			wallTxt.GetComponent<TextMesh>().text = "Hello world";
			var myFont = (Font) Resources.Load("Arial");
			myFont.material.color = new Color(1.0f, 0.0f, 0.0f);
			wallTxt.GetComponent<TextMesh>().font = myFont;
		}
		
		void Update()
		{
			//text.text = StringToDisplay;
		
			List<string> str = new List<string>();
		
			foreach (QuadTreeItem qi in QuadTreeItems)
				str.Add(qi.name);
		
			wallTxt.GetComponent<TextMesh>().text = String.Join(",", str.ToArray());
		}
	}
