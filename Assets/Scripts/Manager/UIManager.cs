using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>(); //컴포넌트를 담는 스택 (Popup Canvas UI)

    public UI_Scene SceneUI { get; private set; }

    public UI_Popup PopupUI { get; private set; }

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);

        //world space로 URP적용받도록 진행 -> 저 부분은 각각 따로 진행하기
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            canvas.renderMode = RenderMode.WorldSpace;

        }

        canvas.overrideSorting = true;

        if (sort) //Popup UI : 1씩 증가
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else //Scene UI : 0으로 고정
        {
            canvas.sortingOrder = 0;
        }
    }




    //굳이 버튼이 아니더라도 다른것도 되는데 일단 버튼이 급해서 버튼 로드할때 이거 쓰기
    // SubItem -> 나에게 있어서는 Button (MakeButton이라고 붙여도 됨 -> ChoicePopup에서 사용됨) , 팩토리처럼 인스턴스 담당 함수
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject prefab = Utils.Load<GameObject>($"Prefabs/UI/SubItem/{name}");

        GameObject go = Object.Instantiate(prefab);
        if (parent != null)
            //parent : canvas가 없으면 얘가 표시될 수 없으므로, parent 필요
            go.transform.SetParent(parent);

        //~(Clone) 붙여진거 빼고 이름 설정됨.
        go.name = name;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = prefab.transform.position;

        return Utils.GetOrAddComponent<T>(go);
    }


    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Utils.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utils.GetOrAddComponent<T>(go);
        SceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null, Transform parent = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name)) //이름을 받지 않았다면, T로 진행
            name = typeof(T).Name;

        GameObject prefab = Utils.Load<GameObject>($"Prefabs/UI/Popup/{name}");

        GameObject go = Utils.Instantiate($"UI/Popup/{name}");
        T popup = Utils.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        if (parent != null)
            go.transform.SetParent(parent);
        else if (SceneUI != null)
            go.transform.SetParent(SceneUI.transform);
        else
            go.transform.SetParent(Root.transform);

        go.name = name;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = prefab.transform.position;

        return popup;
    }

    public T FindPopup<T>() where T : UI_Popup
    {
        return _popupStack.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
    }

    public T PeekPopupUI<T>() where T : UI_Popup
    {
        if (_popupStack.Count == 0)
            return null;

        return _popupStack.Peek() as T;
    }

    public void ClosePopupUI(UI_Popup popup) //안전 차원에서, 후술할 오류를 방지하기 위한 Debug.Log 생성 함수
    {
        if (_popupStack.Count == 0) //비어있는 스택이라면 삭제 불가
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!"); // 스택의 가장 위에있는 Peek() 것만 삭제할 수 잇기 때문에 popup이 Peek()가 아니면 삭제 못함
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Utils.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        SceneUI = null;
    }
}
