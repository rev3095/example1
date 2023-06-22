using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VisItemData : IItemData
{
	public int ID;
	public string type;
	public int nameID;
	public string iconPath;
	public string detailPath;


	int IItemData.GetID => this.ID;
	string IItemData.GetIconPath => this.iconPath;
	ItemDataType IItemData.GetItemType => ItemDataType.Vis;
}


[Serializable]
public class VisItemDataLoader : ILoader<int, VisItemData>
{
	public VisItemData[] VisItemDatas;
	public Dictionary<int, VisItemData> MakeDict()
	{
		Dictionary<int, VisItemData> dict = new Dictionary<int, VisItemData>();
		foreach (VisItemData data in VisItemDatas)
			dict.Add(data.ID, data);
		return dict;
	}

}