using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomData
{
    public int ID;
    public string roomID;
    public bool isItem;
    public int itemID;
}

[Serializable]
public class RoomDataLoader : ILoader<int, RoomData>
{
	public RoomData[] RoomDatas;
	public Dictionary<int, RoomData> MakeDict()
	{
		Dictionary<int, RoomData> dict = new Dictionary<int, RoomData>();
		foreach (RoomData data in RoomDatas)
			dict.Add(data.ID, data);
		return dict;
	}
}

