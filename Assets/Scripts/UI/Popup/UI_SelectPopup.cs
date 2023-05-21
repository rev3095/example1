using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using static Define;
using System.Linq;
using TMPro;

public class UI_SelectPopup: UI_Popup
{
    public TMP_Text[] slotText; // 슬롯버튼 아래에 존재하는 Text들



    public string nowtime;

    bool[] saveFile = new bool[4]; //세이브 파일 존재 유무 저장
    enum Texts
    {
        LoadText,
        LoadSlotText_1,
        LoadSlotText_2,
        LoadSlotText_3,
        LoadSlotText_4,
        PlayButtonText,
        CopyButtonText,
        DeleteButtonText,
        ExitButtonText,
    }

    enum Buttons
    {
        PlayButton,
        CopyButton,
        DeleteButton,
        ExitButton,
        LoadSlot_1,
        LoadSlot_2,
        LoadSlot_3,
        LoadSlot_4,
    }

    void Start()
    {
        Init();

        RefreshUI();
    }

    public void Update()
    {
        nowtime = DateTime.Now.ToString(("yyyy-MM-dd"));


    }


    public override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.PlayButton).gameObject.BindEvent(OnClickPlayButton);
        GetButton((int)Buttons.CopyButton).gameObject.BindEvent(OnClickCopyButton);
        GetButton((int)Buttons.DeleteButton).gameObject.BindEvent(OnClickDeleteButton);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.LoadSlot_1).gameObject.BindEvent(OnClickLoadSlot_1);
        GetButton((int)Buttons.LoadSlot_2).gameObject.BindEvent(OnClickLoadSlot_2);
        GetButton((int)Buttons.LoadSlot_3).gameObject.BindEvent(OnClickLoadSlot_3);
        GetButton((int)Buttons.LoadSlot_4).gameObject.BindEvent(OnClickLoadSlot_4);

        /*
        GetText((int)Texts.LoadSlotText_1).text = Managers.Game.Name + " " + GameManagerEx.TimeUtils.GetCurrentDate() + " " + Managers.Game.CurFloor +"F"+ " " + Managers.Game.Difficulty;
        GetText((int)Texts.LoadSlotText_2).text = Managers.Game.Name + " " + nowtime.ToString("yyyy-MM-dd") + " " + Managers.Game.CurFloor + " " + Managers.Game.Difficulty;
        GetText((int)Texts.LoadSlotText_3).text = Managers.Game.Name + " " + nowtime.ToString("yyyy-MM-dd") + " " + Managers.Game.CurFloor + " " + Managers.Game.Difficulty;
        GetText((int)Texts.LoadSlotText_4).text = Managers.Game.Name + " " + nowtime.ToString("yyyy-MM-dd") + " " + Managers.Game.CurFloor + " " + Managers.Game.Difficulty;
        */

    }

    //이름 가져올때 데이터 로드하는데, 이름 가져올려고 로드한거기 때문에 Init()을 해줘야함.
    void OnClickPlayButton(PointerEventData data)
    {

        if (Managers.Game.nowSlot > -1)
        {

            //bool 배열에서 현재 슬롯번호가 true라면 = 데이터 존재한다는 뜻

            if (saveFile[Managers.Game.nowSlot])
            {
                Managers.Game.LoadGame();
                GoLobbyPopup();
            }
            // bool 배열에서 현재 슬롯번호가 false라면 데이터가 없다는 뜻

            else
            {

                saveFile[Managers.Game.nowSlot] = true;


                //여기서, GoLobbyPopup()에서 time 저장하던걸 실행
                Managers.Game.Init();
                Managers.Game.SaveData.time = nowtime;
                Managers.Game.SaveGame();
                Managers.UI.ShowPopupUI<UI_NamePopup>();


            }

        }


    }

    private bool isClickCopy = false;
    private int copyslot;

    void OnClickCopyButton(PointerEventData data)
    {
        if(Managers.Game.nowSlot > -1)
        {
            copyslot = Managers.Game.nowSlot;
            isClickCopy = true;
        }

    }
    void OnClickDeleteButton(PointerEventData data)
    {
        Debug.Log("OnClicked");
        Managers.Game.DeleteGame();
        saveFile[Managers.Game.nowSlot] = false;

        RefreshUI();
    }

    void OnClickExitButton(PointerEventData data)
    {
        Managers.UI.ClosePopupUI(this);
        SelectAction?.Invoke();


    }

    void CopyData()
    {
        if (isClickCopy)
        {
            if (Managers.Game.nowSlot != copyslot)
            {
                Managers.Game.CopyGame(copyslot, Managers.Game.nowSlot);
                RefreshUI();
            }
        }
        isClickCopy = false;
    }

    void OnClickLoadSlot_1(PointerEventData data)
    {
        //세이브데이터를 json으로 저장할 때, 이 슬롯이 클릭됐을 때, 
        //nowSlot의 번호를 얘가 클릭되었을때 1로 하고,

        //데이터가 있더라도 로드할때 데이터 번호를 nowSlot으로 찾기 때문에 동일.
      

        Managers.Game.nowSlot = 0;
        Debug.Log(Managers.Game.nowSlot = 0);

        CopyData();
    }


    void OnClickLoadSlot_2(PointerEventData data)
    {
        Managers.Game.nowSlot = 1;
        Debug.Log(Managers.Game.nowSlot = 1);


        CopyData();
    }
    void OnClickLoadSlot_3(PointerEventData data)
    {
        Managers.Game.nowSlot = 2;
        Debug.Log(Managers.Game.nowSlot = 2);


        CopyData();
    }
    void OnClickLoadSlot_4(PointerEventData data)
    {
        Managers.Game.nowSlot = 3;
        Debug.Log(Managers.Game.nowSlot = 3);


        CopyData();
    }

    public void Slot(int number)
    {
        Managers.Game.nowSlot = number;

    }

    //UI SaveSlot 표시용 임시 데이터 처리
    public void RefreshUI() {

        GameData TempData = new GameData();
        //if (IsNormal == true) if (isHard ==true);로 return 코드 작성 (밸런스는 Enemy 스탯 1.5배 높여서 스탯 데이터 작성)
        for (int i = 0; i < 4; i++)
        {
            if (File.Exists(Managers.Game._path + $"{i}" + ".json"))  // 슬롯 데이터 존재시, 슬롯 번호 및 PlayerName 표시
            {
                saveFile[i] = true;
                Managers.Game.nowSlot = i;
                Managers.Game.LoadGame(); //이름값 불러오려고 LoadGame 시키는거!

                TempData = Managers.Game.SaveData;
                // slotText[i].text = Managers.Game.SaveData.Name + " " + Managers.Game.SaveData.time + " " + Managers.Game.SaveData.CurFloor + "F" + " " + Managers.Game.SaveData.Difficulty;
                slotText[i].text = TempData.Name + " " + TempData.time + " " + TempData.CurFloor + "F" + " " + TempData.Difficulty;
            }
            else // 슬롯 데이터가 없을 때
            {
                slotText[i].text = "Empty";
            }

            TempData = new();
        }
        //Managers.Game.DataClear(); // 슬롯에 표기된 데이터 초기화
        Managers.Game.nowSlot = -1;
        //오늘코딩씨는 Json파일 (startdata)을 가져올 필요가 없어서 DataClear를 했던 거임, 지금은 쓰면 날라가서 X
    }

    public void GoLobbyPopup()
    {
        nowtime = GameManagerEx.TimeUtils.GetCurrentDate();
        Managers.Game.SaveData.time = nowtime;
        Managers.Game.SaveGame();


        Managers.UI.CloseAllPopupUI();
        //Managers.UI.ShowPopupUI<UI_Lobby_10F>();
        SceneManager.LoadScene("LobbyScene");

    }

    public Action SelectAction;
    public void SetInfo(Action action)
    {
        SelectAction = action;
    }

}