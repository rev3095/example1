using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static Define;
using TMPro;


public class UI_MoveEventPopup : UI_Popup
{
    enum Buttons
    {
        ConfirmButton,
    }

    enum Texts
    {
        DetailText,
    }

    MoveEventData _MoveEventData = new MoveEventData();


    //오류날때는 start init 빼고, 그냥 setinfo에서 init까지 할 수 있도록 정리.


    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(PointerEventData => OnClickConfirmButton());

        GetText((int)Texts.DetailText).text = "";

    }

    public void SetInfo(MoveEventData data)
    {
        Init();
        _MoveEventData = data;
        Refresh();
    }

    void Refresh()
    {
        GetText((int)Texts.DetailText).text = Managers.GetText(_MoveEventData.nameID);

        if(_MoveEventData.isBattle == true)
        {
            //Managers.Game.GoBattleScene(); //여기서 변수 Mangers.Game에대한 변수 세팅 후 게임에서 씬 이동 + 배틀 프리펩 생성 및 SetInfo
        }
        else
        {
            Managers.Game.SaveData.CurFatigue += _MoveEventData.difFatigue;
            Managers.Game.SaveData.CurHunger += _MoveEventData.difHunger;
            Managers.Game.SaveData.CurStress += _MoveEventData.difStress;
        }

        Managers.Game.RefreshLobbyUI();

        //nowSlot이 지정되었어야되는데 바로 불러오면 ㅈ되는듯.
        Managers.Game.SaveGame();

    }

    void OnClickConfirmButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
