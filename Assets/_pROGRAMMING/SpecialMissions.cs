using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpecMission
{
	private string name;
	private string text;
	private float time;
	private string win;
	private string fail;
	private int points;
	private int count=1;
	
	
	public SpecMission(string aName,string aText,float aTime,int Points,string Winmessage,string Failmessage,int aCount=1)
	{
		name=aName;
		text=aText;
		time=aTime;
		points=Points;
		win=Winmessage;
		fail=Failmessage;
		count=aCount;
	}
	
	public bool update(float step)
	{
		time-=step;
		if(time<=0)
		{
			if(count>0)
			{
				//display failing text
			}
			return false;	
		}
		else
		{
			return true;	
		}
	}
	
	public void counting()
	{
		--count;
		if(count<=0)
		{
			time=0;
			//add points to highscore
			//display winning text
		}
	}
}

public class SpecialMissions 
{
	Dictionary<string,SpecMission> specops = new Dictionary<string,SpecMission>();
	
	
	public void addMission(string name, SpecMission mission)
	{
		specops.Add(name,mission);
	}
	
	public void update(float step)
	{
//		IDictionaryEnumerator it = specops.GetEnumerator();
//		while(it.MoveNext())
//		{
//			if(!specops[it.Key.ToString()].update(step))
//			{
//				specops.Remove(it.Key.ToString());
//			}
//		}
		foreach(KeyValuePair<string, SpecMission> p in specops){
			if(!p.Value.update(step)){
				specops.Remove(p.Key);
				return;
			}
		}
	}
	
	public void count(string name)
	{
		specops[name].counting();	
	}

}
