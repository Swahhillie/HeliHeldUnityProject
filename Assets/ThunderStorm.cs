using UnityEngine;
using System.Collections;

public class ThunderStorm : MonoBehaviour {

	public LineRenderer lightningPrefab;
	
	
	public float minDelay = .5f;
	public float maxDelay = 5.0f;
	public float chancePerTick = .1f;
	public float ticksDelay = .5f;
	
	public float minJump = 20.0f;
	public float maxJump = 50.0f;
	
	public int minPoints  = 2;
	public int maxPoints  =  7;
	public float straightNess = 5;
	
	public int minBranches;
	public int maxBranches;
	
	
	private float _lastTick = 0;
	private float _lastLighting = 0;
	
	
	private delegate void LightningTick ();
	//private event LightningTick tick;
	
	
	private void Start()
	{
		//tick += OnLightningTick;
	}
	private void Update()
	{
		if(_lastTick + ticksDelay < Time.time)
		{
			_lastTick = Time.time;
			OnLightningTick();
		}
	}
	private void OnLightningTick()
	{
		float timeSinceLastLightning = Time.time - _lastLighting;
		if(timeSinceLastLightning > minDelay)
		{
			if(timeSinceLastLightning > maxDelay)
			{
				CreateStrike();
			}
			else
			{
				if(Random.value < chancePerTick)
				{
					CreateStrike();
				}
			}
		}	
		else{
			//to close to last lightning
		}
	}
	private void CreateStrike()
	{
		_lastLighting = Time.time;
		int branches = Random.Range(minBranches, maxBranches);
		CreateBranch(lightningPrefab);
		StartCoroutine(DisableStrike(lightningPrefab));
	}
	
	private void CreateBranch(LineRenderer lineRenderer)
	{
		lineRenderer.enabled = true;
		int points = Random.Range(minPoints, maxPoints);
		Vector3 point = lineRenderer.transform.position;
		lineRenderer.SetVertexCount(points);
		lineRenderer.SetPosition(0, point);
		for(int i = 1; i < points; i++)
		{
			Vector3 p = Random.insideUnitSphere * maxJump;
			if(Vector3.SqrMagnitude(p) < minJump * minJump)
			{
				p = Vector3.Normalize(p) * minJump;
			}
			
			p.y = p.y > 0? -p.y : p.y; // make p.y negative
			point += p;
			lineRenderer.SetPosition(i, point);
		}
	}
	private IEnumerator DisableStrike(LineRenderer lineRenderer)
	{
		yield return new WaitForSeconds(1.0f);//magic
		lineRenderer.enabled = false;
	}
}
