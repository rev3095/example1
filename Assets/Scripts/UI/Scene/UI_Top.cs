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

    public override void Init()
    { 
        base.Init();
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BagButton).gameObject.BindEvent(OnClickBagButton);
        GetButton((int)Buttons.TeamButton).gameObject.BindEvent(OnClickTeamButton);
        GetButton((int)Buttons.MenuButton).gameObject.BindEvent(OnClickMenuButton);

        Managers.Game.Top_Script = this;
    }

    public void Refresh()
    {
        GetText((int)Texts.FatigueCurText).text = $"{Managers.Game.CurFatigue}";
        GetText((int)Texts.HungerCurText).text = $"{Managers.Game.CurHunger}";
        GetText((int)Texts.StressCurText).text = $"{Managers.Game.CurStress}";

    }

    #region OnClick Function
    void OnClickBagButton(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_InvenPopup>();
    }
    
    void OnClickTeamButton(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_TeamPopup>();
    }

    void OnClickMenuButton(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_MenuPopup>();

    }
    #endregion

}