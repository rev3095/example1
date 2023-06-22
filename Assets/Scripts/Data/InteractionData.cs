using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InteractionData
{
	public int ID;
	public string type;
	public int difFatigue;
	public int difHunger;
	public int difStress;
	public double battlePercent;
}

[Serializable]
public class InteractionDataLoader : ILoader<int, InteractionData>
{
	public InteractionData[] InteractionDatas;
	public Dictionary<int, InteractionData> MakeDict()
	{
		Dictionary<int, InteractionData> dict = new Dictionary<int, InteractionData>();
		foreach (InteractionData data in  InteractionDatas)
			dict.Add(data.ID, data);
		return dict;
	}
}

