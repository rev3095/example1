using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComItemData : IItemData
{
	public int ID;
	public int nameID;
	public int difMinHP;
	public int difMaxHP;
	public int difACC;
	public int difDGE;
	public int difCRT;
	public int difSPD;
	public int duration;
	public bool efficacy;
	public float acqPercent;
	public string iconPath;


	int IItemData.GetID => this.ID;
	string IItemData.GetIconPath => this.iconPath;

	ItemDataType IItemData.GetItemType => ItemDataType.Com;
}

[Serializable]
public class ComItemDataLoader : ILoader<int, ComItemData>
{
	public ComItemData[] ComItemDatas;
	public Dictionary<int, ComItemData> MakeDict()
	{
		Dictionary<int, ComItemData> dict = new Dictionary<int, ComItemData>();
		foreach (ComItemData data in ComItemDatas)
			dict.Add(data.ID, data);
		return dict;
	}
	
}