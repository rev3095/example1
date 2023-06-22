using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Define;


//클래스도 배열로 사용 가능, 이 요소들을 가지고 인스턴스가 생성되는 것.
//PlayerState : K 기준으로 생성됨, 나머지 캐릭터들은 캐릭터 설정에 맞게 값 수정해주기.
[Serializable]
public class PlayerState
{
	public AnimState state = AnimState.None;

}

[Serializable]
public class ItemState
{
	public string ItemType;
	public int ItemTypeIdx;
}


[Serializable]
public class GameData
{
	//입력받는 이름 : LobbyScene-DialoguePopup
	public string Name;
	public PlayerState playerstate;

	//Lobby에 표시되는 stats
	public int CurFatigue;
	public int MaxFatigue;
	public int CurHunger;
	public int MaxHunger;
	public int CurStress;
	public int MaxStress;

	//현재 플로어,방
	public int CurFloor;
	public int CurRoom;

	public CheckedRoom[] checkedRoom = new CheckedRoom[MAX_ROOM_COUNT_10F];

	public int nowMP;

	//크기 제한 X , 그러나 초기화 부분 존재 & 보여지는 부분은 MAX_INVEN_COUNT =15 만큼 MakeSubItem
	public List<UseItemData> UseItemList = new List<UseItemData>();
	public List<ComItemData> ComItemList = new List<ComItemData>();
	public List<VisItemData> VisItemList = new List<VisItemData>();
	public List<AccItemData> AccItemList = new List<AccItemData>();

	// 15칸인데 => 30개로 늘리자 !
	// define 15 -> 30
	// 아이템 5개
	// 15개 => Array List remove 15개 => 15칸인데
	// for (int i < AccItemList .Length; i++)
	// 

	// const int SLOT_COUNT =15;
	// for (int i < SLOT

	//list[0] [1] [2] 

	// 0을 지운다!

	//list[1->0] [2->1]

	// 정렬이된다


	// Array 
	// [0][1][2]

	// 0을 지운다

	// [0] = -1

	// [0][1][2]



	//List로 다시 바꾸기? => 사용함에 있어서 편리함. 
	//2가지 

	public int UpgradePosession;

	//난이도 선택
	public bool IsNormal = false;
	public string Difficulty;

	//최소 데미지, 최대 데미지 ( 데미지 공식 적용 이전에 적용될 1턴당 데미지 값, 추후 스탯에 맞게 수정 필요 )
		//그리고 그것과 별개로 MINDMG MAXDMG 또한 구현할 필요가 있음.

	//Managers.Data.Character의 Stat
	public int HP;
	public int MAXHP;
	public int DGE;
	public int ACC;
	public int PRO;
	public int SPD;

	public int DMG;
	public int MINDMG;
	public int MAXDMG;

	public double CRT;

	public double BLEED;
	public double BLOCKSIGHT;
	public double MARK;
	public double STUN;
	public double DOWN;
	public double MOVE;
	public double DEATH;

	//Managers.Data.Character의 Resistance
	public double RESBLEED;
	public double RESBLOCKSIGHT;
	public double RESMARK;
	public double RESSTUN;
	public double RESDOWN;
	public double RESMOVE;
	public double RESDEATH;

	//처음 시작시 1회성 이벤트 동작을 확인할 bool값
	public bool IsFirstStart;

	//Slot에 표시될 저장 시점의 시간 y,m,d 순으로 표시됨.
	public string time;

}

public class GameManagerEx
{
	public GameData _gameData = new GameData();
	public GameData SaveData { get { return _gameData; } set { _gameData = value; } }

	public CheckedRoom[] checkedRoom
    {
        get { return _gameData.checkedRoom; }
		set { _gameData.checkedRoom = value; }
    }
    #region 스탯


	//클래스가 다르니까 프로퍼티 Name과 _gameData.Name이 같아도 상관 없다.
	public string Name
	{
		get { return _gameData.Name; }
		set { _gameData.Name = value; }
	}

	public PlayerState playerstate
	{
		get { return _gameData.playerstate; }
		set { _gameData.playerstate = value; }
	}

	
	public int CurFatigue
	{
		get { return _gameData.CurFatigue; }
		set { _gameData.CurFatigue = value; }
	}
	//_gameData.CurFatigue = Mathf.Clamp(value, 0, MaxFatigue); 사용할지는 미지수.
	public int MaxFatigue
	{
		get { return _gameData.MaxFatigue; }
		set { _gameData.MaxFatigue = value; }
	}

	//stress가 변화되었을 때 실행되는 public Action OnStressChanged; 사용할지는 미지수.
	public int CurStress
	{
		get { return _gameData.CurStress; }
		set { _gameData.CurStress = value; }
	}

	public int MaxStress
	{
		get { return _gameData.MaxStress; }
		set { _gameData.MaxStress = value; }
	}
	public int CurHunger
	{
		get { return _gameData.CurHunger; }
		set { _gameData.CurHunger = value; }
	}

	public int MaxHunger
	{
		get { return _gameData.MaxHunger; }
		set { _gameData.MaxHunger = value; }
	}


	//public float FatiguePercent { get { return Hp * 100 / (float)MaxFatigue; } }
	//public float StressPercent { get { return Stress * 100 / (float)MaxStress; } }

	public int CurFloor
	{
		get { return _gameData.CurFloor; }
		set { _gameData.CurFloor = value; }
	}

	public int CurRoom
    {
		get { return _gameData.CurRoom; }
		set { _gameData.CurRoom = value; }
    }

	public int nowMP
    {
		get { return _gameData.nowMP; }
        set { _gameData.nowMP = value; }
    }


	public List<UseItemData> UseItemList
    {
        get { return _gameData.UseItemList; }
		set { _gameData.UseItemList = value; }
    }


	public List<ComItemData> ComItemList
    {
		get { return _gameData.ComItemList; }
		set { _gameData.ComItemList = value; }
    }

	public List<VisItemData> VisItemList
	{
		get { return _gameData.VisItemList; }
		set { _gameData.VisItemList = value; }
    }
	
	public List<AccItemData> AccItemList
    {
		get { return _gameData.AccItemList; }
		set { _gameData.AccItemList = value; }
    }

	public int UpgradePossesion
    {
		get { return _gameData.UpgradePosession; }
        set { _gameData.UpgradePosession = value; }
    }
    public bool IsNormal
	{
		get { return _gameData.IsNormal; }
		set { _gameData.IsNormal = value; }

	}

	public string Difficulty
    {
		get { return _gameData.Difficulty; }
		set { _gameData.Difficulty = value; }
	}

	public int HP
	{
		get { return _gameData.HP; }
		set { _gameData.HP = value; }

	}
	public int MAXHP
    {
		get { return _gameData.MAXHP; }
		set { _gameData.MAXHP = value; }

	}

	public int DGE
	{
		get
		{
			//foreach (StatData statData in Managers.Data.Stats.Values)
			//	attack += GetStat(statData.type) * statData.difAtk;

			//float incPercent = 0.0f;
			//foreach (StatData statData in Managers.Data.Stats.Values)
			//	incPercent += GetStat(statData.type) * statData.difAllAtkPercent;

			//attack += (int)(attack * incPercent);
			//if (attack < 0)
			//	attack = 0;

			return _gameData.DGE;
		}


		set
		{
			_gameData.DGE = value;
		}
	}

/// <summary>
/// 	public int ACC
///	{
///		get
///		{
///			int Acc = Managers.Data.Start.acc;
///			return Acc;
///		}
///	}
/// </summary>
	public int ACC
	{
		get { return _gameData.ACC; }
		set { _gameData.ACC = value; }
	}
	public int PRO
	{
		get { return _gameData.PRO; }
		set { _gameData.PRO = value; }
	}
	public int SPD
	{
		get { return _gameData.SPD; }
		set { _gameData.SPD = value; }
	}

	public int DMG
	{
		get { return _gameData.DMG; }
		set { _gameData.DMG = value; }
	}
	public int MINDMG
    {
		get { return _gameData.MINDMG; }
		set { _gameData.MINDMG = value; }
	}
	public int MAXDMG
	{
		get { return _gameData.MAXDMG; }
		set { _gameData.MAXDMG = value; }
	}

	public double CRT
	{
		get { return _gameData.CRT; }
		set { _gameData.CRT = value; }
	}


	public double BLEED
	{
		get { return _gameData.BLEED; }
		set { _gameData.BLEED = value; }
	}
	public double BLOCKSIGHT
	{
		get { return _gameData.BLOCKSIGHT; }
		set { _gameData.BLOCKSIGHT = value; }
	}
	public double MARK
	{
		get { return _gameData.MARK; }
		set { _gameData.MARK = value; }
	}
	public double STUN
	{
		get { return _gameData.STUN; }
		set { _gameData.STUN = value; }
	}
	public double DOWN
	{
		get { return _gameData.DOWN; }
		set { _gameData.DOWN = value; }
	}
	public double MOVE
	{
		get { return _gameData.MOVE; }
		set { _gameData.MOVE = value; }
	}
	public double DEATH
	{
		get { return _gameData.DEATH; }
		set { _gameData.DEATH = value; }
	}

	public double RESBLEED
    {
		get { return _gameData.RESBLEED; }
		set { _gameData.RESBLEED = value; }
	}
	public double RESBLOCKSIGHT
    {
		get { return _gameData.RESBLOCKSIGHT; }
		set { _gameData.RESBLOCKSIGHT = value; }
	}

	public double RESMARK
    {
		get { return _gameData.RESMARK; }
		set { _gameData.RESMARK = value; }
	}

	public double RESSTUN
    {
		get { return _gameData.RESSTUN; }
		set { _gameData.RESSTUN = value; }
	}

	public double RESDOWN
    {
		get { return _gameData.RESDOWN; }
		set { _gameData.RESDOWN = value; }
	}

	public double RESMOVE
    {
		get { return _gameData.RESMOVE; }
		set { _gameData.RESMOVE = value; }
	}
	public double RESDEATH
    {
		get { return _gameData.RESDEATH; }
		set { _gameData.RESDEATH = value; }
	}


	public bool IsFirstStart
    {
		get { return _gameData.IsFirstStart; }
		set { _gameData.IsFirstStart = value; }
	}
	/*
	public int GetStat(StatType type)
	{
		switch (type)
		{
			case StatType.MaxFatigue:
				return MaxFatigue;
			case StatType.:
				return WorkAbility;
			case StatType.Likeability:
				return Likeability;
			case StatType.Luck:
				return Luck;
			case StatType.Stress:
				return Stress;
		}

		return 0;
	}
	*/

	/*
	public PlayerState GetPlayerState(PlayerState type)
	{
		return _gameData.playerstate[(int)type];
	}
	*/


#endregion


    #region 시간

/*
public float PlayTime
{
	get { return _gameData.PlayTime; }
	set { _gameData.PlayTime = value; }
}
*/
public class TimeUtils
	{
		public static string GetCurrentDate()
		{
			return DateTime.Now.ToString(("yyyy-MM-dd"));
		}

		public static string GetTime()
		{
			return DateTime.Now.ToString(("HH:mm"));
		}
	}

	#endregion


	#region 배틀



	#endregion


	#region UI

	IEnumerator FullTransition()
	{
		yield return null;
	}

	IEnumerator CenterTransition()
	{
		yield return null;

	}

	#endregion

	public string InputName;

	//어차피 여기서 Item 배열을 new 해줄 필요가 없음. 게임 데이터를 시작 시 새로 가지게 되기 때문이다..  맞나? 초기화 안되면, 새로 new 써서 작성하기.
	public void Init()
	{
		StartData startdata = new StartData();
		startdata = Managers.Data.Start;

		//SaveData -> _gameData에 담겨지는 항목들 ( Init 시 실행됨 )
		Name = "NoName";
		InputName = "None";
		CurFatigue = startdata.curFatigue;
		MaxFatigue = startdata.maxFatigue;
		CurHunger = startdata.curHunger;
		MaxHunger = startdata.maxHunger;
		CurStress = startdata.curStress;
		MaxStress = startdata.maxStress;
		CurFloor = startdata.curFloor;
		CurRoom = startdata.curRoom;
		//nowMP = startdata.nowMP;
		IsFirstStart = startdata.isFirstStart;

		nowMP = 0;

		UpgradePossesion = 0;

		UseItemList = new List<UseItemData>();
		ComItemList = new List<ComItemData>();
		VisItemList = new List<VisItemData>();
		AccItemList = new List<AccItemData>();

		//_UseItemList.Clear();
		//_ComItemList.Clear();
		//HP = startdata.hp; , 근데 이제 이 데이터들은 Managers.Data.Character.hp에서 가져오는데 , Character ID에 따라서 달라짐 -> AntCompany 구조 참조

		for (int i = 0; i < MAX_ROOM_COUNT_10F; i++)
		{
			checkedRoom[i] = CheckedRoom.Default;
		}


		_path = Application.persistentDataPath + "/SaveData";
		//PlayTime = 0.0;
		
	}

	//Temp List -> 요소는  Array가 동적으로 삭제, 추가되지만 append가 불가능하여, 그냥 리스트로 한번 cast시킨뒤 다시 array를 반환하는 아이템 추가 함수 제작
	private List<UseItemData> _UseItemList = new List<UseItemData>();
	private List<ComItemData> _ComItemList = new List<ComItemData>();


	//필연적으로 하나밖에 제작이 안되는거 같음. 두 개 이상 생성될때는 두 번 호출시키기.
	//
	public void AddItem(string type, int idx)
    {
		switch (type)
        {
			case "Use":
				//CheckItemIDX("Use");
				Managers.Game.UseItemList.Add(Managers.Data.UseItemDict[idx]);
				break;

			case "Com":
				//CheckItemIDX("Com");
				Managers.Game.ComItemList.Add(Managers.Data.ComItemDict[idx]);
				break;

			case "Vis":
				Managers.Game.VisItemList.Add(Managers.Data.VisItemDict[idx]);
				break;

			case "Acc":
				Managers.Game.AccItemList.Add(Managers.Data.AccItemDict[idx]);
				break;

			default:
				break;

		}

    }
	public int CheckItemIDX(string type)
	{
		int Idx = 0;

		switch (type)
		{
			case "Use":
				for (int i = 0; i < 15; i++)
				{
					if (Managers.Game.UseItemList[i] != null)
					{
						Debug.Log("i++");
					}
					else
					{
						Idx = i;
						break;

					}

				}
				break;

			case "Com":
				for (int i = 0; i < 15; i++)
				{
					if (Managers.Game.ComItemList[i] != null)
					{
						Debug.Log("i++");
					}
					else
					{
						Idx = i;
						break;

					}

				}
				break;

		}


		return Idx;

	}
	#region Save & Load	
	public string _path;
	public int nowSlot;

	//무조건 nowSlot이, SelectScene과 동일한 역할을 하는, LoadGamePopup에서 이루어져야 함.
	public void SaveGame()
	{
		string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
		File.WriteAllText(_path + nowSlot.ToString()+".json", jsonStr);
		Debug.Log($"Save Game Completed : {_path + nowSlot.ToString()}");
		Debug.Log("저장 경로: " + _path + nowSlot.ToString() + ".json");
	}

	public void CopyGame(int copySlot, int pasteSlot)
    {
		string fileStr = File.ReadAllText(_path + copySlot.ToString() + ".json");
		File.WriteAllText(_path + pasteSlot.ToString() + ".json", fileStr);

	}

	public void DeleteGame()
    {
		File.Delete(_path + nowSlot.ToString() + ".json");
    }
	public void LoadGame()
	{
		string fileStr = File.ReadAllText(_path + nowSlot.ToString()+".json");
		Managers.Game.SaveData = JsonUtility.FromJson<GameData>(fileStr);
		/*
		GameData data = JsonUtility.FromJson<GameData>(fileStr);
		if (data != null)
		{
			Managers.Game.SaveData = data;
		}

		Debug.Log($"Save Game Loaded : {_path+nowSlot.ToString()}");
		*/
	}

	public void DataClear()
    {
		nowSlot = -1;
		Managers.Game.SaveData = new GameData(); //현재 게임 데이터

    }
	#endregion
}
