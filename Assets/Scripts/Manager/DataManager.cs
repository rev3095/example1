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
    public StartData Start { get; private set; }
    //public Dictionary<int, StatData> Stats { get; private set; }
    //public Dictionary<int, Text> TextDict { get; private set; }
    public Dictionary<int, Texts> TextDict { get; private set; } = new Dictionary<int, Texts>();
    //public Dictionary<int, DialogueEventData> Dialogues { get; private set; }


    public void Init()
    {
        Start = LoadSingleJson<StartData>("StartData");

        // -> string 나온걸 바로 받아주기. 그럼 올바른 형식으로 Json이 들어감. 검증 과정.
        //var data = new TextData();
        //data.texts = new Text[]
        //{
        //    new Text()
        //    {
        //        ID = 1,
        //        eng = "NoName",
        //    }
        //};

        //var test = JsonUtility.ToJson(data);
        //Debug.Log(test);

        //TextDict = LoadJson("TextData").MakeDict();
        TextDict = LoadJson<TextData, int, Texts>("TextData").MakeDict();
        //StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();


        //var dialogueLoader = LoadJson<DialogueEventDataLoader, int, DialogueEventData>("DialogueEventData");
        //Dialogues = dialogueLoader.MakeDic();


    }

    // C# Template 대문자 !
    // C# Value -> T로 변경 (for문에서 int i 같은 애)
    public T LoadSingleJson<T>(string path)
    { 
        TextAsset textAsset = Utils.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<T>(textAsset.text);
    }

    /*
    TextData LoadJson(string path)
    {
        TextAsset textAsset = Utils.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<TextData>(textAsset.text);
    }
    */

    //private 함수 
    //(private) T 함수이름<T>()
    //제너릭인데 타입(T)이 무조건 Loader타입이어야 된다는거 +
    //이 함수에서 타입을 정의할 수 있구나. 여기서 정의가 된 것
    //Loader라는 T를 정의할때, where로 Loader : ILoader로 추가 정의

    //Loader라는 타입은 LoadJson에서밖에 못쓴다.
    //Loader를 T로 바꿔주는게 보기더 편할듯? 그리고 T:ILoader 니까, 실질적으로 Key,Value를 무조건 가짐.

    //public Loader LoadJson<Loader, Key, Value>(string path) where Loader :ILoader<Key, Value>
    //{
    //    TextAsset textAsset = Utils.Load<TextAsset>($"Data/{path}");
    //    return JsonUtility.FromJson<Loader>(textAsset.text);
    //}

    public T LoadJson<T,Key,Value>(string path) where T : ILoader<Key, Value>
    {
        //Utils.Load<>는 무조건 유니티 게임오브젝트를 받게 되므로, TestAsset 타입으로 받게 된다.
        TextAsset textAsset = Utils.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<T>(textAsset.text);
    }
}

