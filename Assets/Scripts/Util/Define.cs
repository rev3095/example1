using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Define
{
	public enum DialogueType
    {
		Dialogue,
		Choice,
    }
	public enum UIEvent
	{
		Click,
		Drag,
	}

	public enum Scene
	{
		Unknown=0,
		Title,
		Lobby,
		Battle,
	}
	public enum Sound
	{
		Bgm,
		Sfx,
		MaxCount,
	}

	public enum AnimState
	{
		None,
		Idle,
		Attack,
		Guard,
	}

	public enum CharacterList
    {
		K,
		Chloe,
		Svana,
    }

	public const int CHARACTER_LIST_COUNT = (int)CharacterList.Svana;
	public const int MAX_ROOM_COUNT_10F = 18;
	public enum CheckedRoom
	{
		Default =0,
		Checked,
	}

	public enum InteractionType
    {
		Default =0,
		Open,
		Knock,
		Break,
		Eavedrop,
		Leave,
	}
	public enum StatType
    {
		
    }

	public enum ItemType
    {
		Default =0,
		Use = 1,
		Com,
		Vis,
		Acc,
    }

	public enum InvenTab
	{
		None = 0,
		Usage,
		Combat,
		Visible,
		Accessories,
	}

	//Data Max Count
	public const int MAX_MOVEEVENT_COUNT = 23;
	public const int MAX_INTERACTION_COUNT = 9;
	//UI Texts -> GetText(nameID)로 바로 사용하기 위한 명시적 const 변수
	public const int MAX_INVEN_COUNT = 15;
	public const int MAX_VIS_COUNT = 33;

	public const int NormalButtonText_2 = 10012;
	public const int HardButtonText_2 = 10013;

	public const int KTitle_1 = 50000;
	public const int KTitle_2 = 50001;
	public const int KTitle_3 = 50002;
	public const int KTitle_4 = 50003;
	public const int KTitle_5 = 50004;
	public const int KTitle_6 = 50005;
	public const int KTitle_7 = 50006;
	public const int KTitle_8 = 50007;

	public const int CTitle_1 = 51000;
	public const int CTitle_2 = 51001;
	public const int CTitle_3 = 51002;
	public const int CTitle_4 = 51003;
	public const int CTitle_5 = 51004;
	public const int CTitle_6 = 51005;
	public const int CTitle_7 = 51006;
	public const int CTitle_8 = 51007;

	public const int STitle_1 = 52000;
	public const int STitle_2 = 52001;
	public const int STitle_3 = 52002;
	public const int STitle_4 = 52003;
	public const int STitle_5 = 52004;
	public const int STitle_6 = 52005;
	public const int STitle_7 = 52006;
	public const int STitle_8 = 52007;

}
