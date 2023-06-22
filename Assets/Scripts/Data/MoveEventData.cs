using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveEventData
{
    public int ID;
    public int nameID;
    public int difFatigue;
    public int difHunger;
    public int difStress;
    public bool isBattle;
    public double acqPercent;
}

[Serializable]
public class MoveEventDataLoader : ILoader<int, MoveEventData>
{
	public MoveEventData[] MoveEventDatas;
	public Dictionary<int, MoveEventData> MakeDict()
	{
		Dictionary<int, MoveEventData> dict = new Dictionary<int, MoveEventData>();
		foreach (MoveEventData data in MoveEventDatas)
			dict.Add(data.ID, data);
		return dict;
	}
}
