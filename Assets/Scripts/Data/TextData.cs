using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextData
{
	public int ID;
	public string eng;
}


[Serializable]
public class TextDataLoader : ILoader<int, TextData>
{
	//public List<TextData> texts = new List<TextData>(); -> List 동적으로 추가 가능한데, 제거가 필요 없는 상황에서는 array 사용하기.
	public TextData[] textDatas;

	public Dictionary<int, TextData> MakeDict()
	{
		//<int, string>하고 eng만 쓸거면 해도 되긴 하는데, kor eng 둘다 가져와야되기때문에 저렇게 한거 뿐이다.
		Dictionary<int, TextData> dict = new Dictionary<int, TextData>();
		foreach (TextData texts in textDatas)
			dict.Add(texts.ID, texts);
		return dict;
	}
}