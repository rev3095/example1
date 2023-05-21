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

	public enum MouseEvent
    {
		Press,
		Click
    }

	public enum Scene
	{
		Unknown, //디폴트
		Title, //타이틀
		Lobby, //맵 선택 가능
		Battle, // 전투
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
		Default,
		Open,
		Knock,
		Break,
		Eavedrop,
		Leave,
	}

	public enum StatType
    {

    }

	public const int NormalButtonText_2 = 10012;
	public const int HardButtonText_2 = 10013;



}
