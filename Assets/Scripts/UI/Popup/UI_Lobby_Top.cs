using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;
using TMPro;

public class UI_Lobby_Top : UI_Popup
{
    enum Texts
    {
        BagButtonText,
        TeamButtonText,
        MenuButtonText,
        FatigueText,
        FatigueCurText,
        FatigueMaxText,
        HungerText,
        HungerCurText,
        HungerMaxText,
        StressText,
        StressCurText,
        StressMaxText,
        FloorCurText,
        FloorText,

    }

    enum Buttons
    {
        BagButton,
        TeamButton,
        MenuButton,

    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    { 
        base.Init();

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BagButton).gameObject.BindEvent(OnClickBagButton);
        GetButton((int)Buttons.TeamButton).gameObject.BindEvent(OnClickTeamButton);
        GetButton((int)Buttons.MenuButton).gameObject.BindEvent(OnClickMenuButton);

        if(Managers.Game.SaveData.IsFirstStart == false)
        {
            Managers.UI.CloseAllPopupUI();
            Managers.UI.ShowPopupUI<UI_DialoguePopup>();
            //이때 그 DialogueTest에 해당하는 , IntroDialogue가 실행되어야 함.

            //Managers.Game.SaveData.IsFirstStart = true; -> DialoguePopup에 있는 IntroDialogue에서 true로 바꿔주기로 하자.
        }
    }

    void OnClickBagButton(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_InvenPopup>();
    }
    
    void OnClickTeamButton(PointerEventData data)
    {
        Debug.Log("Button Clicked");
    }

    void OnClickMenuButton(PointerEventData data)
    {
        Debug.Log("Button Clicked");
        Managers.UI.ShowPopupUI<UI_MenuPopup>();

    }

}