using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Texts
{
	public int ID;
	public string eng;
}

/*
[Serializable]
public class TextData
{
	// list 동적으로 추가, 제거가 필요없는 상황에서는 array로 씁시다.
	//public Text[] texts;
	public List<Text> texts = new List<Text>();

	public Dictionary<int, Text> MakeDict()
	{
		Dictionary<int, Text> dict = new Dictionary<int, Text>();
		foreach (Text text in texts)
			dict.Add(text.ID, text);
		return dict;
	}
}
*/

[Serializable]
public class TextData : ILoader<int,Texts>
{
	//public List<Texts> texts = new List<Texts>();
	public Texts[] texts;

	public Dictionary<int, Texts> MakeDict()
	{
		Dictionary<int, Texts> dict = new Dictionary<int, Texts>();
		foreach (Texts text in texts)
			dict.Add(text.ID, text);
		return dict;
	}
}