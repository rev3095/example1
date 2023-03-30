using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager
{
	int _order = -20; //전부 Popup UI로만 처리한다면, -20으로 설정해도 되는데 SceneUI까지 있기 때문에 겹치지 않도록 10으로 놓고 10,11.. 진행 

	Stack<UI_Popup> _popupStack = new Stack<UI_Popup>(); //오브젝트가 아닌, 컴포넌트를 담는 스택 (Popup Canvas UI)

	public UI_Scene SceneUI { get; private set; }

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

	//_order++처리를 ShowPopupUI 에서 안하고 SetCanvas에서 해준 이유는,
	//캔버스 프리팹을 통해 생성되는 경우(ShowPopupUI 사용해서) 말고! 이미 게임 시작전부터 static 하게 존재하고 있던,
	//프리팹으로부터 생성된게 아닌 팝업 UI일 경우도 있을 수 있기 때문이다.
	//그런 경우엔 ShowPopupUI를 거치지 않기 때문에 sort order 관리가 안될 것이다. 그래서 SetCanvas에서 해준 것!
	public void SetCanvas(GameObject go, bool sort = true)
	{
		Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);
		
		//sort order를 받을 수 있는 모드
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;

		//

		canvas.overrideSorting = true; //캔버스 안에 캔버스 중첩 경우 ( 부모 캔버스가 어떤 값을 가지던, 나 자신은 고유한 오더값을 가지기 위해 )

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
		// SubItem -> 나에게 있어서는 Button
	public T MakeButton<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject prefab = Utils.Load<GameObject>($"Prefabs/UI/Button/{name}");

		GameObject go = Object.Instantiate(prefab);
		if (parent != null)

			//parent : canvas가 없으면 얘가 표시될 수 없으므로, parent 필요
			go.transform.SetParent(parent);

		go.name = name;

		// 이 부분은 transform으로 가져올 필요가 없는게, 내가 이미 Scale with ScreenSize 해놔가지고 그냥 생성만 시키면 알아서 할당이 될 텐데 일단 냅둬보겠음.

		//이거 그냥 현재 가지고 있는 오브젝트의 Transform 
		go.transform.localScale = Vector3.one;

		//
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

	
	// where T로 popup,sceneUI 분리 진행 후, 프리펩을 Instantiate로 생성하고 popup에 바인딩 후 스택 추가 후 리턴
	//Root 프로퍼티를 통해 @UI_Root의 자식으로 넣어줌. (UI관련 오브젝트는 DontDestroyOnLoad로 여기서 자식 요소로 추가되거나 삭제됨
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
