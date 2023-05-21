using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InvenPopup : UI_Popup
{
    //Texts를 적당히 줄이되, 탭 별 텍스트에 접근하는게 아니라 하나씩 바인드하는게 맞을까?
    //대신 이렇게하면 개발 난이도가 쉬워지고, enable로 제어하기 때문에 SetActive에서의 바인딩 오류 문제가 일어나지 않는다.
    //kor replace할때 , GetText를 사용한다면, 나머지 세부 Text까지 전부 Binding 작업 필요함.
    enum Texts
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
        AccelerationTabButton,

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
        VisItems,
        AccItems,

    }

    //Define으로 빼도 될듯
    public enum InvenTab
    {
        None = 0,
        Usage,
        Combat,
        Visible,
        Acceleration
    }

    private InvenTab _tab = InvenTab.None;
    private int curVisIndex = 0;


    public void Start()
    {
        Init();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Managers.UI.ClosePopupUI(this);
        }
    }

    public override void Init()
    {
        base.Init();

        //Bind
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);

        GetButton((int)Buttons.UsageTabButton).gameObject.BindEvent((PointerEventData data) => Showtab(InvenTab.Usage));
        GetButton((int)Buttons.CombatTabButton).gameObject.BindEvent((PointerEventData data) => Showtab(InvenTab.Combat));
        GetButton((int)Buttons.VIsibleTabButton).gameObject.BindEvent((PointerEventData data) => Showtab(InvenTab.Visible));
        GetButton((int)Buttons.AccelerationTabButton).gameObject.BindEvent((PointerEventData data) => Showtab(InvenTab.Acceleration));

        GetButton((int)Buttons.VisTabLeftButton).gameObject.BindEvent(OnClickVisTabLeftButton);
        GetButton((int)Buttons.VisTabRightButton).gameObject.BindEvent(OnClickVisTabRightButton);
        GetButton((int)Buttons.VisWatchButton).gameObject.BindEvent(OnClickVisWatchButton);

        //Inventory 선행 코드
        GameObject[] GridPanels = new GameObject[4];

        GameObject UseGridPanel = GetObject((int)GameObjects.UseItems); GridPanels[0] = UseGridPanel;
        GameObject ComGridPanel = GetObject((int)GameObjects.ComItems); GridPanels[1] = ComGridPanel;
        GameObject VisGridPanel = GetObject((int)GameObjects.VisItems); GridPanels[2] = VisGridPanel;
        GameObject AccGridPanel = GetObject((int)GameObjects.AccItems); GridPanels[3] = AccGridPanel;
        
        ////먼저 자식 인벤토리 요소들(gameObject) 날리고 시작함.
        //foreach (GameObject go in GridPanels)
        //{
        //    foreach(Transform child in go.transform)
        //    {
        //        Managers.Resource.Destroy(child.gameObject);
        //    }
        //}

        ////실제 아이템 데이터 설정, 시작 아이템 1개씩 제공 (Use,Com)
        //foreach (GameObject go in GridPanels)
        //{
        //    for (int i = 0; i < 2; i++)
        //    {
        //        GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(go.transform).gameObject; // GridPanel의 자식으로, 인벤토리 슬롯 프리펩 생성 (MakeSubItem 팩토리 이용)
        //        UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();

        //        invenItem.SetInfo();
        //    }

        //}

        //VisTab 선행 코드
        GetObject((int)GameObjects.Vis1).SetActive(false);
        GetObject((int)GameObjects.Vis2).SetActive(false);
        GetObject((int)GameObjects.Vis3).SetActive(false);

        GetButton((int)Buttons.VisTabLeftButton).gameObject.SetActive(false);

        Showtab(InvenTab.Usage);
    }

    #region OnClick Function
    void OnClickCloseButton(PointerEventData data)
    {
        Managers.UI.ClosePopupUI(this);
    }
    void OnClickVisTabLeftButton(PointerEventData data)
    {

        if (curVisIndex <= 1)
            return;
        
        curVisIndex--;
        ShowVisTab();
    }
    void OnClickVisTabRightButton(PointerEventData data)
    {
        if (curVisIndex >= 3)
            return;

        curVisIndex++;
        ShowVisTab();
    }
    void OnClickVisWatchButton(PointerEventData data)
    {
        
    }

    #endregion

    void ShowVisTab()
    {
        Debug.Log(curVisIndex);

        GetObject((int)GameObjects.Vis1).SetActive(false);
        GetObject((int)GameObjects.Vis2).SetActive(false);
        GetObject((int)GameObjects.Vis3).SetActive(false);

        switch (curVisIndex)
        {
            case 1:
                GetObject((int)GameObjects.Vis1).SetActive(true);
                GetButton((int)Buttons.VisTabLeftButton).gameObject.SetActive(false);
                break;                                     
            case 2:                                        
                GetObject((int)GameObjects.Vis2).SetActive(true);
                break;                                    
            case 3:                                       
                GetObject((int)GameObjects.Vis3).SetActive(true);
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
        if (_tab == tab)
            return;
        _tab = tab;

        GetObject((int)GameObjects.UseTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.ComTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.VisTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.AccTab).gameObject.SetActive(false);

   
        switch (_tab)
        {
            case InvenTab.Usage:
                GetObject((int)GameObjects.UseTab).gameObject.SetActive(true);
                break;

            case InvenTab.Combat:
                GetObject((int)GameObjects.ComTab).gameObject.SetActive(true);
                break;

            case InvenTab.Visible:
                GetObject((int)GameObjects.VisTab).gameObject.SetActive(true);
                curVisIndex = 1;
                ShowVisTab();
                break;

            case InvenTab.Acceleration:
                GetObject((int)GameObjects.AccTab).gameObject.SetActive(true);
                break;
        }

    }

    public void SetInfo()
    {

    }


}