using UnityEngine;
using System.Collections;


class OOBCollisionDetection : MonoBehaviour
{	
	void Update()
	{
		GameObject box1 = GameObject.Find("Cube1");	
		GameObject box2 = GameObject.Find("Cube2");
		
		Vector3 a = new Vector3(box1.transform.localScale.x / 2.0f, box1.transform.localScale.y / 2.0f, box1.transform.localScale.z / 2.0f);
		Vector3 b = new Vector3(box2.transform.localScale.x / 2.0f, box2.transform.localScale.y / 2.0f, box2.transform.localScale.z / 2.0f);
		
		Vector3 Pa = box1.transform.position;
		Vector3 Pb = box2.transform.position;
		
		Vector3[] A = new Vector3[3];
		
		A[0] = box1.transform.forward;//new Vector3(1f, 0f, 0f);
		A[1] = box1.transform.up;//new Vector3(0f, 1f, 0f);
		A[2] = box1.transform.right;//new Vector3(0f, 0f, 1f);
		
		Vector3[] B = new Vector3[3];
		
		B[0] = box2.transform.forward;//new Vector3(1f, 0f, 0f);
		B[1] = box2.transform.up;//new Vector3(0f, 1f, 0f);
		B[2] = box2.transform.right;//new Vector3(0f, 0f, 1f);
		
		if (OBBOverlap(a, Pa, A, b, Pb, B))
		{
			Debug.Log ("Collision");	
		}
		else
		{
			Debug.Log ("Not Collision");	
		}
	}
	
	
	static bool OBBOverlap(Vector3 a, Vector3 Pa, Vector3[] A, Vector3 b, Vector3 Pb, Vector3[] B)
	{
		//translation, in parent frame
		Vector3 v = Pb - Pa;
		//translation, in A's frame
		Vector3 T = new Vector3(Vector3.Dot(v, A[0]), Vector3.Dot(v, A[1]), Vector3.Dot(v, A[2]));  
		
		//B's basis with respect to A's local frame
		float[][] R = new float[3][];
		
		for (int m = 0; m < 3; m++)
		{
			R[m] = new float[3];
			for (int l = 0; l < 3; l++)
			{
				R[m][l] = 0.0f;	
			}
		}
		
		float ra, rb, t;
		long i, k;
		
		//calculate rotation matrix
		for( i=0 ; i<3 ; i++ )
		{
			for( k=0 ; k<3 ; k++ )
			{	
				R[i][k] = Vector3.Dot(A[i], B[k]);
			}
		}
				
		/*ALGORITHM: Use the separating axis test for all 15 potential 
		separating axes. If a separating axis could not be found, the two 
		boxes overlap. */

		//A's basis vectors
		for( int x=0 ; x<3 ; x++ )
		{		
			ra = a[x];

			rb = b[0]*Mathf.Abs(R[x][0]) + b[1]*Mathf.Abs(R[x][1]) + b[2]*Mathf.Abs(R[x][2]);

			t = Mathf.Abs( T[x] );

			if( t > ra + rb ) 
				return false;		
		}

		//B's basis vectors
		for( int y=0 ; y<3 ; y++ )
		{
			ra = a[0] * Mathf.Abs(R[0][y]) + a[1]*Mathf.Abs(R[1][y]) + a[2]*Mathf.Abs(R[2][y]);
			rb = b[y];
		
			t = Mathf.Abs(T[0]*R[0][y] + T[1]*R[1][y] + T[2]*R[2][y] );
		
			if( t > ra + rb )
				return false;				
		}

		//9 cross products
		
		//L = A0 x B0
		ra = a[1]*Mathf.Abs(R[2][0]) + a[2]*Mathf.Abs(R[1][0]);
		
		rb = 
		b[1]*Mathf.Abs(R[0][2]) + b[2]*Mathf.Abs(R[0][1]);
		
		t = 
		Mathf.Abs( T[2]*R[1][0] - T[1]*R[2][0] );
		
		if( t > ra + rb )
			return false;
		
		//L = A0 x B1
		ra = 
		a[1]*Mathf.Abs(R[2][1]) + a[2]*Mathf.Abs(R[1][1]);
		
		rb = 
		b[0]*Mathf.Abs(R[0][2]) + b[2]*Mathf.Abs(R[0][0]);
		
		t = 
		Mathf.Abs( T[2]*R[1][1] - T[1]*R[2][1] );
		
		if( t > ra + rb )
			return false;
		
		//L = A0 x B2
		ra = 
		a[1]*Mathf.Abs(R[2][2]) + a[2]*Mathf.Abs(R[1][2]);
		
		rb = 
		b[0]*Mathf.Abs(R[0][1]) + b[1]*Mathf.Abs(R[0][0]);
		
		t = Mathf.Abs( T[2]*R[1][2] - T[1]*R[2][2] );
		
		if( t > ra + rb )
			return false;
		
		//L = A1 x B0
		ra = 
		a[0]*Mathf.Abs(R[2][0]) + a[2]*Mathf.Abs(R[0][0]);
		
		rb = 
		b[1]*Mathf.Abs(R[1][2]) + b[2]*Mathf.Abs(R[1][1]);
		
		t =Mathf.Abs( T[0]*R[2][0] - T[2]*R[0][0] );
		
		if( t > ra + rb )
			return false;
		
		//L = A1 x B1
		ra = 
		a[0]*Mathf.Abs(R[2][1]) + a[2]*Mathf.Abs(R[0][1]);
		
		rb = 
		b[0]*Mathf.Abs(R[1][2]) + b[2]*Mathf.Abs(R[1][0]);
		
		t = Mathf.Abs( T[0]*R[2][1] - T[2]*R[0][1] );
		
		if( t > ra + rb )
			return false;
		
		//L = A1 x B2
		ra = 
		a[0]*Mathf.Abs(R[2][2]) + a[2]*Mathf.Abs(R[0][2]);
		
		rb = 
		b[0]*Mathf.Abs(R[1][1]) + b[1]*Mathf.Abs(R[1][0]);
		
		t = Mathf.Abs( T[0]*R[2][2] - T[2]*R[0][2] );
		
		if( t > ra + rb )
			return false;
		
		//L = A2 x B0
		ra = 
		a[0]*Mathf.Abs(R[1][0]) + a[1]*Mathf.Abs(R[0][0]);
		
		rb = 
		b[1]*Mathf.Abs(R[2][2]) + b[2]*Mathf.Abs(R[2][1]);
		
		t = 
		Mathf.Abs( T[1]*R[0][0] - T[0]*R[1][0] );
		
		if( t > ra + rb )
			return false;
		
		//L = A2 x B1
		ra = 
		a[0]*Mathf.Abs(R[1][1]) + a[1]*Mathf.Abs(R[0][1]);
		
		rb = 
		b[0] *Mathf.Abs(R[2][2]) + b[2]*Mathf.Abs(R[2][0]);
		
		t = 
		Mathf.Abs( T[1]*R[0][1] - T[0]*R[1][1] );
		
		if( t > ra + rb )
			return false;
		
		//L = A2 x B2
		ra = 
		a[0]*Mathf.Abs(R[1][2]) + a[1]*Mathf.Abs(R[0][2]);
		
		rb = 
		b[0]*Mathf.Abs(R[2][1]) + b[1]*Mathf.Abs(R[2][0]);
		
		t = 
		Mathf.Abs( T[1]*R[0][2] - T[0]*R[1][2] );
		
		if( t > ra + rb )
			return false;
		
		/*no separating axis found,
		the two boxes overlap */
		
		return true;		
	}	
};

 

