using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{
	//Mesh
	[Header("Mesh Properties")]
	[Tooltip("Automatically generates a mesh based on the desired number of vertices.\nNote: Must be an even number >= 4.")]
	public int VertexCount; // 200
	private float VertexSpacing = 1.0f; // 0.3f
	private float StartX; // -10.0f
	private float YSurface; // 0.0f
	private float YBottom; // -3.0f

	//Water Properties
	[Header("Liquid Properties")]
	[Tooltip("Springs")]
	public Transform SpringPrefab;
	[Tooltip("How hard the vertices attract each other.")]
	public float Tension = 0.025f;
	[Tooltip("How far the waves can spread.\nNote: Dont increase this above 0.30f, Although you should try it.")]
	public float Spread = 0.25f;
	[Tooltip("How fast the waves loose their velocity.")]
	public float Damping = 0.025f;
	[Tooltip("The speed of which the waves will react to a colliding object to simulate a splash.\nMultiplier of the speed of the collisions done to the water.")]
	public float CollisionVelocity;
	[Tooltip("The max height the waves can increase.\nMAX Increase of water In Y.")]
	public float MaxIncrease;
	[Tooltip("The max height the waves can decrease.\nMAX Decrease of water In -Y (Use negative number).")]
	public float MaxDecrease;

	[Header("Wave Properties")]
	[Tooltip("This is just another spring but it is used to make the waves.")]
	public Transform WaveSimulatorPrefab;
	[Tooltip("The desired height the wave sim should reach each loop.")]
	public float WaveSimHeight;
	[Tooltip("The speed of which it will perform the loops.")]
	public float WaveSimSpeed;
	[Tooltip("A multiplier to increase the real wave height.")]
	public float WaveHeight;
	[Tooltip("How fast should it wait in seconds to copy the height of the simulator to the next vertex in the mesh.")]
	public float WaveTimeStep;

	//Hide in inspector
	private int SurfaceVertices;
	//This is used alot because all the work is done to the surface of the water not the bottom.
	private float Height;
	private float SmoothTime;
	private bool ChangedHeight;
	private float yVelocity = 0.0f;
	private Vector3[] vertices;
	private MeshFilter mf;
	private Mesh mesh;
	private int triNumber;
	private int[] triangle;
	private SpringScript[] SpringList;
	private float[] lDeltas;
	private float[] rDeltas;
	private bool Delaying = false;
	private Transform DelayingObject;
	[HideInInspector]
	public Vector3 DelayingObjectOldPos = new Vector3(0,0,0);
	private WaveSimulator WaveSimulator;
	private int te;
	private bool Waving = false;

	#region DrawGizmos, JNN: added
	private Color outlineColor = Color.green;

	void OnDrawGizmos()
	{
		Gizmos.color=outlineColor*0.5f;
		DrawBox();
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color=outlineColor;
		DrawBox();
	}
	
	void DrawBox()
	{
		// Calculate sides
		float left = this.transform.position.x - this.transform.localScale.x * 0.5f;
		float right = left + this.transform.localScale.x;
		float top = this.transform.position.y + this.transform.localScale.y * 0.5f;
		float bottom = top - this.transform.localScale.y;

		// Put together Corners
		Vector3 upperLeft = new Vector3(left, top, 0);
		Vector3 lowerLeft = new Vector3(left, bottom, 0);
		Vector3 upperRight = new Vector3(right, top, 0);
		Vector3 lowerRight = new Vector3(right, bottom, 0);

		// Draw Sides
		Gizmos.DrawLine(upperLeft,upperRight);
		Gizmos.DrawLine(upperRight,lowerRight);
		Gizmos.DrawLine(lowerRight,lowerLeft);
		Gizmos.DrawLine(lowerLeft,upperLeft);
	}
	#endregion

	void Awake()
	{
		SurfaceVertices = VertexCount / 2;
		vertices = new Vector3[VertexCount];
		triNumber = 0;
		triangle = new int[(VertexCount-2) * 3];
		SpringList = new SpringScript[SurfaceVertices];
		lDeltas = new float[SpringList.Length];
		rDeltas = new float[SpringList.Length];
	}

	void Start ()
	{
		VertexSpacing = 1.0f / (float)(SurfaceVertices-1);
		StartX = -0.5f;
		YSurface = 0.5f;
		YBottom = -0.5f;

		GenerateMesh();

		//Generating Springs and saving each of Spring's Scripts into the array for refrence later. Also setting the properties of it.
		for(int sprngs = 0; sprngs < SurfaceVertices; sprngs++)
		{
			Transform TransformHolder;
			TransformHolder = Instantiate (SpringPrefab, this.transform.position + vertices[sprngs]*this.transform.localScale.x, Quaternion.identity) as Transform;
			SpringList[sprngs] = TransformHolder.GetComponent<SpringScript>();
			SpringList[sprngs].MaxIncrease = MaxIncrease;
			SpringList[sprngs].MaxDecrease = MaxDecrease;
			SpringList[sprngs].TargetY = YSurface;
			SpringList[sprngs].Damping = Damping;
			SpringList[sprngs].Tension = Tension;
			SpringList[sprngs].ID = sprngs;
			SpringList[sprngs].Water = this;
			BoxCollider2D boxCollider = TransformHolder.GetComponent<BoxCollider2D>();
			boxCollider.size = new Vector2(this.transform.localScale.x*VertexSpacing,0.1f);//new Vector3(VertexSpacing,0,2);
			SpringList[sprngs].transform.parent = this.transform;
		}

		//Wave Simulator
		WaveSimulatorPrefab = Instantiate(WaveSimulatorPrefab, this.transform.position/*new Vector3(0,0,0)*/,Quaternion.identity) as Transform;
		WaveSimulator = WaveSimulatorPrefab.GetComponent<WaveSimulator>();
		WaveSimulator.WaveHeight = WaveSimHeight;
		WaveSimulator.WaveSpeed = WaveSimSpeed;
		WaveSimulator.transform.parent = this.transform;

		//StartCoroutine(ChangeWaterHeight(5,2));
	}

	void GenerateMesh()
	{
		mf = GetComponent<MeshFilter>();
		mesh = new Mesh();
		mf.mesh = mesh;

		//Generates the the surface using var i, then generates the bottom using var oo
		int oo = SurfaceVertices-1;
		for(int i = 0; i < VertexCount; i++)
		{
			if(i < SurfaceVertices)
			{
				vertices[i] = new Vector3(StartX + (VertexSpacing * i), YSurface, 0);
			}
			else if(i >= SurfaceVertices)
			{
				vertices[i] = new Vector3(vertices[oo].x, YBottom, 0);
				oo--;
			}
		}
		mesh.vertices = vertices;
		
		//Connecting the dots. :)
		//Setting the Triangles. I got this part working by lots of trial and error. I am sure there could be a better solution but anyways for now this doesnt affect the gameplay peformance and it works.
		int tt = SurfaceVertices;
		for(int t = SurfaceVertices - 1; t > 0; t--)
		{
			TriangulateRectangle(t, t-1, tt, tt+1);
			tt++;
		}
		mesh.triangles = triangle;
		
		//Setting the Normals
		Vector3[] normals = new Vector3[VertexCount];
		for(int n = 0; n < VertexCount; n++)
		{
			normals[n] = -Vector3.forward;
		}
		mesh.normals = normals;
		
		//Setting the UVS
		int nVertices = mesh.vertices.Length;
		Vector2[] uvs = new Vector2[nVertices];
		for(int r = 0; r < nVertices; r++)
		{
			uvs[r] = mesh.vertices[r];
		}
		mesh.uv = uvs;
	}

	void Update ()
	{
		//Without this each spring is independent.
		for(int e = 0; e < SpringList.Length; e++)
		{
			if(e > 0)
			{
				lDeltas[e] = Spread * (SpringList[e].transform.localPosition.y - SpringList[e - 1].transform.localPosition.y);
				SpringList[e-1].Speed += lDeltas[e];
			}
			if(e < SpringList.Length - 1)
			{
				rDeltas[e] = Spread * (SpringList[e].transform.localPosition.y - SpringList[e + 1].transform.localPosition.y);
				SpringList[e+1].Speed += rDeltas[e];
			}
		}

		for(int i = 0; i < SpringList.Length; i++)
		{
			if(i > 0)
			{
				SpringList[i - 1].transform.localPosition += new Vector3(0,lDeltas[i],0);
			}
			if(i < SpringList.Length - 1)
			{
				SpringList[i + 1].transform.localPosition += new Vector3(0,rDeltas[i],0);
			}
		}
		
		if(ChangedHeight)
		{
			float newPosition = Mathf.SmoothDamp(SpringList[0].TargetY,Height,ref yVelocity,SmoothTime);
			SetWaterHeight(newPosition);
		} 
		
		//for(int t=0; t< SpringList.Length; t++)
		//{
		// Optional code for springs goes here
		//}       
		
		//Set each Mesh vertex to the position of its spring 
		for(int vert = 0; vert < SurfaceVertices; vert++)
		{
			vertices[vert] = SpringList[vert].transform.localPosition;
		}
		mesh.vertices = vertices;
	}

	void FixedUpdate ()
	{
		if(!Waving)
		{
			StartCoroutine(MakeWave());
		}		
	}

	IEnumerator MakeWave ()
	{
		Waving = true;
		SpringList[te].Speed += Vector2.Distance(new Vector2(0,WaveSimulatorPrefab.localPosition.y), new Vector2(0,SpringList[te].transform.localPosition.y)) * WaveHeight;

		//And here is another wave behind the first wave
		//if( te > 20) {
		//	SpringList[te-20].Speed += Vector2.Distance(Vector2(0,WaveSimulatorPrefab.position.y), Vector2(0,SpringList[te].transform.position.y)) * WaveHeight;
		//}
		
		//SpringList[te].Speed += 0.20f; //This could be used but it wont look realistic, if you use this you dont need the WaveSimulator
		te++;
		if(te > SpringList.Length - 1)
		{
			te = 0;
		}
		yield return new WaitForSeconds(WaveTimeStep);
		Waving = false;
	}

	public void Splash(float Velocity, int ID, Transform Victim)
	{
		if(!Delaying)
		{
			SpringList[ID].Speed += Velocity * CollisionVelocity;
			DelayingObject = Victim;
			DelayingObjectOldPos = Victim.localPosition;
			Delaying = true;
			StartCoroutine(Delayer());
		}
		else
		{
			if(DelayingObject == Victim)
			{
				//Only 1 Splash each 1 Second per object
			}
			else
			{
				SpringList[ID].Speed += Velocity * CollisionVelocity;
				Delaying = true;
				StartCoroutine(Delayer());
				DelayingObject = Victim;
			}
		}
	}

	IEnumerator Delayer()
	{
		yield return new WaitForSeconds(1);
		Delaying = false;
	}

	public void SetWaterHeight(float mHeight)
	{
		for(int t = 0; t < SpringList.Length; t++)
		{
			SpringList[t].TargetY = mHeight;
		}
	}

	public IEnumerator ChangeWaterHeight(float mHeight, float mSmoothTime)
	{
		Height = mHeight;
		SmoothTime = mSmoothTime;
		ChangedHeight = true;
		yield return new WaitForSeconds(mSmoothTime);
	}
	
	void TriangulateRectangle(int p1, int p2, int p3, int p4)
	{
		triangle[triNumber] = p1;
		triNumber++;
		triangle[triNumber] = p3;
		triNumber++;
		triangle[triNumber] = p4;
		triNumber++;
		
		triangle[triNumber] = p4;
		triNumber++;
		triangle[triNumber] = p2;
		triNumber++;
		triangle[triNumber] = p1;
		triNumber++;
	}
}