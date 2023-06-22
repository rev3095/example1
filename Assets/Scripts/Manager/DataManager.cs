using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;

/*
public class DataManager
{
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();

    public void Init()
    {
        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Utils.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
*/


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public StartData Start;
    public Dictionary<int, TextData> TextDict /*{ get; private set; }*/ = new Dictionary<int, TextData>();

    public Dictionary<int, CharacterSkillData> CharacterSkillDict = new Dictionary<int, CharacterSkillData>();
    //public Dictionary<int, DialogueEventData> Dialogues { get; private set; }

    public Dictionary<int, AccItemData> AccItemDict = new Dictionary<int, AccItemData>();
    public Dictionary<int, ComItemData> ComItemDict = new Dictionary<int, ComItemData>();
    public Dictionary<int, UseItemData> UseItemDict = new Dictionary<int, UseItemData>();
    public Dictionary<int, VisItemData> VisItemDict = new Dictionary<int, VisItemData>();

    public Dictionary<int, InteractionData> InteractionDict = new Dictionary<int, InteractionData>();

    public Dictionary<int, MoveEventData> MoveEventDict = new Dictionary<int, MoveEventData>();

    public Dictionary<int, RoomData> RoomDict = new Dictionary<int, RoomData>();


    public void Init()
    {
        Start = LoadSingleJson<StartData>("StartData");

        TextDict = LoadJson<TextDataLoader, int, TextData>("TextData").MakeDict();

        CharacterSkillDict = LoadJson<CharacterSkillDataLoader, int, CharacterSkillData>("CharacterSKillData").MakeDict();

        AccItemDict = LoadJson<AccItemDataLoader, int, AccItemData>("AccItemData").MakeDict();
        ComItemDict = LoadJson<ComItemDataLoader, int, ComItemData>("ComItemData").MakeDict();
        UseItemDict = LoadJson<UseItemDataLoader, int, UseItemData>("UseItemData").MakeDict();
        VisItemDict = LoadJson<VisItemDataLoader, int, VisItemData>("VisItemData").MakeDict();

        InteractionDict = LoadJson<InteractionDataLoader, int, InteractionData>("InteractionData").MakeDict();

        MoveEventDict = LoadJson<MoveEventDataLoader, int, MoveEventData>("MoveEventData").MakeDict();

        RoomDict = LoadJson<RoomDataLoader, int, RoomData>("RoomData").MakeDict();

    }


    public T LoadSingleJson<T>(string path)
    {
        TextAsset textAsset = Utils.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<T>(textAsset.text);
    }

    public Loader LoadJson<Loader ,Key,Value>(string path) where Loader : ILoader<Key, Value>
    {
        //Utils.Load<>는 무조건 유니티 게임오브젝트를 받게 되므로, TestAsset 타입으로 받게 된다.
        TextAsset textAsset = Utils.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}