using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class VoronoiIsland : MonoBehaviour {
	
	public int seed = 0;
	
	public Texture2D output = null;
	public Gradient landGradient = new Gradient();
	public Gradient seaGradient = new Gradient();
	public float colorRandomness = .1f;
	public bool drawCenters = false;
	public bool overlayRegions = true;
	
	public int gridSize = 4;
	
	[Range(0f,1f)]
	public float altitudeRandomness = .5f;
	
	[Range(0f,1f)]
	public float seaLevel = .25f;
	
	public float slopeSmoothing = 2f;
	
	public int rasterSize = 128;
	public int meshSize = 8;
	public float meshScale = 8f;
	public float altitudeMultiplier = 32f;
	
	public Material RockMaterial = null;
	
	struct regionCenter
	{
		public Vector2 point;
		public Color color;
		public float elavation;
	}
	regionCenter[,] regionPoints;
	
	// Use this for initialization
	void Start () {
		output = new Texture2D(rasterSize,rasterSize);
		Init();
	}
	
	void OnGUI()
	{
		if (Input.GetKey(KeyCode.Tab))
		{
			GUI.DrawTexture (new Rect(0f,0f,Screen.width,Screen.height), output);
		}
	}
	
	// TODO Add coroutine stuff.
	void Init()
	{
		Debug.Log("Initializing.");
		InitGrid(gridSize);
		StartCoroutine(DoRaster());
		StartCoroutine(GenerateMesh());
	}
	
	void InitGrid (int width)
	{
		if (seed != 0)
		{
			Random.seed = seed;
		}
		
		regionPoints = new regionCenter[width,width];
		
		Vector2 center = Vector2.zero;
		center.x = width;
		center.y = width;
		center.x = center.x / 2f - .5f;
		center.y = center.y / 2f - .5f;
		float radius = center.x;
		
		Debug.Log("Generatoing voronoi points.");
		for (int row = 0; row < width; row++)
		{
			for (int col = 0; col < width; col++)
			{
				regionPoints[col,row] = GeneratePoint(col, row, center, radius);
			}
		}
		
		regionPoints[width/2,width/2].color = Color.magenta;
	}
	
	regionCenter GeneratePoint (int x, int y, Vector2 center, float islandRadius)
	{
		regionCenter ret = new regionCenter();
		
		Vector2 offset = Random.insideUnitCircle * .5f;
		ret.point = new Vector2(offset.x + x, offset.y + y);
		
		float distance = Vector2.Distance (
			ret.point, center);
		distance /= islandRadius;
		
		float altitude = 1f - distance;
		//Debug.Log("P:" + new Vector2(col,row) + "D:" + distance + " A: " + altitude);
		altitude *= Random.Range(1f-altitudeRandomness, 1f+altitudeRandomness);
		altitude -= seaLevel;
		altitude /= (1f-seaLevel);
		ret.elavation = altitude;
		
		if (altitude > 0f)
		{
			ret.color = landGradient.Evaluate(altitude);
		}
		else
		{
			ret.color = seaGradient.Evaluate(-1f*altitude/seaLevel);
		}
		ret.color.r *= Random.Range(1f-colorRandomness,1f+colorRandomness);
		ret.color.g *= Random.Range(1f-colorRandomness,1f+colorRandomness);
		ret.color.b *= Random.Range(1f-colorRandomness,1f+colorRandomness);
		
		return ret;
	}
	
	IEnumerator DoRaster ()
	{
		float scale = rasterSize;
		scale /= (float) gridSize + 1;
		scale = 1f / scale;
		
		float left = -1f; // Space on left edge
		float bottom = left;
		
		int coro = 0;
		
		output = new Texture2D(rasterSize, rasterSize);
		
		Debug.Log("Rasterizing image.");
		// Draw voronoi regions.
		for (int row = 0; row < rasterSize; row++)
		{
			for (int col = 0; col < rasterSize; col++)
			{
				Vector2 worldPoint = new Vector2(
					col * scale + left,
					row * scale + bottom
					);
				float t = GetAltitude(worldPoint);
				
				Color pointColor = Color.magenta;
				if (t >= 0f)
				{
					pointColor = landGradient.Evaluate(t);
				} 
				else 
				{
					pointColor = seaGradient.Evaluate(t*-1f);
				}
				output.SetPixel(col,row,pointColor);
				
				if (overlayRegions)
				{
					regionCenter r = GetClosestRegion(worldPoint);
					output.SetPixel(col, row, Color.Lerp(r.color, pointColor, .5f));
				}
				
				coro++;
				if (coro % rasterSize == 0) {
					output.Apply();
					yield return 0;
				}
			}
		}
		
		// Draw black dots for each of the points.
		if (drawCenters) {
			float colorMod = .75f;
			foreach (regionCenter r in regionPoints) {
				Vector2 v = r.point;
				Vector2 rasterPoint = new Vector2 (
					(v.x - left) / scale,
					(v.y - bottom) / scale
					);
				output.SetPixel (
					(int)rasterPoint.x,
					(int)rasterPoint.y,
					new Color (
					r.color.r * colorMod,
					r.color.g * colorMod,
					r.color.b * colorMod
					)
					);
			}
		}
		
		output.Apply();
		
		DumpMapToFile(output);
		
		return true;
	}
	
	// TODO Highly unoptimized, we can do a lot better than this because we have a general idea of the point layout in a grid.
	regionCenter GetClosestRegion (Vector2 worldPoint)
	{
		regionCenter ret = new regionCenter();
		float bestDistance = float.PositiveInfinity;
		ret.point = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
		
		regionCenter[] ballpark = GetNeighborPoints(worldPoint, 2);
		foreach (regionCenter current in ballpark)
		{
			float newDistance = Vector2.Distance(worldPoint,current.point);
			if (bestDistance > newDistance)
			{
				ret = current;
				bestDistance = newDistance;
			}
		}
		return ret;
	}
	
	regionCenter[] GetNeighborPoints (Vector2 worldPoint, int radius)
	{
		int d = radius;
		int left = Mathf.Max(0,(int)worldPoint.x-d);
		int right = Mathf.Min(gridSize-1,(int)worldPoint.x+d);
		int bottom = Mathf.Max(0,(int)worldPoint.y-d);
		int top = Mathf.Min(gridSize-1,(int)worldPoint.y+d);
		
		//Debug.Log("" + left + "," + right + "," + bottom + "," + top);
		
		List<regionCenter> ret = new List<regionCenter>();
		for (int x = left; x <= right; x++)
		{
			for (int y = bottom; y <= top; y++)
			{
				ret.Add(regionPoints[x,y]);
			}
		}
		return ret.ToArray();
	}
	
	void DumpMapToFile (Texture2D texture)
	{
		int i = 1;
		bool written = false;
		while (!written)
		{
			string target = Application.dataPath + "/Output/Map" + i + ".png";
			if (!File.Exists(target))
			{
				byte[] b = texture.EncodeToPNG();
				File.WriteAllBytes(target,b);
				written = true;
				Debug.Log("Map written to " + target);
			}
			i++;
		}
	}
	
	private Dictionary<Vector2,float> visitedPoints = new Dictionary<Vector2, float>();
	//public int pointsSavedFromDictionaryOptimization = 0;
	
	public float GetAltitude(Vector2 position)
	{
		if (visitedPoints.ContainsKey(position))
		{
			//pointsSavedFromDictionaryOptimization++;
			return visitedPoints[position];
		}
		
		regionCenter[] regions = GetNeighborPoints(position, 2);
		regionCenter one = new regionCenter();
		float distOne = float.PositiveInfinity;
		regionCenter two = new regionCenter();
		float distTwo = float.PositiveInfinity;
		regionCenter three = new regionCenter();
		float distThree = float.PositiveInfinity;
		foreach (regionCenter r in regions)
		{
			float newDist = Vector2.Distance(position, r.point);
			if (newDist < distOne)
			{
				three = two;
				distThree = distTwo;
				two = one;
				distTwo = distOne;
				one = r;
				distOne = newDist;
			} else if (newDist < distTwo)
			{
				three = two;
				distThree = distTwo;
				two = r;
				distTwo = newDist;
			} else if (newDist < distThree)
			{
				three = r;
				distThree = newDist;
			}
		}
		
		float ret = 0f;
		
		// Interpolate geometrically by calculating triangle areas.
		float oneTwo = Vector2.Distance(one.point,two.point);
		float twoThree = Vector2.Distance(two.point,three.point);
		float oneThree = Vector2.Distance(one.point,three.point);
		float areaOne = Herons(twoThree,distTwo,distThree);
		float areaTwo = Herons(oneThree,distOne,distThree);
		float areaThree = Herons(oneTwo,distOne,distTwo);
		float totalArea = areaOne+areaTwo+areaThree;
		
		ret += areaOne/totalArea * one.elavation;
		ret += areaTwo/totalArea * two.elavation;
		ret += areaThree/totalArea * three.elavation;
		/*
		float average = one.elavation + two.elavation + three.elavation;
		ret = Mathf.Lerp(one.elavation,average,distOne/(distOne+distTwo+distThree)+
		                 slopeSmoothing*
		                 Mathf.Cos(Vector2.Angle(two.point-position,three.point-position))*
	 					 Mathf.Cos(Vector2.Angle(one.point-position,three.point-position)));
		*/
		// Deterministic noise function.
		ret += (1f+ret)/2f*slopeSmoothing*distOne/(distOne+distTwo+distThree)*
			Mathf.Cos(Vector2.Angle(two.point-position,three.point-position))
				*Mathf.Cos(Vector2.Angle(one.point-position,three.point-position))
				*Mathf.Cos(Vector2.Angle(one.point-position,two.point-position));
		
		visitedPoints.Add(position,ret);
		
		return ret;
	}
	
	// Calculates the area of a triangle from its three side lengths.
	float Herons (float a, float b, float c)
	{
		float p = a+b+c;
		p /= 2f;
		return Mathf.Sqrt(p*(p-a)*(p-b)*(p-c));
	}
	
	IEnumerator GenerateMesh ()
	{
		for (int y = 0; y < gridSize; y++)
		{
			for (int x = 0; x < gridSize; x++)
			{
				MakeMesh(x,y,meshScale,meshSize);
				yield return 0;
			}
		}
	}
	
	void MakeMesh (int left, int bottom, float scale, int outputMeshSize)
	{
		Mesh ret = new Mesh();
		List<Vector3> triangles = new List<Vector3>();
		List<int> verts = new List<int>();
		int currentVertIndex = 0;
		List<Vector2> uvs = new List<Vector2>();
		
		//float separation = scale / (float)(outputMeshSize+1);
		float xSeparation = 1f / (float)(outputMeshSize+1);
		float zSeparation = 1f / (float)(outputMeshSize);
		Vector2 westOffset = new Vector2(-1f*xSeparation,0f);
		Vector2 northwestOffset = new Vector2(-.5f*xSeparation,zSeparation);
		Vector2 northeastOffset = new Vector2(.5f*xSeparation,zSeparation);
		
		//Vector2 lowerLeft = new Vector2(scale*left,scale*bottom);
		Vector2 lowerLeft = new Vector2(left,bottom);
		for (int posZ = 0; posZ <= outputMeshSize-1; posZ++)
		{
			int offset = posZ % 2; // Offset every other row for a triangular grid.
			for (int posX = 0; posX <= outputMeshSize+1; posX++)
			{
				Vector2 currentVertex = new Vector2(xSeparation*posX,zSeparation*posZ); // Offset from lower-left corner.
				currentVertex.x += xSeparation / 2f * (float)offset;
				
				Vector2 west = currentVertex + westOffset;
				Vector2 northwest = currentVertex + northwestOffset;
				Vector2 northeast = currentVertex + northeastOffset;
				
				// Northwest triangle.
				if (posX > 0)
				{
					triangles.Add(new Vector3(currentVertex.x,GetAltitude(lowerLeft+currentVertex),currentVertex.y));
					triangles.Add(new Vector3(west.x,GetAltitude(lowerLeft+west),west.y));
					triangles.Add(new Vector3(northwest.x,GetAltitude(lowerLeft+northwest),northwest.y));
					
					verts.Add(currentVertIndex);
					currentVertIndex++;
					verts.Add(currentVertIndex);
					currentVertIndex++;
					verts.Add(currentVertIndex);
					currentVertIndex++;
					
					// TODO uv mapping for texture color
					Vector2 center = (currentVertex + west + northwest) / 3f;
					center = ((center - new Vector2(.5f,.5f))*.95f)+new Vector2(.5f,.5f); // Prevent UV wrapping at south edge.
					uvs.Add(center);
					uvs.Add(center);
					uvs.Add(center);
				}
				// North triangle.
				if ((offset==1 || (posX > 0)) &&
				    !(offset == 1 && posX == outputMeshSize+1)) // Avoids making an extra triangle on the far right every other row.
				{
					triangles.Add(new Vector3(currentVertex.x,GetAltitude(lowerLeft+currentVertex),currentVertex.y));
					triangles.Add(new Vector3(northwest.x,GetAltitude(lowerLeft+northwest),northwest.y));
					triangles.Add(new Vector3(northeast.x,GetAltitude(lowerLeft+northeast),northeast.y));
					
					verts.Add(currentVertIndex);
					currentVertIndex++;
					verts.Add(currentVertIndex);
					currentVertIndex++;
					verts.Add(currentVertIndex);
					currentVertIndex++;
					
					// TODO uv mapping for texture color
					Vector2 center = (currentVertex + northwest + northeast) / 3f;
					center = ((center - new Vector2(.5f,.5f))*.95f)+new Vector2(.5f,.5f); // Prevent UV wrapping at south edge.
					uvs.Add(center);
					uvs.Add(center);
					uvs.Add(center);
				}
			}
		}
		
		/*
		float max = scale*gridSize*2;
		foreach (Vector3 v in triangles)
		{
			if (Mathf.Abs(v.x) > max
			    || Mathf.Abs(v.y) > max
			    || Mathf.Abs(v.z) > max)
			{
				throw new System.ArgumentOutOfRangeException();
			}
		}
		if (triangles.Count != verts.Count)
		{
			throw new System.ArgumentOutOfRangeException();
		}
		*/
		
		ret.vertices = triangles.ToArray();
		ret.triangles = verts.ToArray();
		
		ret.uv = uvs.ToArray();
		ret.RecalculateBounds();
		ret.RecalculateNormals();
		
		GameObject g = new GameObject();
		g.transform.position = new Vector3(lowerLeft.x*scale, 0f, lowerLeft.y*scale);
		g.transform.localScale *= scale;
		
		g.AddComponent<MeshRenderer>();
		g.AddComponent<MeshFilter>();
		g.GetComponent<MeshFilter>().sharedMesh = ret;
		for (int i = 0; i < ret.normals.GetLength(0); i++)
		{
			ret.normals[i] = Vector3.zero;
		}
		// TODO
		g.AddComponent<MeshCollider>();
		g.GetComponent<MeshCollider>().sharedMesh = ret;
		g.transform.parent = this.transform;
		g.GetComponent<MeshRenderer>().material = new Material(RockMaterial);
		
		int textureWidth = Mathf.NextPowerOfTwo(outputMeshSize + 1);
		Texture2D t = new Texture2D (textureWidth,textureWidth);
		for (int col = 0; col < textureWidth; col++)
		{
			for (int row = 0; row < textureWidth; row++)
			{
				Vector2 currentPosition = new Vector2(1f*col/textureWidth,1f*row/textureWidth);
				float height = GetAltitude(lowerLeft + currentPosition);
				
				Color newColor;

				/*if (height >= 0f)
				{
					newColor = landGradient.Evaluate(height);
				} else
				{
					newColor = seaGradient.Evaluate(-1f*height);
				}*/
				newColor = landGradient.Evaluate(height);
				t.SetPixel(col,row,newColor);
			}
		}
		//t.filterMode = FilterMode.Point;
		t.Apply ();
		g.GetComponent<MeshRenderer>().material.SetTexture (0,t);
	}
	
	
}