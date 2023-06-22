using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AccItemData : IItemData
{
    public int ID;
	public int nameID;
	public string iconPath;
	public int detailID;


	int IItemData.GetID => this.ID;
	string IItemData.GetIconPath => this.iconPath;
	ItemDataType IItemData.GetItemType => ItemDataType.Acc;
}

[Serializable]
public class AccItemDataLoader : ILoader<int, AccItemData>
{
	public AccItemData[] AccItemDatas;
	public Dictionary<int, AccItemData> MakeDict()
	{
		Dictionary<int, AccItemData> dict = new Dictionary<int, AccItemData>();
		foreach (AccItemData data in AccItemDatas)
			dict.Add(data.ID, data);
		return dict;
	}
}