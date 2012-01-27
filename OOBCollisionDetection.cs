using UnityEngine;
using System.Collections;


class OOBCollisionDetection //: MonoBehaviour
{	
	// test code
	void Update()
	{
		GameObject box1 = GameObject.Find("Cube1");	
		GameObject box2 = GameObject.Find("Cube2");
		
		Vector3 a = new Vector3(box1.transform.localScale.x / 2.0f, box1.transform.localScale.y / 2.0f, box1.transform.localScale.z / 2.0f);
		Vector3 b = new Vector3(box2.transform.localScale.x / 2.0f, box2.transform.localScale.y / 2.0f, box2.transform.localScale.z / 2.0f);
		
		Vector3 Pa = box1.transform.position;
		Vector3 Pb = box2.transform.position;
		
		Vector3[] A = new Vector3[3];
		
		A[0] = box1.transform.forward;
		A[1] = box1.transform.up;
		A[2] = box1.transform.right;
		
		Vector3[] B = new Vector3[3];
		
		B[0] = box2.transform.forward;
		B[1] = box2.transform.up;
		B[2] = box2.transform.right;
		
		if (OBBOverlap(a, Pa, A, b, Pb, B))
		{
			Debug.Log ("Collision");	
		}
		else
		{
			Debug.Log ("Not Collision");	
		}
	}
	
	public static Vector3 GetLineIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4){
		var denom = ((p4.z - p3.z) * (p2.x - p1.x)) - ((p4.x - p3.x) * (p2.z - p1.z));
		
		var nume_a = ((p4.x - p3.x) * (p1.z - p3.z)) - ((p4.z - p3.z) * (p1.x - p3.x));
		
		var nume_b = ((p2.x - p1.x) * (p1.z - p3.z)) - ((p2.z - p1.z) * (p1.x - p3.x));
		
		var result = Vector3.zero;
		
		if (denom == 0.0) {
			result.x = -1;
			result.z = -1;
			
			return result;
		}
		
		var ua = nume_a / denom;
		var ub = nume_b / denom;
		
		if (ua >= 0.0 && ua <= 1.0 && ub >= 0.0 && ub <= 1.0) {
			var x = p1.x + ua * (p2.x - p1.x);
			var y = p1.y + ua * (p2.y - p1.y);
			
			result.x = x;
			result.z = y;
			
			return result;
		}
		else {
			result.x = -1;
			result.z = -1;
			
			return result;
		}
	}
	
	/*public static bool IsRayIntersectingBox(Vector3 boxPosition, Vector3 boxSize, Vector3 start, Vector3 end)
	{
		Vector3 leftFront 	= new Vector3(boxPosition.x - boxSize.x/2.0f, boxPosition.y, boxPosition.z - boxSize.z/2.0f);
		Vector3 rightFront 	= new Vector3(boxPosition.x + boxSize.x/2.0f, boxPosition.y, boxPosition.z - boxSize.z/2.0f);
		
		Vector3 leftBack 	= new Vector3(boxPosition.x - boxSize.x/2.0f, boxPosition.y, boxPosition.z + boxSize.z/2.0f);
		Vector3 rightBack 	= new Vector3(boxPosition.x + boxSize.x/2.0f, boxPosition.y, boxPosition.z + boxSize.z/2.0f);	
		
		bool result = false;
		
		Vector3 v1 = GetLineIntersection(leftFront, rightFront, start, end);
		Vector3 v2 = GetLineIntersection(rightFront, rightBack, start, end);
		Vector3 v3 = GetLineIntersection(rightBack, leftBack, start, end);
		Vector3 v4 = GetLineIntersection(leftBack, leftFront, start, end);
		
		result = (v1.x != -1 && v1.y != -1) || (v2.x != -1 && v2.y != -1) || (v3.x != -1 && v3.y != -1) || (v4.x != -1 && v4.y != -1);
		
		return result;
	}*/
	
	/*public static bool IsRayIntersectingBox(Vector3 boxPosition, Vector3 boxSize, Vector3 lineDir, Vector3 midpoint, double segmentHalflength)
	{
		
		Vector3 T = boxPosition - midpoint;
		Vector3 v = Vector3.zero;
		double r = 0;
		
		//do any of the principal axes
		//form a separating axis?
		
		if( Mathf.Abs(T.x) > boxSize.x + segmentHalflength*Mathf.Abs(lineDir.x) )
			return false;
		
		if( Mathf.Abs(T.y) > boxSize.y + segmentHalflength*Mathf.Abs(lineDir.y) )
			return false;
		
		if( Mathf.Abs(T.z) > boxSize.z + segmentHalflength*Mathf.Abs(lineDir.z) )
			return false;
		
		//l.cross(x-axis)?
		
		r = boxSize.y*Mathf.Abs(lineDir.z) + boxSize.z*Mathf.Abs(lineDir.y);
		
		if( Mathf.Abs(T.y*lineDir.z - T.z*lineDir.y) > r )
			return false;
		
		//l.cross(y-axis)?
		
		r = boxSize.x*Mathf.Abs(lineDir.z) + boxSize.z*Mathf.Abs(lineDir.x);
		
		if( Mathf.Abs(T.z*lineDir.x - T.x*lineDir.z) > r )
			return false;
		
		//l.cross(z-axis)?
		
		r = boxSize.x*Mathf.Abs(lineDir.y) + boxSize.y*Mathf.Abs(lineDir.x);
		
		if( Mathf.Abs(T.x*lineDir.y - T.y*lineDir.x) > r )
			return false;
		
		return true;
	}*/
	
	/*public static bool IsRayIntersectingBox(Vector3 lineDest, Vector3 midPoint, Vector3 boxPosition, Vector3 boxSize) 
	{ 
	    float s, ss, xhit, yhit, zhit; 
	
	    ss = float.MaxValue; 
	
		int sideHit = 0;
		
	    // translate ray origin to objects space 
	    Vector3 relativePos = new Vector3(midPoint.x, midPoint.y, midPoint.z); 
	    relativePos -= boxPosition; 
	
	    // check x faces 
	    if (lineDest.x != 0) 
	    { 
	        s = (boxSize.x - relativePos.x) / lineDest.x; 
	
	        if ((s > 0.0) && (s < ss)) 
	        { 
	            yhit = Mathf.Abs(relativePos.y + s * lineDest.y); 
	            zhit = Mathf.Abs(relativePos.z + s * lineDest.z); 
	
	            if ((yhit < boxSize.y) && (zhit < boxSize.z)) 
	            { 
	                sideHit = 0; 
	                ss = s; 
	            } 
	        } 
	
	        s = (-boxSize.x - relativePos.x) / lineDest.x; 
	
	        if ((s > 0.0) && (s < ss)) 
	        { 
	            yhit = Mathf.Abs(relativePos.y + s * lineDest.y); 
	            zhit = Mathf.Abs(relativePos.z + s * lineDest.z); 
	
	            if ((yhit < boxSize.y) && (zhit < boxSize.z)) 
	            { 
	                sideHit = 1; 
	                ss = s; 
	            } 
	        } 
	    } 
	
	    // check y faces 
	    if (lineDest.y != 0) 
	    { 
	        s = (boxSize.y - relativePos.y) / lineDest.y; 
	
	        if ((s > 0.0) && (s < ss)) 
	        { 
	            xhit = Mathf.Abs(relativePos.x + s * lineDest.x); 
	            zhit = Mathf.Abs(relativePos.z + s * lineDest.z); 
	
	            if ((xhit < boxSize.x) && (zhit < boxSize.z)) 
	            { 
	                sideHit = 2; 
	                ss = s; 
	            } 
	        } 
	
	        s = (-boxSize.y - relativePos.y) / lineDest.y; 
	
	        if ((s > 0.0) && (s < ss)) 
	        { 
	            xhit = Mathf.Abs(relativePos.x + s * lineDest.x); 
	            zhit = Mathf.Abs(relativePos.z + s * lineDest.z); 
	
	            if ((xhit < boxSize.x) && (zhit < boxSize.z)) 
	            { 
	                sideHit = 3; 
	                ss = s; 
	            } 
	        } 
	    } 
	
	    // check z faces 
	    if (lineDest.z != 0) 
	    { 
	        s = (boxSize.z - relativePos.z) / lineDest.z; 
	
	        if ((s > 0.0) && (s < ss)) 
	        { 
	            xhit = Mathf.Abs(relativePos.x + s * lineDest.x); 
	            yhit = Mathf.Abs(relativePos.y + s * lineDest.y); 
	
	            if ((xhit < boxSize.x) && (yhit < boxSize.y)) 
	            { 
	                sideHit = 4; 
	                ss = s; 
	            } 
	        } 
	
	        s = (-boxSize.z - relativePos.z) / lineDest.z; 
	
	        if ((s > 0.0) && (s < ss)) 
	        { 
	            xhit = Mathf.Abs(relativePos.x + s * lineDest.x); 
	            yhit = Mathf.Abs(relativePos.y + s * lineDest.y); 
	
	            if ((xhit < boxSize.x) && (yhit < boxSize.y)) 
	            { 
	                sideHit = 5; 
	                ss = s; 
	            } 
	        } 
	    } 
	
	    if (ss == float.MaxValue) 
	        return false; 
	
	    // ss is the distance the ray travelled before hitting the box.
	    hit.m_isect_t = ss; 
	    // Project the ray ss units to find the intersection point.
	    hit.m_Point = ray.Project(ss); 
	
	    hit.m_fEnter = ((ray.m_Position.x > m_Min.x || ray.m_Position.x < m_Max.x) || 
	                    (ray.m_Position.y > m_Min.y || ray.m_Position.y < m_Max.y) || 
	                    (ray.m_Position.z > m_Min.z || ray.m_Position.z < m_Max.z)); 
	
	    hit.m_pPrim = this; 
	    hit.m_pSurf = m_pSurf; 
	
	    return true; 
	}*/
	
	static Vector3  operatorOr (Vector3 _c, Vector3 _d) 
    {
        return new Vector3(_c.y * _d.z - _c.z * _d.y,
                _c.z * _d.x - _c.x * _d.z,
                _c.x * _d.y - _c.y * _d.x);
    }
	
	public static  bool IsRayIntersectingBox(Vector3 segment_pt0, Vector3 segment_pt1, Vector3 boxPosition, Vector3 boxSize)
	{
		/*if (box.isInside(segment_pt0) && box.isInside(segment_pt1))
			return true;*/
	
		float[] fAWdU = new float[3];
		float[] fADdU = new float[3];
		float[] fAWxDdU = new float[3];
		float fRhs;
		Vector3 kSDir = 0.5f * (segment_pt1 - segment_pt0);
		Vector3 kSCen = segment_pt0 + kSDir;
	
		Vector3 kDiff = kSCen - boxPosition;
	
		fAWdU[0] = Mathf.Abs(kSDir.x);
		fADdU[0] = Mathf.Abs(kDiff.x);
		fRhs = boxSize.x + fAWdU[0];
		if (fADdU[0] > fRhs)
			return false;
	
		fAWdU[1] = Mathf.Abs(kSDir.y);
		fADdU[1] = Mathf.Abs(kDiff.y);
		fRhs = boxSize.y + fAWdU[1];
		if (fADdU[1] > fRhs)
			return false;
	
		fAWdU[2] = Mathf.Abs(kSDir.z);
		fADdU[2] = Mathf.Abs(kDiff.z);
		fRhs = boxSize.z + fAWdU[2];
		if (fADdU[2] > fRhs)
			return false;
	
		Vector3 kWxD = operatorOr(kSDir, kDiff);
	
		fAWxDdU[0] = Mathf.Abs(kWxD.x);
		fRhs = boxSize.y * fAWdU[2] + boxSize.z * fAWdU[1];
		if (fAWxDdU[0] > fRhs)
			return false;
	
		fAWxDdU[1] = Mathf.Abs(kWxD.y);
		fRhs = boxSize.x * fAWdU[2] + boxSize.z * fAWdU[0];
		if (fAWxDdU[1] > fRhs)
			return false;
	
		fAWxDdU[2] = Mathf.Abs(kWxD.z);
		fRhs = boxSize.x * fAWdU[1] + boxSize.y * fAWdU[0];
		if (fAWxDdU[2] > fRhs)
			return false;
	
	    return true;
	}
	
	public static bool AreBoxexOverlapping(GameObject box1, Vector3 box2Size, Vector3 box2Position)
	{	
		Vector3 a = new Vector3(box1.transform.localScale.x / 2.0f, box1.transform.localScale.y / 2.0f, box1.transform.localScale.z / 2.0f);
		Vector3 b = new Vector3(box2Size.x / 2.0f, box2Size.y / 2.0f, box2Size.z / 2.0f);	
		
		Vector3 Pa = box1.transform.position;
		Vector3 Pb = box2Position;
		
		Vector3[] A = new Vector3[3];
		
		A[0] = box1.transform.forward;
		A[1] = box1.transform.up;
		A[2] = box1.transform.right;
		
		Vector3[] B = new Vector3[3];
		
		B[0] = new Vector3(1f, 0f, 0f);
		B[1] = new Vector3(0f, 1f, 0f);
		B[2] = new Vector3(0f, 0f, 1f);
		
		return OBBOverlap(a, Pa, A, b, Pb, B);
	}
	
	public static bool AreBoxexOverlapping(Vector3 box1Size, Vector3 box1Position, Vector3 box2Size, Vector3 box2Position)
	{	
		Vector3 a = new Vector3(box1Size.x / 2.0f, box1Size.y / 2.0f, box1Size.z / 2.0f);
		Vector3 b = new Vector3(box2Size.x / 2.0f, box2Size.y / 2.0f, box2Size.z / 2.0f);	
		
		Vector3 Pa = box1Position;
		Vector3 Pb = box2Position;
		
		Vector3[] A = new Vector3[3];
		
		A[0] = new Vector3(1f, 0f, 0f);
		A[1] = new Vector3(0f, 1f, 0f);
		A[2] = new Vector3(0f, 0f, 1f);
		
		Vector3[] B = new Vector3[3];
		
		B[0] = new Vector3(1f, 0f, 0f);
		B[1] = new Vector3(0f, 1f, 0f);
		B[2] = new Vector3(0f, 0f, 1f);
		
		return OBBOverlap(a, Pa, A, b, Pb, B);
	}
	
	public static bool AreBoxesOverlapping(GameObject box1, GameObject box2)
	{
		if (box1 == null || box2 == null)
			return false;
		
		Vector3 a = new Vector3(box1.transform.localScale.x / 2.0f, box1.transform.localScale.y / 2.0f, box1.transform.localScale.z / 2.0f);
		Vector3 b = new Vector3(box2.transform.localScale.x / 2.0f, box2.transform.localScale.y / 2.0f, box2.transform.localScale.z / 2.0f);	
		
		Vector3 Pa = box1.transform.position;
		Vector3 Pb = box2.transform.position;
		
		Vector3[] A = new Vector3[3];
		
		A[0] = box1.transform.forward;
		A[1] = box1.transform.up;
		A[2] = box1.transform.right;
		
		Vector3[] B = new Vector3[3];
		
		B[0] = box2.transform.forward;
		B[1] = box2.transform.up;
		B[2] = box2.transform.right;
		
		return OBBOverlap(a, Pa, A, b, Pb, B);
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

 

