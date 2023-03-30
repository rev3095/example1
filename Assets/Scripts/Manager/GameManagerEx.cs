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
public enum RoomState
{
	Default,
	Open,
	Knock,
	Break,
	Eavedrop,
	Leave,
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

	//오픈한 방의 상태 -> CurRoom을 0부터 시작하게 만들고, 오픈 상태를 true,false(Defalut)로 받는 
	public bool[] OpenedRoom = new bool[MAX_ROOM_COUNT];
	public RoomState[] RoomStates = new RoomState[MAX_ROOM_COUNT];


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

	public string InputName;
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
		IsFirstStart = startdata.isFirstStart;

		//HP = startdata.hp; , 근데 이제 이 데이터들은 Managers.Data.Character.hp에서 가져오는데 , Character ID에 따라서 달라짐 -> AntCompany 구조 참조


		_path = Application.persistentDataPath + "/SaveData";
		//PlayTime = 0.0f;

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
