using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Utils.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    //abstract일 경우, struct,interface와 같이, BaseScene을 상속받는 스크립트는 무조건 Clear()를 구현해야 한다는 것을 뜻함. 다만 BaseScene에서, sturct,interface처럼 이후 로직을 쓸 수 없고, 형식만 강제시킴.
    public abstract void Clear();
}
