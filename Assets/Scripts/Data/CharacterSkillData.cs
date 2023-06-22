using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterSkillData
{
	public int ID;
	public int nameID;
	public string iconPath;
	public string type;
	public double applyDmg;
	public double difAcc;
	public double difCrt;
}

[Serializable]
public class CharacterSkillDataLoader : ILoader<int, CharacterSkillData>
{
	public CharacterSkillData[] CharacterSkillDatas;
	public Dictionary<int, CharacterSkillData> MakeDict()
	{
		Dictionary<int, CharacterSkillData> dict = new Dictionary<int, CharacterSkillData>();
		foreach (CharacterSkillData data in CharacterSkillDatas)
			dict.Add(data.ID, data);
		return dict;
	}
}
