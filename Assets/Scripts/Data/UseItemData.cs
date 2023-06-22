using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UseItemData : IItemData
{
	public int ID;
	public int nameID;
	public int difFatigue;
	public int difHunger;
	public int difStress;
	public double acqPercent;
	public string iconPath;

	int IItemData.GetID => this.ID;
	string IItemData.GetIconPath => this.iconPath;
	ItemDataType IItemData.GetItemType => ItemDataType.Use;
}

[Serializable]
public class UseItemDataLoader : ILoader<int, UseItemData>
{
	public UseItemData[] UseItemDatas;
	public Dictionary<int, UseItemData> MakeDict()
	{
		Dictionary<int, UseItemData> dict = new Dictionary<int, UseItemData>();
		foreach (UseItemData data in UseItemDatas)
			dict.Add(data.ID, data);
		return dict;
	}

}
