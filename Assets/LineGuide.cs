using UnityEngine;
using System.Collections;

public class LineGuide : TriggeredObject {

	private LineRenderer lineRenderer;
	public float p1Forward = 10.0f;
	public float startForward = 10.0f;
	private Helicopter helicopter;
	public enum Bezier
	{
		Quadratic,
		Cubic
	}
	public Bezier bezier = Bezier.Quadratic;
	
	public int segments = 50;
	public GameObject targetObject;
	private Vector3 target;
	
	public TrailRenderer movedObject;
	public float objectSpeed = 1.0f;
	
	void Start () {
		//targetObject = new GameObject("lineGuideTarget");
		//targetObject.transform.position = transform.position;
		lineRenderer = GetComponent<LineRenderer>();
		helicopter = transform.parent.GetComponent<Helicopter>();
		if(helicopter == null)
		{
			Debug.LogError("There is no helicopter on the parent", this);
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
	void LateUpdate () {
		if(targetObject != null){
			DrawLineToRescuable();
		}
		
	}
	Vector3 p0;
	Vector3 p1;
	Vector3 p2;
	Vector3 p3;

	private void DrawLineToRescuable()
	{
		
		lineRenderer.SetVertexCount(segments);
		lineRenderer.material.mainTextureOffset = new Vector2(1  - Time.time - Mathf.Floor(Time.time), 0 );
		p0 = transform.position + transform.forward * startForward;
		
		//Vector3 p2 = helicopter.nearestRescuable.transform.position;
		p1 = p0 + transform.forward * p1Forward;
		p2 = GetTargetPosition();
		
		float segSize = 1.0f / segments;
		for(int i = 0 ; i < segments; i++)
		{
			float t = segSize * i;
			if(bezier == Bezier.Quadratic)lineRenderer.SetPosition(i, BezierCurveQuadratic(t, p0, p1, p2));
			if(bezier == Bezier.Cubic)
			{
				p3 = GetTargetPosition();
				p2 = new Vector3(p3.x, p0.y, p3.z);
				lineRenderer.SetPosition(i, BezierCurveCubic(t, p0, p1, p2, p3));	
			}
			//else if(Bezier.Cubic)lineRenderer.SetPosition(i, BezierCurveCubic(t, , ));
			
		}
		float a = Time.time * objectSpeed - (int) (Time.time * objectSpeed);
		if(a < 0.1f)movedObject.time = 0;
		else movedObject.time = 1.5f;
		movedObject.transform.position = BezierCurveQuadratic(a, p0, p1, p2);
		
	}
	private Vector3 GetTargetPosition()
	{
		if(targetObject != null) target = targetObject.transform.position;
		
		return target;
		
	}
	private Vector3 BezierCurveCubic(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		// its a mathmetical formulla. the variable names are based of that
		float u = (1.0f - t);
		float uu = u * u;
		float uuu = uu * u;
		float tt = t * t;
		float ttt = tt * t; 
		
		Vector3 p = uuu * p0;
		p += 3 * uu * t  * p1;
		p += 3 * u * tt * p2;
		p += ttt * p3;
		return p;
	}
	public override void OnTriggered (EventReaction eventReaction)
	{
		if(eventReaction.type == EventReaction.Type.LineGuide)
		{
			targetObject = eventReaction.go;
		}
		
	}
	public void OnDrawGizmos()
	{
		if(target != Vector3.zero)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(target, 1.0f);
		}
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(p0, 1.0f);
		Gizmos.DrawWireSphere(p1, 1.0f);
		Gizmos.DrawWireSphere(p2, 1.0f);
		Gizmos.DrawWireSphere(p3, 1.0f);
	}
	private Vector3 BezierCurveQuadratic(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float u = (1.0f - t);
		float uu = u * u;
		float tt = t * t;
		
		Vector3 p = uu * p0;
		p += 2 * u * t * p1;
		p += tt * p2;
		return p;
	}
}
