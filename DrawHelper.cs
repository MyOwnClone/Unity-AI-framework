using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawHelper
{
	public static void DrawCube(Vector3 position, Vector3 size, Color color)
	{
		Vector3 leftFrontDown 	= new Vector3( -size.x / 2.0f, -size.y / 2.0f, -size.z / 2.0f );
		Vector3 rightFrontDown 	= new Vector3( 	size.x / 2.0f, -size.y / 2.0f, -size.z / 2.0f );
		Vector3 rightFrontUp 	= new Vector3( 	size.x / 2.0f, 	size.y / 2.0f, -size.z / 2.0f );
		Vector3 leftFrontUp 	= new Vector3( -size.x / 2.0f, 	size.y / 2.0f, -size.z / 2.0f );
		
		Vector3 leftBackDown 	= new Vector3( -size.x / 2.0f, -size.y / 2.0f, size.z / 2.0f );
		Vector3 rightBackDown 	= new Vector3( 	size.x / 2.0f, -size.y / 2.0f, size.z / 2.0f );
		Vector3 rightBackUp 	= new Vector3( 	size.x / 2.0f, 	size.y / 2.0f, size.z / 2.0f );
		Vector3 leftBackUp 		= new Vector3( -size.x / 2.0f, 	size.y / 2.0f, size.z / 2.0f );
		
		Vector3[] arr = new Vector3[8];
		
		arr[0] = leftFrontDown;
		arr[1] = rightFrontDown;
		arr[2] = rightFrontUp;
		arr[3] = leftFrontUp;
		
		arr[4] = leftBackDown;
		arr[5] = rightBackDown;
		arr[6] = rightBackUp;
		arr[7] = leftBackUp;
		
		for (int i = 0; i < arr.Length; i++)
			arr[i] += position;
		
		for (int i = 0; i < arr.Length; i++)
		{
			for (int j = 0; j < arr.Length; j++)
			{
				if (i != j)
				{
					Debug.DrawLine(arr[i], arr[j], color);	
				}
			}
		}
	}
}

