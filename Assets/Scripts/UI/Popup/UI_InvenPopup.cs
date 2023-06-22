using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;

#region LOG
/* ===================
 * 최대한 주석을 제외하기 위해, 주석 관련 세부 내용은 LOG에 정리.
 
   #6.18
    아이템 정보 갱신은 어차피 현재 팝업에서는 갱신될 일이 없이, 아이템 획득 시점에서만 갱신되므로 Refresh()를 굳이 시키지는 않음.
    특정 UI_Inven_Item이 사용되었을때, 해당 코드의 InfoPopup의 OnClickYesButton이 생성되었을때, 특정 아이템을 Destroy -> 현재 탭에서의 MakeLastSubItem(nowTab)
    
    아이템이 클릭되었을때 정보 세팅은 Text가 Bind되어있는 해당 팝업에서 표시되어야 하므로, SetItemText()실행 시 표시됨. 해당 함수 호출은 Item 내부의 OnClickItem에서 실행됨.

    [리팩토링 List]

    1. InvenPopup / Item / InfoPopup이 가지고 있어야 될 이상적인 데이터 구조. 현재 데이터와 새로운 객체 => 현제 데이터를 가지고 있는 팝업에서 데이터를 주고받고 해야되는데 개체 참조를 어떻게 하는가?
        ex_1) (Managers.UI.SceneUI as UI_SelectStageScene).InventoryPopupUI.SetInfo();
        ex_2) 다른 Inven_Item, InfoPopup에서 InvenPopup = new InvenPopup을 실행한 뒤, 새로 만들어진 개체에서 메서드를 이용한다. => 이 경우 아까 처음에 적었던 의문이 해결되지 않음. 포인터를 사용해야 될까?

    2. ItemClass에 대한 고찰 -> 한번에 ItemData를 가지고 ItemType으로 구분해야, SetInfo 함수를 타입별로 여러 개 만들거나, 따로 구분할 필요 없이 switch문으로 표현하면 되니까.
        만약에 현재 구조도 괜찮다고 한다면, 인자값을 어떻게 주고받아야 하는가?

        ex) Item Class가 4개이므로, Inven_Item에서 인자를 전달받을때 , 어쩔 수 없이 직관적으로 각 4개의 SetInfo 함수 제작. (다음 번에는, DataLoader부터 포함하여, 아이템 클래스도 합칠 것)


    3.     //MakeSubItem을 만들어줄때 이미 다 아이템 세팅이 끝난 상태이기 때문에, 현재 NowItem 데이터를 그대로 보내고, InvenInfoPopup에서, 데이터 검사해서 맞는 팝업을 띄워주면 된다.
        //다만, VisTab과 AccTab의 경우 InfoPopup이 뜨지 않음. -> UI_Inven_Item에서 확인하기.
        //Managers.UI.ShowPopupUI<UI_InvenInfoPopup>().SetInfo(NowItem);

        //InvenPopup이 생성될 때, 미리 다른 곳에서 (1. IsFirstStart에서 제공된 두 개의 아이템 / 2. 배틀이나 Interaction이 끝난 뒤 지급되는 아이템 ) 제공된 아이템 정보를 배열에 세팅함.


    이 부분은 Inven_Item 내부에서 OnClick 두번 되었을때 실행시키기.

    [구현 List]
    1. VisItem Lock/UnLock System (Source Code : AlicePang_Inventory)

    2. Vis Watch Button을 통해, VisItem의 detailPath 출력 -> Scale 설정시키기. ( InfoPopup으로 실행되는게 아닌, 자체적으로 UI_Inven_Item이 Vis Type일 경우, 현재 팝업에서 Watch 버튼 실행시키기
    
    3. DoubleClick 상태에서 왼쪽 클릭시 : 아이템 사용 (UI_Info Popup. 사용 팝업) , 좌클릭시 (UI_Info Popup. 삭제 팝업)
    ==================
    #6.20
    
        // VisItem은 Lock 해금 System이므로, 나중에 한번에 Set 후, 해금 함수 실행 시 Unlock.

        //if 인덱스 0도 없는 길이가 0일때는,return null; 아닐 경우, 현재 아이템 가지고 있는 것 생성, 나머지는 정보 세팅 없이 else로 세팅.
        //배열 -> 리스트로 변경, 로직도 변경해서 디버깅 다시 필요.

   =================== */
#endregion

//ItemManager는 따로 뺌.
public class UI_InvenPopup : UI_Popup
{

    #region enum Texts/Buttons/GameObjects
    public enum Texts
    {
        UseItemTitleText,
        UseItemDetailText,
        UseItemCurValueText,

        ComItemTitleText,
        ComItemDetailText,
        ComItemCurValueText,

        VisItemTitleText,
        VisItemCurValueText,

        AccItemTitleText,
        AccItemDetailText,
        AccItemCurValueText,

        VisTabLeftButtonText,
        VisTabRightButtonText,

    }

    enum Buttons
    {
        CloseButton,

        UsageTabButton,
        CombatTabButton,
        VIsibleTabButton,
        AccessoriesTabButton,

        VisTabLeftButton,
        VisTabRightButton,
        VisWatchButton,

    }

    enum GameObjects
    {
        UseTab,
        ComTab,
        VisTab,
        AccTab,

        Vis1,
        Vis2,
        Vis3,

        UseItems,
        ComItems,
        AccItems,

    }
    #endregion

    public static InvenTab nowTab = InvenTab.None;

    //Memory
    private UseItemData _UseItemData;
    private ComItemData _ComItemData;
    private VisItemData _VisItemData;
    private AccItemData _AccItemData;

    private IItemData _ItemData;

    [SerializeField] private int slotNum = 15;
    [SerializeField] private int remainSlotNum = 3;

    //Index
    private int curVisIndex = 0;

    public void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(PointerEventData => OnClickCloseButton());

        GetButton((int)Buttons.UsageTabButton).gameObject.BindEvent((PointerEventData data) => Showtab(InvenTab.Usage));
        GetButton((int)Buttons.CombatTabButton).gameObject.BindEvent((PointerEventData data) => Showtab(InvenTab.Combat));
        GetButton((int)Buttons.VIsibleTabButton).gameObject.BindEvent((PointerEventData data) => Showtab(InvenTab.Visible));
        GetButton((int)Buttons.AccessoriesTabButton).gameObject.BindEvent((PointerEventData data) => Showtab(InvenTab.Accessories));

        GetButton((int)Buttons.VisTabLeftButton).gameObject.BindEvent(PointerEventData => OnClickVisTabLeftButton());
        GetButton((int)Buttons.VisTabRightButton).gameObject.BindEvent(PointerEventData => OnClickVisTabRightButton());
        GetButton((int)Buttons.VisWatchButton).gameObject.BindEvent(PointerEventData => OnClickVisWatchButton());


        #region TEST 아이템 등록

        //TEST
        Managers.Game.UseItemList = new List<UseItemData>(MAX_INVEN_COUNT);

        Managers.Game.AddItem("Use", 2);
        Managers.Game.AddItem("Com", 2);

        #endregion


        #region MakeSubItem Use/Com/Vis/Com

        //Use
        if (Managers.Game.UseItemList.Count == 0)
        {
            for (int i = 0; i < MAX_INVEN_COUNT; i++)
            {
                Transform parent = GetObject((int)GameObjects.UseItems).transform;
                var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                item.Init();
                item.SlotItem.color = new Color(1, 1, 1, 0);

            }
        }
        else
        {
            for (int i = 0; i < Managers.Game.UseItemList.Count; i++)
            {
                Transform parent = GetObject((int)GameObjects.UseItems).transform;
                _UseItemData = Managers.Game.UseItemList[i];

                var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                item.Init();
                item.SetItemInfo(_UseItemData);

            }

            if (Managers.Game.UseItemList.Count != MAX_INVEN_COUNT)
            {
                for (int i = Managers.Game.UseItemList.Count; i < MAX_INVEN_COUNT; i++)
                {
                    Transform parent = GetObject((int)GameObjects.UseItems).transform;
                    var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                    item.Init();
                    item.SlotItem.color = new Color(1, 1, 1, 0);

                }
            }

        }


        //Com
        if (Managers.Game.ComItemList.Count == 0)
        {
            for (int i = 0; i < MAX_INVEN_COUNT; i++)
            {
                Transform parent = GetObject((int)GameObjects.ComItems).transform;
                var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                item.Init();
                item.SlotItem.color = new Color(1, 1, 1, 0);

            }
        }
        else
        {
            for (int i = 0; i < Managers.Game.ComItemList.Count; i++)
            {
                Transform parent = GetObject((int)GameObjects.ComItems).transform;
                _ComItemData = Managers.Game.ComItemList[i];

                UI_Inven_Item item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                item.Init();
                item.SetItemInfo(_ComItemData);

            }

            if (Managers.Game.ComItemList.Count != MAX_INVEN_COUNT)
            {
                for (int i = Managers.Game.ComItemList.Count; i < MAX_INVEN_COUNT; i++)
                {
                    Transform parent = GetObject((int)GameObjects.ComItems).transform;
                    var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                    item.Init();
                    item.SlotItem.color = new Color(1, 1, 1, 0);

                }

            }

        }


        //Vis
        if (Managers.Game.VisItemList.Count == 0)
        {
            for (int i = 0; i < MAX_INVEN_COUNT; i++)
            {
                Transform parent = GetObject((int)GameObjects.Vis1).transform;
                var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                item.Init();
                item.SlotItem.color = new Color(1, 1, 1, 0);

            }
            for (int i = this.slotNum; i < (this.slotNum * 2); i++)
            {
                Transform parent = GetObject((int)GameObjects.Vis2).transform;
                var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                item.Init();
                item.SlotItem.color = new Color(1, 1, 1, 0);

            }
            for (int i = (this.slotNum * 2); i < (this.slotNum * 2 + this.remainSlotNum); i++)
            {
                Transform parent = GetObject((int)GameObjects.Vis3).transform;
                var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                item.Init();
                item.SlotItem.color = new Color(1, 1, 1, 0);

            }
        }
        else
        {
            //VisItemList.Count 1~15일때
            if (Managers.Game.VisItemList.Count < 16)
            {
                for (int i = 0; i < Managers.Game.VisItemList.Count; i++)
                {
                    Transform parent = GetObject((int)GameObjects.Vis1).transform;
                    _VisItemData = Managers.Game.VisItemList[i];

                    var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                    item.Init();
                    item.SetItemInfo(_VisItemData);

                }

                //15개라면, 이미 꽉 찼으므로 새로 만들 필요가 없음. 이후 동일 적용.
                if (Managers.Game.VisItemList.Count != MAX_INVEN_COUNT)
                {
                    for (int i = Managers.Game.VisItemList.Count; i < MAX_INVEN_COUNT; i++)
                    {
                        Transform parent = GetObject((int)GameObjects.Vis1).transform;
                        var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                        item.Init();
                        item.SlotItem.color = new Color(1, 1, 1, 0);

                    }
                }

            }

            //VisItemList.Count 16~30일때
            if (Managers.Game.VisItemList.Count < 31 && Managers.Game.VisItemList.Count > 15)
            {
                for (int i = 0; i < Managers.Game.VisItemList.Count; i++)
                {
                    Transform parent = GetObject((int)GameObjects.Vis2).transform;
                    _VisItemData = Managers.Game.VisItemList[i];

                    var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                    item.Init();
                    item.SetItemInfo(_VisItemData);

                }

                if (Managers.Game.VisItemList.Count != 30)
                {
                    for (int i = Managers.Game.VisItemList.Count; i < 31; i++)
                    {
                        Transform parent = GetObject((int)GameObjects.Vis2).transform;
                        var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                        item.Init();
                        item.SlotItem.color = new Color(1, 1, 1, 0);

                    }
                }
            }

            //VisItemList.Count 31~33일때
            if (Managers.Game.VisItemList.Count < 34 && Managers.Game.VisItemList.Count > 30)
            {
                for (int i = 0; i < Managers.Game.VisItemList.Count; i++)
                {
                    Transform parent = GetObject((int)GameObjects.Vis3).transform;
                    _VisItemData = Managers.Game.VisItemList[i];

                    var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                    item.Init();
                    item.SetItemInfo(_VisItemData);

                }

                if (Managers.Game.VisItemList.Count != 33)
                {
                    for (int i = Managers.Game.VisItemList.Count; i < 34; i++)
                    {
                        Transform parent = GetObject((int)GameObjects.Vis3).transform;
                        var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                        item.Init();
                        item.SlotItem.color = new Color(1, 1, 1, 0);

                    }
                }
            }
        }


        //Acc
        if (Managers.Game.AccItemList.Count == 0)
        {
            for (int i = 0; i < MAX_INVEN_COUNT; i++)
            {
                Transform parent = GetObject((int)GameObjects.AccItems).transform;
                var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                item.Init();
                item.SlotItem.color = new Color(1, 1, 1, 0);

            }
        }
        else
        {
            for (int i = 0; i < Managers.Game.AccItemList.Count; i++)
            {
                Transform parent = GetObject((int)GameObjects.AccItems).transform;
                _AccItemData = Managers.Game.AccItemList[i];

                var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                item.Init();
                item.SetItemInfo(_AccItemData);
            }

            if (Managers.Game.AccItemList.Count != MAX_INVEN_COUNT)
            {
                for (int i = Managers.Game.AccItemList.Count; i < MAX_INVEN_COUNT; i++)
                {
                    Transform parent = GetObject((int)GameObjects.AccItems).transform;
                    var item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent);
                    item.Init();
                    item.SlotItem.color = new Color(1, 1, 1, 0);

                }
            }

        }
        #endregion


        SetCurValueText();

        ItemManager.ui_InvenPopup = this;

        GetButton((int)Buttons.VisTabLeftButton).gameObject.SetActive(false);

        nowTab = InvenTab.Usage;
        Showtab(nowTab);
    }

    public void Deacitvate()
    {
        Transform[] UseChilds = GetObject((int)GameObjects.UseItems).GetComponentsInChildren<Transform>();

        Transform[] ComChilds = GetObject((int)GameObjects.ComItems).GetComponentsInChildren<Transform>();

        Transform[] Vis1Childs = GetObject((int)GameObjects.Vis1).GetComponentsInChildren<Transform>();
        Transform[] Vis2Chjlds = GetObject((int)GameObjects.Vis2).GetComponentsInChildren<Transform>();
        Transform[] Vis3Childs = GetObject((int)GameObjects.Vis3).GetComponentsInChildren<Transform>();

        Transform[] AccChilds = GetObject((int)GameObjects.AccItems).GetComponentsInChildren<Transform>();


        foreach (Transform child in UseChilds)
            Managers.Resource.Destroy(child.gameObject);
        foreach (Transform child in ComChilds)
            Managers.Resource.Destroy(child.gameObject);
        foreach (Transform child in Vis1Childs)
            Managers.Resource.Destroy(child.gameObject);
        foreach (Transform child in Vis2Chjlds)
            Managers.Resource.Destroy(child.gameObject);
        foreach (Transform child in Vis3Childs)
            Managers.Resource.Destroy(child.gameObject);
        foreach (Transform child in AccChilds)
            Managers.Resource.Destroy(child.gameObject);

        Managers.UI.ClosePopupUI(this);
    }


    #region Set Item Info , UI Text ( Item Info / Value )

    public void SetItemText(IItemData _iData)
    {
        _ItemData = _iData;
        SetItemText();

    }
    public void SetItemText()
    {
        List<string> _result = new List<string>(); 
        int IDX = _ItemData.GetID;
        ItemDataType _type = _ItemData.GetItemType;


        switch (_type)
        {
            case ItemDataType.Use:
                _UseItemData = Managers.Data.UseItemDict[IDX];
                GetText((int)Texts.UseItemTitleText).text = Managers.GetText(_UseItemData.nameID);
                if (_UseItemData.difFatigue < 0) { _result.Add($"Heal Fatigue {_UseItemData.difFatigue}"); }
                if (_UseItemData.difFatigue > 0) { _result.Add($"<color=red>Damage Fatigue {_UseItemData.difFatigue}</color>"); }
                if (_UseItemData.difHunger < 0) { _result.Add($"Heal Hunger {_UseItemData.difHunger}"); }
                if (_UseItemData.difHunger > 0) { _result.Add($"<color=red>Damage Hunger {_UseItemData.difHunger}</color>"); }
                if (_UseItemData.difStress < 0) { _result.Add($"Heal Stress {_UseItemData.difStress}"); }
                if (_UseItemData.difStress > 0) { _result.Add($"<color = red> Damage Stress { _UseItemData.difStress}</color>"); }

                string _useArray = string.Join("\n", _result);
                GetText((int)Texts.UseItemDetailText).text = _useArray;
                break;

            case ItemDataType.Com:
                _ComItemData = Managers.Data.ComItemDict[IDX];
                GetText((int)Texts.ComItemTitleText).text = Managers.GetText(_ComItemData.nameID);
                if (_ComItemData.difMinHP != 0) { _result.Add($"Heal {_ComItemData.difMinHP} ~ {_ComItemData.difMaxHP} HP"); }

                if (Managers.Data.ComItemDict[4] == _ComItemData)
                { _result.Add("Cleanse All Debuff"); _result.Add($"<color=red>Decrease DGE {_ComItemData.difDGE}</color>");
                    _result.Add($"<color=red>Decrease CRT {_ComItemData.difCRT}%</color>"); }

                if (Managers.Data.ComItemDict[5] == _ComItemData)
                { _result.Add($"Increase ACC {_ComItemData.difACC}%"); _result.Add($"Increase DGE {_ComItemData.difDGE}");
                    _result.Add($"Increase CRT {_ComItemData.difCRT}%"); _result.Add($"<color=red>Decrease SPD{_ComItemData.difSPD}</color>"); }

                string _comArray = string.Join("\n", _result);
                GetText((int)Texts.ComItemDetailText).text = _comArray;

                break;

            case ItemDataType.Vis:
                _VisItemData = Managers.Data.VisItemDict[IDX];
                GetText((int)Texts.VisItemTitleText).text = Managers.GetText(_VisItemData.nameID);

                break;

            case ItemDataType.Acc:
                _AccItemData = Managers.Data.AccItemDict[IDX];
                GetText((int)Texts.AccItemTitleText).text = Managers.GetText(_AccItemData.nameID);

                break;

            default:
                break;
        }

    }

    public void SetCurValueText()
    {

        GetText((int)Texts.UseItemCurValueText).text = $"{Managers.Game.UseItemList.Count}";
        GetText((int)Texts.ComItemCurValueText).text = $"{Managers.Game.ComItemList.Count}";
        GetText((int)Texts.VisItemCurValueText).text = $"{Managers.Game.VisItemList.Count}";
        GetText((int)Texts.AccItemCurValueText).text = $"{Managers.Game.AccItemList.Count}";

    }

    #endregion



    #region On Click Function

    void OnClickCloseButton()
    {
        this.Deacitvate();
    }
    void OnClickVisTabLeftButton()
    {

        if (curVisIndex <= 1)
            return;
        
        curVisIndex--;
        ShowVisTab();
    }
    void OnClickVisTabRightButton()
    {
        if (curVisIndex >= 3)
            return;

        curVisIndex++;
        ShowVisTab();
    }

    //Vis Item의 경우, Parent Transform과 Scale 설정이 필요함. //추가 작업 필요
    void OnClickVisWatchButton()
    {
        Sprite SetInfo(VisItemData Data)
        {
            return Managers.Resource.Load<Sprite>(Data.detailPath);
        }
        
    }
    #endregion

    #region Tab System ( Logic , Vis Only )
    public void ShowVisTab()
    {
        Debug.Log(curVisIndex);

        GetObject((int)GameObjects.Vis1).SetActive(false);
        GetObject((int)GameObjects.Vis2).SetActive(false);
        GetObject((int)GameObjects.Vis3).SetActive(false);

        nowTab = InvenTab.Visible;

        switch (curVisIndex)
        {
            case 1:
                GetObject((int)GameObjects.Vis1).SetActive(true);
                GetText((int)Texts.VisItemTitleText).text = "";

                GetButton((int)Buttons.VisTabLeftButton).gameObject.SetActive(false);
                break;                                     
            case 2:                                        
                GetObject((int)GameObjects.Vis2).SetActive(true);
                GetText((int)Texts.VisItemTitleText).text = "";

                break;                                    
            case 3:                                       
                GetObject((int)GameObjects.Vis3).SetActive(true);
                GetText((int)Texts.VisItemTitleText).text = "";
                GetButton((int)Buttons.VisTabRightButton).gameObject.SetActive(false);
                break;
        }

        if (curVisIndex == 3)
        {
            //Right 비활성화
            GetButton((int)Buttons.VisTabRightButton).gameObject.SetActive(false);
        }
        else if (curVisIndex < 2)
        {
            GetButton((int)Buttons.VisTabLeftButton).gameObject.SetActive(false);
        }
        else
        {
            GetButton((int)Buttons.VisTabRightButton).gameObject.SetActive(true);
            GetButton((int)Buttons.VisTabLeftButton).gameObject.SetActive(true);
        }
    }
   

    public void Showtab(InvenTab tab)
    {
        if (nowTab != tab)
            ItemManager.OnChangedInvenTab = true;

        nowTab = tab;

        GetObject((int)GameObjects.UseTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.ComTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.VisTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.AccTab).gameObject.SetActive(false);
   
        switch (nowTab)
        {
            case InvenTab.Usage:
                GetObject((int)GameObjects.UseTab).gameObject.SetActive(true);
                GetText((int)Texts.UseItemTitleText).text = "";
                GetText((int)Texts.UseItemDetailText).text = "";

                break;

            case InvenTab.Combat:
                GetObject((int)GameObjects.ComTab).gameObject.SetActive(true);
                GetText((int)Texts.ComItemTitleText).text = "";
                GetText((int)Texts.ComItemDetailText).text = "";
                break;

            case InvenTab.Visible:
                GetObject((int)GameObjects.VisTab).gameObject.SetActive(true);
                GetText((int)Texts.VisItemTitleText).text = "";
                curVisIndex = 1;
                ShowVisTab();
                break;

            case InvenTab.Accessories:
                GetObject((int)GameObjects.AccTab).gameObject.SetActive(true);
                GetText((int)Texts.AccItemTitleText).text = "";
                GetText((int)Texts.AccItemDetailText).text = "";
                break;
        }

    }
    #endregion

}