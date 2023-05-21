using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;
using TMPro;


//UIManager : canvas.renderMode != RenderMode.ScreenSpaceOverlay
public class UI_Top : UI_Scene
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

    public enum Buttons
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